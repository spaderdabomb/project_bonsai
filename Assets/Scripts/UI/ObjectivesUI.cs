using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectivesUI : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset objectiveElement;
    [SerializeField] private Texture2D uncheckedBox;
    [SerializeField] private Texture2D checkedBox;

    private VisualElement rootLayout;
    private VisualElement objectiveTitleSymbol;
    private Label objectiveTitleLabel;
    private List<VisualElement> objectiveCheckboxes = new List<VisualElement>();
    private List<Label> objectiveLabels = new List<Label>();

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        rootLayout = root.Q<VisualElement>("objective-in-game");
        objectiveTitleLabel = root.Q<Label>("objective-title");
        objectiveTitleSymbol = root.Q<VisualElement>("objective-symbol");
        objectiveElement = Resources.Load<VisualTreeAsset>("UIuxml/ObjectiveElement");

        string titleText = "Skill progression";
        List<string> newTexts = new List<string>() { "1/2 - Reach lv. 2 woodcutting", "0/50 - Deliver 50 logs to Hogart" };
        InitNewObjectiveUI(titleText, newTexts);
    }

    public void InitNewObjectiveUI(string objectiveTitleText, List<string> objectiveElementsText)
    {
        objectiveTitleLabel.text = objectiveTitleText;
        for (int i = 0; i < objectiveElementsText.Count; i++)
        {
            VisualElement newObjectiveElement = objectiveElement.CloneTree();
            VisualElement newObjectiveCheckbox = newObjectiveElement.Q<Label>("objective-checkbox");
            Label newObjectiveLabel = newObjectiveElement.Q<Label>("objective-label");

            newObjectiveElement.Q<Label>("objective-label").text = objectiveElementsText[i];

            rootLayout.Add(newObjectiveElement);
            objectiveCheckboxes.Add(newObjectiveCheckbox);
            objectiveLabels.Add(newObjectiveLabel);
        }

    }
}
