using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static ObjectiveTask;

public class ObjectivesUI : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset objectiveElement;
    [SerializeField] private VisualTreeAsset objectiveHeader;
    [SerializeField] private Texture2D uncheckedBox;
    [SerializeField] private Texture2D checkedBox;

    private VisualElement root;
    private VisualElement rootLayout;
    private List<VisualElement> objectiveCheckboxes = new List<VisualElement>();
    private List<Label> objectiveLabels = new List<Label>();

    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        rootLayout = root.Q<VisualElement>("objective-in-game");
    }

    public void InitNewObjectiveUI(string objectiveTitleText, List<ObjectiveSubTask> objectiveElementsText)
    {
        // Set header layout
        VisualElement newObjectiveHeader = objectiveHeader.CloneTree();
        VisualElement objectiveTitleSymbol = root.Q<VisualElement>("header-symbol");
        Label objectiveTitleLabel = newObjectiveHeader.Q<Label>("header-label");
        rootLayout.Add(newObjectiveHeader);

        // Set element layout
        for (int i = 0; i < objectiveElementsText.Count; i++)
        {
            VisualElement newObjectiveElement = objectiveElement.CloneTree();
            VisualElement newObjectiveCheckbox = newObjectiveElement.Q<Label>("objective-checkbox");
            Label newObjectiveLabel = newObjectiveElement.Q<Label>("objective-label");

            newObjectiveElement.Q<Label>("objective-label").text = objectiveElementsText[i].ObjectiveDescription;

            rootLayout.Add(newObjectiveElement);
            objectiveCheckboxes.Add(newObjectiveCheckbox);
            objectiveLabels.Add(newObjectiveLabel);
        }

    }

    public void ClearUI()
    {
        while (rootLayout.childCount > 0)
        {
            VisualElement child = rootLayout.ElementAt(0);
            child.RemoveFromHierarchy();
        }

        objectiveCheckboxes.Clear();
        objectiveLabels.Clear();
    }
}
