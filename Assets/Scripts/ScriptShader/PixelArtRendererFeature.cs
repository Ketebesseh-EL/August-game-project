using UnityEngine;
using UnityEngine.Rendering.Universal;
public class PixelArtRendererFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        [Header("Résolution ciblée (basse)")] 
        public int targetWidth = 320;
        public int targetHeight = 180;
        
        [Header("Upscaling Material")]
        public Material pixelArtMaterial;
    }
    
    public Settings settings = new Settings();
    private PixelArtRenderPass m_Pass;

    public override void Create()
    {
        m_Pass = new PixelArtRenderPass(
        settings.targetWidth,
        settings.targetHeight,
        settings.pixelArtMaterial
        );
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.pixelArtMaterial != null)
        {
            renderer.EnqueuePass(m_Pass);
        }
    }

    protected override void Dispose(bool disposing)
    {
        m_Pass?.Dispose();
    }
}


