using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(GoopBehavior))]
public class GoopPathEditor : Editor
{
    public Color lineColor = Color.yellow; //new Color(1f, 1f, 0f);
    public Color sphereColor = new Color(1f, 1f, 0f, 0.5f);


    private GoopBehavior targetComponent;
    private TreeVisitor<GoopKey> treeDelegate;

    //private static readonly StaticEditorFlags _STATIC_COLLIDER_FLAGS = new StaticEditorFlags();

    // public GoopPathEditor() {
    //     treeDelegate = new TreeVisitor<GoopPoint>(DrawLine);
    // }

    void OnSceneGUI() {
        targetComponent = target as GoopBehavior;
        Handles.Label(targetComponent.transform.position, "center");

        // Draw path
        //targetComponent.path.Traverse(treeDelegate);
        for(int i = 0; i < targetComponent.path.Count - 1; i++) {
            DrawLine(targetComponent.path[i], targetComponent.path[i+1]);
        }

        // Mark scene as dirty
        if(GUI.changed) {
            EditorUtility.SetDirty(targetComponent);
            EditorSceneManager.MarkSceneDirty(targetComponent.gameObject.scene);

            // Set colliders as static
            //GameObjectUtility.SetStaticEditorFlags(object, StaticEditorFlags.)
        }

        
    }

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if(GUILayout.Button("Generate colliders")) {
            // Regenerate colliders
            targetComponent.ClearColliders();
            targetComponent.GenerateColliders();
        }

        if(GUILayout.Button("Clear colliders")) {
            targetComponent.ClearColliders();
        }
    }

    void DrawLine(GoopKey a, GoopKey b) {
        Handles.color = lineColor;
        Handles.DrawDottedLine(targetComponent.transform.position + a.position, targetComponent.transform.position + b.position, 4f);

        //Handles.color = sphereColor;

        // Manage point A
        a.density = Handles.RadiusHandle(Quaternion.identity, targetComponent.transform.position + a.position, a.density);
        a.position = Handles.PositionHandle(targetComponent.transform.position + a.position, Quaternion.identity) - targetComponent.transform.position;

        // Manage point B
        b.density = Handles.RadiusHandle(Quaternion.identity, targetComponent.transform.position + b.position, b.density);
        b.position = Handles.PositionHandle(targetComponent.transform.position + b.position, Quaternion.identity) - targetComponent.transform.position;
    }
}
