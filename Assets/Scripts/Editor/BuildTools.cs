using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;

public class BuildTools : EditorWindow
{
    private string buildPath = "";

    private const int WINDOW_WIDTH = 400;
    private const int WINDOW_HEIGHT = 135;

    

    [MenuItem("Tools/Build Tools")]
    private static void ShowWindow()
    {
        BuildTools window = EditorWindow.GetWindow<BuildTools>();
        window.titleContent = new GUIContent("Build Tools");
        window.minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
        window.maxSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
        window.Show();
    }

    private void OnGUI()
    {
      
        GUILayout.Label("Build Tools", EditorStyles.boldLabel);

        GUILayout.Space(5);

        // Build path input field
        buildPath = EditorGUILayout.TextField("Build Path", buildPath);

        GUILayout.Space(5);

        if (GUILayout.Button("Everything",GUILayout.Width(391), GUILayout.Height(25)))
        {
            DeleteOldBuild();
            Build();
            Run();
        }
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();

        Vector2 size = new Vector2(125, 50);
        // Build button
        BuildButton("Build",size);

        GUILayout.Space(5);

        // Run button
        RunButton("Run", size);

        GUILayout.Space(5);

        // Delete button
        DeleteButton("Delete Old Build", size);
        GUILayout.EndHorizontal();
    }

    private void DeleteButton(string buttonName, Vector2 size)
    {
        if (GUILayout.Button(buttonName, GUILayout.Width(size.x), GUILayout.Height(size.y)))
        {
            DeleteOldBuild();
        }
    }

    private void DeleteOldBuild()
    {
        if (!string.IsNullOrEmpty(buildPath))
        {
            if (Directory.Exists(buildPath))
            {
                // Delete all files and folders in the directory
                DeleteAllFilesAndFolders(buildPath);

                // Refresh the asset database to reflect the changes
                AssetDatabase.Refresh();

                // Display a success message
                UnityEngine.Debug.Log("All files and folders in " + buildPath + " have been deleted.");
            }
            else
            {
                // Display an error message if the directory doesn't exist
                EditorUtility.DisplayDialog("Delete Build", "The specified directory does not exist.", "OK");
            }
        }
        else
        {
            // Display an error message if the build path is empty
            EditorUtility.DisplayDialog("Delete Build", "Please enter a build path.", "OK");
        }
    }

    private void RunButton(string buttonName, Vector2 size)
    {
        if (GUILayout.Button(buttonName, GUILayout.Width(size.x), GUILayout.Height(size.y)))
        {
            Run();
        }
    }

    private void Run()
    {
        if (!string.IsNullOrEmpty(buildPath))
        {
            if (Directory.Exists(buildPath))
            {
                string exePath = Path.Combine(buildPath, PlayerSettings.productName + ".exe");

                if (File.Exists(exePath))
                {
                    // Run the built executable
                    Process.Start(exePath);

                    // Display a success message
                    UnityEngine.Debug.Log("Executable started.");
                }
                else
                {
                    // Display an error message if the executable doesn't exist
                    EditorUtility.DisplayDialog("Build Tools", "Executable not found at " + exePath, "OK");
                }
            }
            else
            {
                // Display an error message if the directory doesn't exist
                EditorUtility.DisplayDialog("Build Tools", "The specified directory does not exist.", "OK");
            }
        }
        else
        {
            // Display an error message if the build path is empty
            EditorUtility.DisplayDialog("Build Tools", "Please enter a build path.", "OK");
        }
    }

    private void BuildButton(string buttonName, Vector2 size)
    {
        if (GUILayout.Button(buttonName, GUILayout.Width(size.x), GUILayout.Height(size.y)))
        {
            Build();
        }
    }

    private void Build()
    {
        if (!string.IsNullOrEmpty(buildPath))
        {
            if (Directory.Exists(buildPath))
            {
                // Build the project
                BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, buildPath + "/" + PlayerSettings.productName + ".exe", BuildTarget.StandaloneWindows, BuildOptions.None);

                // Display a success message
                UnityEngine.Debug.Log("Build completed.");
            }
            else
            {
                // Display an error message if the directory doesn't exist
                EditorUtility.DisplayDialog("Build Tools", "The specified directory does not exist.", "OK");
            }
        }
        else
        {
            // Display an error message if the build path is empty
            EditorUtility.DisplayDialog("Build Tools", "Please enter a build path.", "OK");
        }
    }

    private void OnEnable()
    {
        // Load the last used build path from editor preferences
        buildPath = EditorPrefs.GetString("DeleteBuild_BuildPath");
    }

    private void OnDisable()
    {
        // Save the current build path to editor preferences
        EditorPrefs.SetString("DeleteBuild_BuildPath", buildPath);
    }

    private void DeleteAllFilesAndFolders(string path)
    {
        if (Directory.Exists(path))
        {
            // Delete all files in the directory
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                File.Delete(file);
            }

            // Recursively delete all subdirectories and their files
            string[] directories = Directory.GetDirectories(path);
            foreach (string directory in directories)
            {
                DeleteAllFilesAndFolders(directory);
                if (directory != EditorPrefs.GetString("DeleteBuild_BuildPath")) Directory.Delete(directory);
            }
        }

    }
}