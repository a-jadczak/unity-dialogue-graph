using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{
    private const string STYLE_NAME = "style";
    private DialogueEditorWindow _editorWindow;
    private NodeSearchWindow _searchWindow;

    public DialogueGraphView(DialogueEditorWindow editorWindow)
    {
        _editorWindow = editorWindow;

        StyleSheet styleSheet = Resources.Load<StyleSheet>(STYLE_NAME);
        styleSheets.Add(styleSheet);

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new FreehandSelector());


        GridBackground grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        AddSearchWindow();
    }

    private void AddSearchWindow()
    {
        _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        _searchWindow.Configure(_editorWindow, this);
        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        Port startPortView = startPort;

        ports.ForEach(port =>
        {
            Port portView = port;

            if (startPortView != portView &&
            startPortView.node != portView.node &&
            startPortView.direction != port.direction)
            {
                compatiblePorts.Add(port);
            }
        });

        return compatiblePorts;
    }


    public StartNode CreateStartNode(Vector2 pos) => new StartNode(pos, _editorWindow, this);
    public EndNode CreateEndNode(Vector2 pos) => new EndNode(pos, _editorWindow, this);
    public DialogueNode CreateDialogueNode(Vector2 pos) => new DialogueNode(pos, _editorWindow, this);
    public EventNode CreateEventNode(Vector2 pos) => new EventNode(pos, _editorWindow, this);
    public BranchNode CreateBranchNode(Vector2 pos) => new BranchNode(pos, _editorWindow, this);


}
