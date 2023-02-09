using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBonsai.Assets.Scripts.Controllers
{
    public class TerrainObjectController : MonoBehaviour
    {
        public static TerrainObjectController Instance;

        [HideInInspector] public GameObject treeTerrainObjectHolder;
        [HideInInspector] public List<GameObject> treeTerrainObjects;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        void Start()
        {
            treeTerrainObjectHolder = Core.FindGameObjectByNameAndTag("Trees", "TerrainObjectHolder");
            treeTerrainObjects = new List<GameObject>();

            for (int i = 0; i < treeTerrainObjectHolder.transform.childCount; i++)
            {
                treeTerrainObjects.Add(treeTerrainObjectHolder.transform.GetChild(i).gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {
/*            foreach (GameObject tree in treeTerrainObjects)
            {

            }*/
        }
    }
}
