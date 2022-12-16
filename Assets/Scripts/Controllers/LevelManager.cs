using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBonsai.Assets.Scripts.Controllers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        public int levelNum;

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

        }

        public void InitLevel(int _levelNum)
        {
            levelNum = _levelNum;
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}