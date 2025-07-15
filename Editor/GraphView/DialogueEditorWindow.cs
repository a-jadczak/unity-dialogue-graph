using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueEditorWindow : EditorWindow
{
    private DialogueContainerSO currentDialogueContainer;
    private DialogueGraphView graphview;
    private DialogueSaveAndLoad saveAndLoad;

    private Label nameOfDialogueContainer;

    private const string STYLE_NAME = "style";

    [OnOpenAsset(1)]
    public static bool ShowWindow(int instanceId, int line)
    {
        UnityEngine.Object item = EditorUtility.InstanceIDToObject(instanceId);

        if (item is DialogueContainerSO)
        {
            DialogueEditorWindow window = GetWindow<DialogueEditorWindow>();
            window.titleContent = new GUIContent("Dialogue Editor");
            window.currentDialogueContainer = item as DialogueContainerSO;
            window.minSize = new Vector2(500, 250);
            window.Load();
        }

        return false;
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
        Load();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graphview);
    }

    private void ConstructGraphView()
    {
        graphview = new DialogueGraphView(this);
        graphview.StretchToParentSize();
        rootVisualElement.Add(graphview);

        saveAndLoad = new DialogueSaveAndLoad(graphview);
    }

    private void GenerateToolbar()
    {
        //Adding Styles
        StyleSheet styleSheet = Resources.Load<StyleSheet>(STYLE_NAME);
        rootVisualElement.styleSheets.Add(styleSheet);

        Toolbar toolbar = new Toolbar();
        #region Toolbar Buttons
        Button saveBtn = new Button()
        {
           text = "Save",
        };

        saveBtn.clicked += ()=> 
        {
            Save();
        };

        Button loadBtn = new Button()
        { 
            text = "Load", 
        };

        loadBtn.clicked += () =>
        {
            Load();
        };

        #endregion


        toolbar.Add(saveBtn);
        toolbar.Add(loadBtn);

        //Name of current opened container
        nameOfDialogueContainer = new Label("");
        toolbar.Add(nameOfDialogueContainer);
        //Eksport to style.uss
        nameOfDialogueContainer.AddToClassList("nameOfDialogueContainer");

        rootVisualElement.Add(toolbar);
    }

    private void Load()
    {
        Debug.Log("Load");
        if (currentDialogueContainer != null)
        {
            nameOfDialogueContainer.text = "Name: " + currentDialogueContainer.name;
            saveAndLoad.Load(currentDialogueContainer);
        }
    }

    private void Save()
    {
        if (currentDialogueContainer != null)
        {
            saveAndLoad.Save(currentDialogueContainer);
        }
    }



}
