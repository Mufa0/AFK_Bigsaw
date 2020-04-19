using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class CreatePlaceholders : EditorWindow
{
    [Tooltip("Root puzzle object")]
    public Object parent;

    public Object script;

    public LayerMask clickableMask;

    [MenuItem("Window/OutlineChildren")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CreatePlaceholders));
    }

    private void OnGUI()
    {
        GUILayout.Label("Create Outline", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        parent = EditorGUILayout.ObjectField("Root puzzle object", parent, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        script = EditorGUILayout.ObjectField("Script to attach", script, typeof(MonoScript), true);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        clickableMask = EditorGUILayout.LayerField("Clickable mask", clickableMask);
        EditorGUILayout.EndHorizontal();


        if (GUILayout.Button("Add Placeholder"))
        {
            if (parent == null)
            {
                ShowNotification(new GUIContent("No object selected for creating placeholders!"));
            }
            else
            {
                CreateChildrenOutlines();
            }
        }
        if (GUILayout.Button("Revert!"))
        {
            RevertChanges();
        }

        if (GUILayout.Button("Attach Script"))
        {
            if (parent == null)
            {
                ShowNotification(new GUIContent("No object selected for attaching script"));
            }
            else
            {
                if (script == null)
                {
                    ShowNotification(new GUIContent("No script selected for attaching!"));
                }
                else
                {
                    AttachScript();
                }
            }
        }
        if (GUILayout.Button("Delete script!"))
        {
            DeleteScript();
        }
    }

    private void AttachScript()
    {
        System.Type scriptType = System.Type.GetType(script.name + ",Assembly-CSharp");
        Transform[] children = ((GameObject)parent).GetComponentsInChildren<Transform>().Where(x => x.gameObject.layer == clickableMask.value).ToArray();
        foreach (Transform child in children)
        {
            child.gameObject.AddComponent(scriptType);
        }
    }

    private void DeleteScript()
    {
        System.Type scriptType = System.Type.GetType(script.name + ",Assembly-CSharp");
        Transform[] children = ((GameObject)parent).GetComponentsInChildren<Transform>().Where(x => x.gameObject.layer == clickableMask.value).ToArray();
        foreach (Transform child in children)
        {
            DestroyImmediate(child.gameObject.GetComponent(scriptType));
        }
    }
    private void CreateChildrenOutlines()
    {
        Transform[] children = ((GameObject)parent).GetComponentsInChildren<Transform>().Where(x => x.gameObject.layer == clickableMask.value).ToArray();
        foreach (Transform child in children)
        {
            CreateChildAndScale(child);
        }
    }

    public void CreateChildAndScale(Transform child)
    {
        GameObject emptyParent = Instantiate(new GameObject("Placeholder-" + child.name));
        
        emptyParent.transform.SetParent(child.parent);
        emptyParent.transform.position = child.transform.position;
        child.SetParent(emptyParent.transform);
        child.transform.localPosition = new Vector3(0f, 0f, 0f);
        

    }

    private void RevertChanges()
    {
        Transform[] children = ((GameObject)parent).GetComponentsInChildren<Transform>().Where(x => x.gameObject.layer == clickableMask.value).ToArray();
        foreach (Transform child in children)
        {
            Transform _tmp = child.transform.parent;
            child.parent = _tmp.parent;
            DestroyImmediate(_tmp.gameObject);
        }
    }
}
