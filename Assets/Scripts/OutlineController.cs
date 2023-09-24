using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineControl : MonoBehaviour
{
    [SerializeField] private float outlineScale;
    [SerializeField] private Renderer rend; // Reference to the Renderer component

    private Material outlineMaterial;
    private static readonly int ScalePropertyID = Shader.PropertyToID("_Scale");
    private const string OutlineShaderName = "Shader Graphs/Outline"; // Replace with your shader's name

    private void Awake()
    {
        if (rend == null)
        {
            Debug.LogError("Renderer reference not set on OutlineControl script!");
            return;
        }

        foreach (var mat in rend.materials)
        {
            if (mat.shader.name == OutlineShaderName)
            {
                outlineMaterial = mat;
                break;
            }
        }

        if (outlineMaterial == null)
        {
            Debug.LogError("No material with the specified shader name found on the renderer!");
        }
    }

    private void OnMouseExit()
    {
        SetOutlineScale(1);
    }

    private void OnMouseOver()
    {
        SetOutlineScale(outlineScale);
    }

    public void SetOutlineScale(float scaleValue)
    {
        if (outlineMaterial != null)
        {
            outlineMaterial.SetFloat(ScalePropertyID, scaleValue);
        }
    }
}
