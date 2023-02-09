using ProjectBonsai.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using static Codice.Client.Common.WebApi.WebApiEndpoints;
using static ItemData;
using static TerrainData;
using static UnityEngine.UI.GridLayoutGroup;

namespace ProjectBonsai.AI
{
    [RequireComponent(typeof(StateController))]
    public class AiController : MonoBehaviour, IDamagable
    {
        #region Class Variables

        [field: SerializeField]
        public StateController StateController { get; private set; } = null!;

        public Animator animator;
        public AiData aiData;
        public LayerMask terainMask;

        public float timer = 0f;
        public float wanderRadius = 50f;
        public float wanderTimer = 10f;
        public float turnSpeed = 3f;
        public float rotateSpeed = 0.5f;
        public float beginTurnAngle = 30f;
        public float maxTurnAngle = 120f;
        public float turnRadius = 90f;

        [HideInInspector] public float runSpeed;

        private bool pathIndexChanged = false;
        private bool createNewPath = true;
        private float angleToPathPoint = 1000f;
        private Vector3 agentVelocity = Vector3.zero;

        [SerializeField]
        private bool UsePathSmoothing;
        [Header("Path Smoothing")]
        [SerializeField]
        private float SmoothingLength = 0.25f;
        [SerializeField]
        private int SmoothingSections = 10;
        [SerializeField]
        [Range(-1, 1)]
        private float SmoothingFactor = 0;
        public NavMeshAgent agent;
        private NavMeshPath currentPath;
        public Vector3[] PathLocations = new Vector3[0];
        [SerializeField]
        private int currentPathIdx = 0;

        [Header("Movement Configuration")]
        [SerializeField]
        [Range(0, 0.99f)]
        private float Smoothing = 0.25f;

        [SerializeField]
        private Vector3 TargetDirection;
        [SerializeField]
        private Vector3 MovementVector;
        private Vector3 InfinityVector = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

        private int dummyVar = 0;

        #region Health variables

        [Header("Health Variables")]

        [SerializeField] public GameObject healthBar;
        [SerializeField] public GameObject healthTotalBar;
        [SerializeField] public GameObject healRemainingBar;
        [SerializeField] public GameObject labelPosition_go;
        [SerializeField] public TextMeshProUGUI healthText;

        [HideInInspector] GameObject itemSpawns;
        [HideInInspector] Camera playerCamera;
        [HideInInspector] float healthbarStartWidth;
        [HideInInspector] float currentHealth;
        [HideInInspector] float timeHealthbarVisible;
        [HideInInspector] float timeHealthbarVisibleRemaining;

        [SerializeField]
        private ItemEnum[] ItemsCanDamage;
        public ItemEnum[] itemsCanDamage { get => ItemsCanDamage; set { ItemsCanDamage = value; } }

        #endregion

        #endregion

        #region Monobehavior
        private void Awake()
        {
            InitSOvalues();

            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = true;
            agent.updatePosition = true;
            agent.updateUpAxis = false;
            agent.isStopped = false;
            agent.velocity = Vector3.zero;
            agent.speed = aiData.BaseSpeed;
            runSpeed = aiData.BaseSpeed * aiData.RunSpeedMultiplier;
        }

        private void Start()
        {
            currentPath = new NavMeshPath();

            itemSpawns = Core.FindGameObjectByNameAndTag("ItemSpawns", "ItemSpawns");
            playerCamera = Player.Instance.playerCamera.GetComponent<Camera>();

            Vector3 screenPoint = playerCamera.WorldToScreenPoint(labelPosition_go.transform.position);
            healthBar.transform.position = screenPoint;
            healthbarStartWidth = healRemainingBar.GetComponent<RectTransform>().sizeDelta.x;
            currentHealth = aiData.Health;
            timeHealthbarVisible = 5f;
            timeHealthbarVisibleRemaining = 0f;
        }

        private void InitSOvalues()
        {
            aiData.aiType.aiEnum = AiType.AiEnum.Boar;
        }


        // Update is called once per frame
        void Update()
        {
            if (timer >= wanderTimer)
            {
                createNewPath = true;
            }

            if (createNewPath)
            {
                UpdateAgentPath(true);
            }
            else if (dummyVar >= currentPath.corners.Length) // Loop through all existing points first
            {
                UpdateAgentPath(false);
                dummyVar = 0;
            }

            MoveAgent();
            FaceTarget();
            Align();
            SetAggression();

            UpdateHealthbar();

            timer += Time.deltaTime;
            dummyVar += 1;
        }

        private void LateUpdate()
        {
            pathIndexChanged = false;
        }

        #endregion

        #region Agent Movement & Behavior

