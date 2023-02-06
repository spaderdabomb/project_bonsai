using ProjectBonsai.AI;
using ProjectBonsai.StateMachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static Codice.Client.Common.WebApi.WebApiEndpoints;

[RequireComponent(typeof(NavMeshAgent))]
public class CurvedPath : MonoBehaviour
{
    public int segments = 50;
    public float turnRadius = 5f;
    public float radius = 5f;
    public float lerp = 0.5f;
    public float wanderRadius = 50f;
    public float maxTurnAngle = 180f;
    public float wanderTimer = 20f;
    public float turnSpeed = 5f;
    public NavMeshAgent agent;

    public Transform rockTrans;

    public NavMeshPath currentPath;
    public Vector3[] PathLocations = new Vector3[0];

    private int currentPathIdx = 0;
    private float timer = 0f;
    private bool createNewPath = true;

    private Vector3 InfinityVector = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentPath = new NavMeshPath();

        if (NavMesh.SamplePosition(rockTrans.position, out NavMeshHit hit, agent.radius * 5f, agent.areaMask))
        {
            print(rockTrans.position);
            print(hit.position);
        }
        else
        {
            print("couldn't find point");

        }

    }

    void Update()
    {
        if (timer >= wanderTimer)
        {
            createNewPath = true;
        }

        if (createNewPath)
        {
            Vector3 newPos = GetNewNavPoint();
            agent.CalculatePath(newPos, currentPath);

            if (currentPath.corners.Length > 2)
            {
                PathLocations = CalculateBezierCurve(currentPath.corners[0], currentPath.corners[1]);
                PathLocations = SamplePathPositions(PathLocations);
            }
            else
            {
                PathLocations = currentPath.corners;
            }

            createNewPath = false;
            timer = 0;
            currentPathIdx = 0;

        }

        //PathLocations = CalculateBezierCurve(startPoint_t.position, endPoint_t.position);
        FaceTarget();
        MoveAgent();
        DrawAgentPath();
        timer += Time.deltaTime;
    }
    private void MoveAgent()
    {
        if (currentPathIdx >= PathLocations.Length)
        {
            return;
        }

        if (Vector3.Distance(transform.position, PathLocations[currentPathIdx] + (agent.baseOffset * Vector3.up)) <= agent.radius * 1.1f)
        {
            currentPathIdx++;

            if (currentPathIdx >= PathLocations.Length)
            {
                // Reached destination
                return;
            }
        }

        Vector3 currentPathVector = (PathLocations[currentPathIdx] + (10f * Vector3.up) - transform.position).normalized;
        Vector3 movementVector = Vector3.RotateTowards(transform.forward, currentPathVector.normalized, 3.14f / turnRadius, 1f);
        agent.velocity = (movementVector * agent.speed * Time.deltaTime);
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
/*        Quaternion quat1 = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, 0f));
        Quaternion quat2 = Quaternion.Euler(new Vector3(0f, lookRotation.eulerAngles.y, 0f));
        angleToPathPoint = Quaternion.Angle(quat1, quat2);*/

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed);
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

    private void MakeRadialBezierPath(Vector3[] points)
    {
        Vector3[] finalPath = new Vector3[0];
        for (int i = 0; i < points.Length - 1; i++)
        {
            CalculateBezierCurve(points[i], points[i+1]);
        }
    }

    private Vector3 CalculateBezierControlPoint(Vector3 startPoint, Vector3 endPoint)
    {
        Vector3 directionStartEnd = (endPoint - startPoint).normalized;
        Vector3 directionTransform = transform.forward.normalized;
        Vector3 directionPerp = Vector3.Cross(transform.up, directionStartEnd).normalized;
        float sign = Mathf.Sign(Mathf.Cos(Mathf.Deg2Rad * Vector3.Angle(transform.forward, directionPerp)));
        Vector3 directionFinal = (directionTransform + sign * directionPerp).normalized;
        Vector3 result = startPoint + directionFinal * radius;

        return result;
    }

    private Vector3[] CalculateBezierCurve(Vector3 startPoint, Vector3 endPoint)
    {
        Vector3[] points = new Vector3[segments];
        Vector3 controlPoint = CalculateBezierControlPoint(startPoint, endPoint);
        for (int i = 1; i <= segments; i++)
        {
            float t = i / (float)segments;
            points[i - 1] = CalculateBezierPoint(t, startPoint, controlPoint, endPoint);
        }

        return points;
    }
    

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }

    private Vector3[] SamplePathPositions(Vector3[] Path)
    {
        for (int i = 0; i < Path.Length; i++)
        { 
            if (NavMesh.SamplePosition(Path[i], out NavMeshHit hit, agent.radius * 5f, agent.areaMask))
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

    private void DrawAgentPath()
    {
        for (int i = 0; i < PathLocations.Length - 1; i++)
        {
            Debug.DrawLine(PathLocations[i], PathLocations[i + 1], Color.blue);
        }
        //Debug.DrawLine(transform.position, PathLocations[points], Color.yellow);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < PathLocations.Length - 1; i++)
        {
            Gizmos.DrawWireSphere(PathLocations[i], 0.3f);
        }

        //Gizmos.DrawWireSphere(controlPoint, 0.3f);
    }
}