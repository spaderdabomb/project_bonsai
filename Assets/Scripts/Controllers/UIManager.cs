using ProjectBonsai.Assets.Scripts.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering.PostProcessing;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public UIDocument playerInfoUI;
    public UIDocument objectivesUI;

    public GameObject gameSceneUI;
    public GameObject playerSprintBarUI;

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

    public void SetGameSceneUIState(bool state)
    {
        Core.SetCanvasGroupState(gameSceneUI.GetComponent<CanvasGroup>(), state);
        playerInfoUI.enabled = state;
        objectivesUI.enabled = state;
        Core.SetCanvasGroupState(playerSprintBarUI.GetComponent<CanvasGroup>(), state);
        VisualEffectsManager.Instance.backgroundBlur.GetComponent<PostProcessVolume>().enabled = !state;
    }
}
