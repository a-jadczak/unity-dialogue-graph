using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
{
    private DialogueEditorWindow editorWindow;
    private DialogueGraphView graphView;

    public void Configure(DialogueEditorWindow editorWindow, DialogueGraphView graphView)
    {
        this.editorWindow = editorWindow;
        this.graphView = graphView;
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        List<SearchTreeEntry> tree = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 0),
            new SearchTreeGroupEntry(new GUIContent("Dialogue"), 1),

            AddNodeSearch("Start Node", new StartNode()),
            AddNodeSearch("Dialogue Node", new DialogueNode()),
            AddNodeSearch("Event Node", new EventNode()),
            AddNodeSearch("End Node", new EndNode()),
            AddNodeSearch("Branch Node", new BranchNode()),
        };


        return tree;
    }

    private SearchTreeEntry AddNodeSearch(string name, BaseNode baseNode)
    {
        SearchTreeEntry temp = new SearchTreeEntry(new GUIContent(name))
        {
            level = 2,
            userData = baseNode
        };

        return temp;
    }


    public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
    {
        Vector2 mousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo
        (
            editorWindow.rootVisualElement.parent, context.screenMousePosition - editorWindow.position.position
        );
        
        Vector2 graphMousePosition = graphView.contentViewContainer.WorldToLocal(mousePosition);

        return CheckForNodeType(searchTreeEntry, graphMousePosition);
    }

    private bool CheckForNodeType(SearchTreeEntry searchTreeEntry, Vector2 pos)
    {
        switch (searchTreeEntry.userData)
        {
            case StartNode node:
                graphView.AddElement(graphView.CreateStartNode(pos));
                return true;
            case DialogueNode node:
                graphView.AddElement(graphView.CreateDialogueNode(pos));
                return true;
            case EventNode node:
                graphView.AddElement(graphView.CreateEventNode(pos));
                return true;
            case EndNode node:
                graphView.AddElement(graphView.CreateEndNode(pos));
                return true;
            case BranchNode node:
                graphView.AddElement(graphView.CreateBranchNode(pos));
                return true;

            default:
                break;
        }
        return false;
    }

}
