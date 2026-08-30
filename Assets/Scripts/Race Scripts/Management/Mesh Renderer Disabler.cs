using UnityEngine;

public class MeshRendererDisabler : MonoBehaviour
{
    public void OnEnable()
    {
        foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            // Disable the MeshRenderer component
            meshRenderer.enabled = false;
        }
    }
}
