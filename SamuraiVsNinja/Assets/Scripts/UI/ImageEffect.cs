using UnityEngine;

[ExecuteInEditMode]
public class ImageEffect : MonoBehaviour
{
    [SerializeField]
    private Material effectMaterial;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, effectMaterial);
    }
}
