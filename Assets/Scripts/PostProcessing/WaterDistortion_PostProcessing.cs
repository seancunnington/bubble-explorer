using UnityEngine;

[ExecuteInEditMode]
public class WaterDistortion_PostProcessing : MonoBehaviour
{
    public Material waterDistortion;

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (waterDistortion != null)
            Graphics.Blit(src, dst, waterDistortion);
    }
}