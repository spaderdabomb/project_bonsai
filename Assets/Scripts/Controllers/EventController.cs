using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectBonsai.Assets.Scripts.Controllers
{
    public class EventController : MonoBehaviour
    {
        public static EventController Instance;

        int UILayer;
        public delegate void ClickAction();
        public static event ClickAction OnClicked;

        public delegate void RAction();
        public static event RAction OnR;

        public delegate void KeyAction(); 
        public static event KeyAction OnKeyPress;

        int currentSpawnIndex = 0;

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
            UILayer = LayerMask.NameToLayer("UI");
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                foreach (RaycastResult result in GetEventSystemRaycastResults())
                {
                    // print(result);
                }

                if (OnClicked != null)
                {
                    OnClicked();
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (OnR != null) { OnR(); }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {

            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameSceneController.Instance.EscapePressed();
            }

            if (Input.anyKeyDown)
            {
                if (OnKeyPress != null) { OnKeyPress(); }
            }
        }

        public bool IsPointerOverUIElement()
        {
            return IsPointerOverUIElement(GetEventSystemRaycastResults());
        }


        //Returns 'true' if we touched or hovering on Unity UI element.
        private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
        {
            for (int index = 0; index < eventSystemRaysastResults.Count; index++)
            {
                RaycastResult curRaysastResult = eventSystemRaysastResults[index];
                if (curRaysastResult.gameObject.layer == UILayer)
                {
                    print("he");
                    return true;
                }
            }
            return false;
        }


        //Gets all event system raycast results of current mouse or touch position.
        static List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            return raysastResults;
        }
    }
}