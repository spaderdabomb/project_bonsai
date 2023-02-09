using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectsManager : MonoBehaviour
{
    public static VisualEffectsManager Instance;

    public GameObject backgroundBlur;

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


    // Update is called once per frame
    void Update()
    {
        
    }
}
