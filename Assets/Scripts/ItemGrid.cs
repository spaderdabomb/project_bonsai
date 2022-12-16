using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBonsai
{
    public class ItemGrid : MonoBehaviour
    {
        [SerializeField] int _rows, _columns;
        [SerializeField] bool alreadyInstantiated;

        public GameObject[] toggleGOs;

        public void InitItemGrid(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
        }
        void Start()
        {
            if (alreadyInstantiated)
            {

            }
            else
            {
                alreadyInstantiated = true;
            }

            toggleGOs = new GameObject[_rows*_columns];
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    GameObject prefab = (GameObject)Resources.Load("Prefabs/UI/GridSpaceUI");
                    GameObject toggle = Instantiate(prefab, this.transform);
                    toggle.GetComponent<Toggle>().group = this.gameObject.GetComponent<ToggleGroup>();
                    toggleGOs[j + i * _columns] = toggle;
                }
            }

            toggleGOs[0].GetComponent<Toggle>().isOn = true;
        }

        void Update()
        {
        
        }

        public void InstantiateItem(string itemName, int slotIndex)
        {
            GameObject item = (GameObject)Resources.Load("Prefabs/UI/" + itemName);
            GameObject instantiatedItem = Instantiate(item, toggleGOs[slotIndex].transform);
        }

        public int GetFirstFreeSlotIndex()
        {
            int index = 0;
            foreach (GameObject gameObject in toggleGOs)
            {
                if (gameObject.transform.childCount == 1)
                {
                    return index;
                }
                index++;
            }

            return index;
        }
    }
}