        private void UpdateAgentPath(bool generateNewPoint)
        {

            Vector3 newPos = new Vector3();
            if (generateNewPoint)
            {
                newPos = GetNewNavPoint();
                timer = 0;
            }
            else
            {
                newPos = currentPath.corners[currentPath.corners.Length - 1];
            }

            agent.CalculatePath(newPos, currentPath);
            pathIndexChanged = true;
            createNewPath = false;
            currentPathIdx = 0;
            List<State> activeState = StateController.ActiveStates;
            StateController.TransitionToState<AiWalkState>(activeState[0]);
            PathLocations = currentPath.corners;

            if (UsePathSmoothing)
            {
                if (currentPath.corners.Length > 2)
                {
                    BezierCurve[] curves = new BezierCurve[PathLocations.Length - 1];
                    SmoothCurves(curves, PathLocations);
                    PathLocations = GetPathLocations(curves);
                    currentPathIdx = 0;
                }
                else
                {
                    PathLocations = currentPath.corners;
                    currentPathIdx = 0;
                }
            }
        }

        public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
        {
            Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;
            randDirection += origin;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
            return navHit.position;
        }

        private Vector3 GetNewNavPoint()
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            int idx = 0;
            while (Vector3.Angle(transform.forward, newPos - transform.position) > maxTurnAngle)
            {

                newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                idx++;
                if (idx > 100)
                {
                    Debug.Log("wasn't able to find position within max angle");
                    break;
                }
            }

            return newPos;
        }

        private void Align()

