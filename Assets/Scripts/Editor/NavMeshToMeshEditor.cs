using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class NavMeshToMeshEditor : EditorWindow
{
    private string meshAssetName = "NavMesh";

    [MenuItem("Tools/Export NavMesh to Mesh")]
    public static void ShowWindow()
    {
        GetWindow<NavMeshToMeshEditor>("Export NavMesh to Mesh");
    }

    private void OnGUI()
    {
        GUILayout.Label("Export NavMesh to Mesh", EditorStyles.boldLabel);

        meshAssetName = EditorGUILayout.TextField("Mesh Asset Name", meshAssetName);

        if (GUILayout.Button("Export"))
        {
            ExportNavMeshToMesh();
        }
    }

    private void ExportNavMeshToMesh()
    {
        NavMeshTriangulation triangulatedNavMesh = NavMesh.CalculateTriangulation();

        Mesh navMeshMesh = new Mesh
        {
            name = meshAssetName,
            vertices = triangulatedNavMesh.vertices,
            triangles = triangulatedNavMesh.indices
        };

        string path = "Assets/Resources/" + meshAssetName + ".asset";

        // Check if an asset with the same name exists, and delete it if it does
        Mesh existingMesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        if (existingMesh != null)
        {
            AssetDatabase.DeleteAsset(path);
        }

        AssetDatabase.CreateAsset(navMeshMesh, path);
        AssetDatabase.SaveAssets();
    }
}
