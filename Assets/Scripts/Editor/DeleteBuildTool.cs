using UnityEngine;
using UnityEditor;
using System.IO;

public class DeleteBuildTool : EditorWindow
{
    private string buildPath = "";

    private const int WINDOW_WIDTH = 300;
    private const int WINDOW_HEIGHT = 90;

    [MenuItem("Tools/Delete Build")]
    private static void ShowWindow()
    {
        DeleteBuildTool window = EditorWindow.GetWindow<DeleteBuildTool>();
        window.titleContent = new GUIContent("Delete Build");
        window.minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
        window.maxSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Delete Build", EditorStyles.boldLabel);

        GUILayout.Space(5);

        // Build path input field
        buildPath = EditorGUILayout.TextField("Build Path", buildPath);

        GUILayout.Space(5);

        // Delete button
        if (GUILayout.Button("Delete All Files and Folders"))
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
                    EditorUtility.DisplayDialog("Delete Build", "All files and folders in " + buildPath + " have been deleted.", "OK");
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
                if(directory != EditorPrefs.GetString("DeleteBuild_BuildPath")) Directory.Delete(directory);
            }
        }

    }
}