        {
            Vector3 theRay = -transform.up;
            RaycastHit hit;


            if (Physics.Raycast(transform.position, theRay, out hit, 20, terainMask))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.red);

                Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, transform.eulerAngles.y, targetRotation.eulerAngles.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed);
            }
        }

        private void FaceTarget()
        {

/*            if ((!pathIndexChanged && angleToPathPoint < 0.05f) || currentPathIdx >= currentPath.corners.Length)
                return;*/

            if (currentPathIdx >= PathLocations.Length)
                return;

            Vector3 currentPathPoint = PathLocations[currentPathIdx];
            Vector3 direction = (currentPathPoint - transform.position).normalized;
            Vector3 newDirection = new Vector3(direction.x, 0, direction.z);
            if (newDirection == Vector3.zero)
            {
                return;
            }

            Quaternion lookRotation = Quaternion.LookRotation(newDirection);
            lookRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            Quaternion quat1 = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, 0f));
            Quaternion quat2 = Quaternion.Euler(new Vector3(0f, lookRotation.eulerAngles.y, 0f));
            angleToPathPoint = Quaternion.Angle(quat1, quat2);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed);
        }

        private void MoveAgent()
        {
            if (currentPathIdx >= PathLocations.Length)
            {
                return;
            }

            if (Vector3.Distance(transform.position, PathLocations[currentPathIdx] + (agent.baseOffset * Vector3.up)) <= agent.radius*1.2f)
            {
                currentPathIdx++;
                pathIndexChanged = true;
                angleToPathPoint = 1000f;

                if (currentPathIdx >= PathLocations.Length)
                {
                    // Reached destination
                    List<State> activeState = StateController.ActiveStates;
                    StateController.TransitionToState<AiIdleState>(activeState[0]);
                    createNewPath = true;
                    return;
                }
            }

            Vector3 currentPathVector = (PathLocations[currentPathIdx] + (10f * Vector3.up) - transform.position).normalized;
            Vector3 movementVector = Vector3.RotateTowards(transform.forward, currentPathVector.normalized, 3.14f / turnRadius, 1f);
            agentVelocity = (movementVector * agent.speed * Time.fixedDeltaTime);
            agent.velocity = agentVelocity;

            DrawAgentPath();
        }

        private void SetAggression()
        {
            if (Vector3.Distance(Player.Instance.transform.position, transform.position) <= aiData.aggressionEnterRange && !StateController.IsStateActive<AiSprintState>())
            {
                List<State> activeState = StateController.ActiveStates;
                StateController.TransitionToState<AiSprintState>(activeState[0]);
                if (currentPathIdx >= PathLocations.Length - 1)
                {
                    createNewPath = true;
                }
            }
            else if (Vector3.Distance(Player.Instance.transform.position, transform.position) >= aiData.aggressionExitRange && !StateController.IsStateActive<AiWalkState>() && !StateController.IsStateActive<AiIdleState>())
            {
                List<State> activeState = StateController.ActiveStates;
                StateController.TransitionToState<AiWalkState>(activeState[0]);
            }
        }

        #endregion

        #region Debug

        private void DrawAgentPath()
        {
            for (int i = 0; i < PathLocations.Length - 1; i++)
            {
                Debug.DrawLine(PathLocations[i], PathLocations[i + 1], Color.blue);
            }
            Debug.DrawLine(transform.position, PathLocations[currentPathIdx], Color.yellow);
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < PathLocations.Length - 1; i++)
            {
                Gizmos.DrawWireSphere(PathLocations[i], 0.5f);
            }
        }

        #endregion

        #region Curved Pathfinding
        private void SmoothCurves(BezierCurve[] Curves, Vector3[] Corners)
        {
            for (int i = 0; i < Curves.Length; i++)
            {
                if (Curves[i] == null)
                {
                    Curves[i] = new BezierCurve();
                }

                Vector3 position = Corners[i];
                Vector3 lastPosition = i == 0 ? Corners[i] : Corners[i - 1];
                Vector3 nextPosition = Corners[i + 1];

                Vector3 lastDirection = (position - lastPosition).normalized;
                Vector3 nextDirection = (nextPosition - position).normalized;

                Vector3 startTangent = (lastDirection + nextDirection) * SmoothingLength;
                Vector3 endTangent = (nextDirection + lastDirection) * -1 * SmoothingLength;

                Curves[i].Points[0] = position; // Start Position (P0)
                Curves[i].Points[1] = position + startTangent; // Start Tangent (P1)
                Curves[i].Points[2] = nextPosition + endTangent; // End Tangent (P2)
                Curves[i].Points[3] = nextPosition; // End Position (P3)
            }


            // Apply look-ahead for first curve and retroactively apply the end tangent
            {
                Vector3 nextDirection = (Curves[1].EndPosition - Curves[1].StartPosition).normalized;
                Vector3 lastDirection = (Curves[0].EndPosition - Curves[0].StartPosition).normalized;

                Curves[0].Points[2] = Curves[0].Points[3] +
                    (nextDirection + lastDirection) * -1 * SmoothingLength;
            }
        }

        private Vector3[] GetPathLocations(BezierCurve[] Curves)
        {
            Vector3[] pathLocations = new Vector3[Curves.Length * SmoothingSections];

            int index = 0;
            for (int i = 0; i < Curves.Length; i++)
            {
                Vector3[] segments = Curves[i].GetSegments(SmoothingSections);
                for (int j = 0; j < segments.Length; j++)
                {
                    pathLocations[index] = segments[j];
                    index++;
                }
            }

            pathLocations = PostProcessPath(Curves, pathLocations);

            return pathLocations;
        }

        private Vector3[] PostProcessPath(BezierCurve[] Curves, Vector3[] Path)
        {
            Vector3[] path = RemoveOversmoothing(Curves, Path);

            path = RemoveTooClosePoints(path);

            path = SamplePathPositions(path);

            return path;
        }

        private Vector3[] SamplePathPositions(Vector3[] Path)
        {
            for (int i = 0; i < Path.Length; i++)
            {
                if (NavMesh.SamplePosition(Path[i], out NavMeshHit hit, agent.radius * 4f, agent.areaMask))
                {
                    Path[i] = hit.position;
                }
                else
                {
                    Debug.LogWarning($"No NavMesh point close to {Path[i]}. Check your smoothing settings!");
                    Path[i] = InfinityVector;
                }
            }

            return Path.Except(new Vector3[] { InfinityVector }).ToArray();
        }

        private Vector3[] RemoveTooClosePoints(Vector3[] Path)
        {
            if (Path.Length <= 2)
            {
                return Path;
            }

            int lastIndex = 0;
            int index = 1;
            for (int i = 0; i < Path.Length - 1; i++)
            {
                if (Vector3.Distance(Path[index], Path[lastIndex]) <= agent.radius)
                {
                    Path[index] = InfinityVector;
                }
                else
                {
                    lastIndex = index;
                }
                index++;
            }

            return Path.Except(new Vector3[] { InfinityVector }).ToArray();
        }

        private Vector3[] RemoveOversmoothing(BezierCurve[] Curves, Vector3[] Path)
        {
            if (Path.Length <= 2)
            {
                return Path;
            }

            int index = 1;
            int lastIndex = 0;
            for (int i = 0; i < Curves.Length; i++)
            {
                Vector3 targetDirection = (Curves[i].EndPosition - Curves[i].StartPosition).normalized;

                for (int j = 0; j < SmoothingSections - 1; j++)
                {
                    Vector3 segmentDirection = (Path[index] - Path[lastIndex]).normalized;
                    float dot = Vector3.Dot(targetDirection, segmentDirection);
                    // Debug.Log($"Target Direction: {targetDirection}. segment direction: {segmentDirection} = dot {dot} with index {index} & lastIndex {lastIndex}");
                    if (dot <= SmoothingFactor)
                    {
                        Path[index] = InfinityVector;
                    }
                    else
                    {
                        lastIndex = index;
                    }

                    index++;
                }

                index++;
            }

            Path[Path.Length - 1] = Curves[Curves.Length - 1].EndPosition;

            Vector3[] TrimmedPath = Path.Except(new Vector3[] { InfinityVector }).ToArray();

            // Debug.Log($"Original Smoothed Path: {Path.Length}. Trimmed Path: {TrimmedPath.Length}");

            return TrimmedPath;
        }

        #endregion

        #region Handle Damage

        public void UpdateHealthbar()
        {
            // Update health bar
            if (healthBar.activeSelf)
            {
                float healthbarWidth = (currentHealth / aiData.Health) * healthbarStartWidth;
                healRemainingBar.GetComponent<RectTransform>().sizeDelta = new Vector2(healthbarWidth, healRemainingBar.GetComponent<RectTransform>().sizeDelta.y);
                healthText.text = currentHealth.ToString() + "/" + aiData.Health.ToString();
                PositionHealthLabel();
            }

            if (timeHealthbarVisibleRemaining >= 0f)
            {
                timeHealthbarVisibleRemaining -= Time.deltaTime;
            }
            else
            {
                healthBar.SetActive(false);
            }
        }

        public void PositionHealthLabel()
        {
            Vector3 screenPoint = playerCamera.WorldToScreenPoint(labelPosition_go.transform.position);
            healthBar.transform.position = screenPoint;
            if (screenPoint.z < 0)
            {
                healthBar.SetActive(false);
            }
            else if (screenPoint.z >= 0 && !healthBar.activeSelf)
            {
                healthBar.SetActive(true);
            }
        }

        public void DestroyObject()
        {
            Vector3 objectPosition = this.gameObject.transform.position;
            Destroy(this.transform.parent.gameObject);

            // Spawn resources
            ItemData.ItemEnum[] itemEnums = aiData.ResourceTypes;
            for (int i = 0; i < itemEnums.Length; i++)
            {
                print(GlobalData.itemsPrefabPath + ItemData.itemDict[itemEnums[i]].modelRef);
                GameObject resource = Resources.Load<GameObject>(GlobalData.itemsPrefabPath + ItemData.itemDict[itemEnums[i]].modelRef);
                GameObject resourceInst = Instantiate(resource, itemSpawns.transform);
                resourceInst.transform.position = objectPosition;
                resourceInst.GetComponent<Rigidbody>().velocity = new Vector3(UnityEngine.Random.Range(-1f, 1f),
                                                                              UnityEngine.Random.Range(4f, 5f),
                                                                              UnityEngine.Random.Range(-1f, 1f));
                resourceInst.GetComponent<Item>().SetNumItem(aiData.NumResources[i]);
            }
        }

        public float Damage(float damage)
        {
            GameObject particleHit = aiData.ParticleHitName;
            GameObject particleSystem_go = Instantiate(particleHit, playerCamera.transform);
            particleSystem_go.transform.localPosition = Player.Instance.weaponCollider.center;

            currentHealth -= damage;
            PositionHealthLabel();
            timeHealthbarVisibleRemaining = timeHealthbarVisible;
            if (currentHealth <= 0f)
            {
                DestroyObject();
                return 0f;
            }

            return currentHealth;
        }

        public bool CanDamage(ItemData.ItemEnum itemEnum)
        {
            print(itemsCanDamage.Contains(ItemData.ItemEnum.AnyWeapon));
            print(ItemData.itemDict[itemEnum].itemType == ItemData.ItemType.Weapon);
            if (itemsCanDamage.Contains(ItemData.ItemEnum.AnyWeapon) && 
                (ItemData.itemDict[itemEnum].itemType == ItemData.ItemType.Weapon) || ((ItemData.itemDict[itemEnum].itemType == ItemData.ItemType.Tool)))
                return true;

            return itemsCanDamage.Contains(itemEnum);
        }

        #endregion

        #region AI Data

        [Serializable]
        public class AiData
        {
            public AiType aiType;
            public string DisplayName = "";
            public string BaseName = "";
            public string Description = "";
            public float Health = 10f;
            public float BaseSpeed = 2f;
            public float RunSpeedMultiplier = 3f;
            public float aggressionWeight = 1.0f;
            public float aggressionEnterRange = 20f;
            public float aggressionExitRange = 40f;
            public bool CanTakeDamage = true;
            public bool CanSwim = true;
            public bool CanSprint = true;
            public bool CanJump = true;
            public GameObject ParticleHitName;
            public int[] NumResources;
            public ItemData.ItemEnum[] ResourceTypes;
/*
            public AiData()
            {
                aiType.aiEnum = new AiType.AiEnum();
                aiType.aiEnum = AiType.AiEnum.Boar;
            }*/
        }

        #endregion

        #region Unity Editor

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (StateController == null)
            {
                StateController = GetComponent<StateController>();
            }
        }
#endif
        #endregion
    }
}
