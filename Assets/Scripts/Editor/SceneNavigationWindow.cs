using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor.SceneManagement;

public class SceneNavigationWindow : EditorWindow
{
    private List<string> scenePaths = new List<string>();
    private Vector2 scrollPosition;
    private string specifiedPath;
    private bool showAllScenes = true;
    private string searchFilter = "";

    [MenuItem("Window/Scene Navigation")]
    public static void ShowWindow()
    {
        GetWindow<SceneNavigationWindow>("Scene Navigation");
    }

    private void OnEnable()
    {
        specifiedPath = EditorPrefs.GetString("SceneNavigationWindow_SpecifiedPath", "Assets/");
        RefreshSceneList();
    }

    private void RefreshSceneList()
    {
        scenePaths.Clear();
        string[] allAssets = AssetDatabase.GetAllAssetPaths();

        foreach (string assetPath in allAssets)
        {
            if (Path.GetExtension(assetPath) == ".unity")
            {
                if (showAllScenes || (!showAllScenes && assetPath.StartsWith(specifiedPath)))
                {
                    scenePaths.Add(assetPath);
                }
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Scene Navigation", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("All Scenes"))
        {
            showAllScenes = true;
            RefreshSceneList();
        }

        if (GUILayout.Button("Specified Path"))
        {
            showAllScenes = false;
            RefreshSceneList();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Path:", GUILayout.Width(40));
        string newPath = EditorGUILayout.TextField(specifiedPath);
        if (newPath != specifiedPath)
        {
            specifiedPath = newPath;
            EditorPrefs.SetString("SceneNavigationWindow_SpecifiedPath", specifiedPath);
            if (!showAllScenes)
            {
                RefreshSceneList();
            }
        }
        GUILayout.EndHorizontal();

        if (showAllScenes)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Search:", GUILayout.Width(50));
            searchFilter = GUILayout.TextField(searchFilter);
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginVertical("box");
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        foreach (string scenePath in scenePaths)
        {
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);

            if (showAllScenes && !string.IsNullOrEmpty(searchFilter) && !sceneName.ToLower().Contains(searchFilter.ToLower()))
            {
                continue;
            }

            if (GUILayout.Button(sceneName))
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(scenePath);
                }
            }
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();

        GUILayout.Space(10);
        if (GUILayout.Button("Refresh"))
        {
            RefreshSceneList();
        }
    }
}
