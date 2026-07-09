using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;

public class PixelArtRenderFeature : ScriptableRendererFeature
{
    class PixelArtRenderPass : ScriptableRenderPass
    {
        const string m_PassName = "PixelArtRenderPass";
        int pixelWidth;
        int pixelHeight;

        public void Setup(int m_PixelWidth, int m_PixelHeight)
        {
            pixelWidth = m_PixelWidth;
            pixelHeight = m_PixelHeight;
            requiresIntermediateTexture = true;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var ressourceData = frameData.Get<UniversalResourceData>();
            if (ressourceData.isActiveTargetBackBuffer)
            {
                Debug.LogError($"Skipping render pass. PixelArtRendererFeature requires an intermediate ColorTexture, we can't use the BackBuffer as a texture input");
                return;
            }

            var source = ressourceData.activeColorTexture;
            
            var lowResDesc = renderGraph.GetTextureDesc(source);
            lowResDesc.name = $"{m_PassName}-LowRes";
            lowResDesc.width = pixelWidth;
            lowResDesc.height = pixelHeight;
            lowResDesc.filterMode = FilterMode.Point;
            TextureHandle lowResTarget = renderGraph.CreateTexture(lowResDesc);

            renderGraph.AddBlitPass(source, lowResTarget, Vector2.one, Vector2.zero,
                filterMode: RenderGraphUtils.BlitFilterMode.ClampNearest, passName: $"{m_PassName}_DownSample");
            
            var destDesc = renderGraph.GetTextureDesc(source);
            destDesc.name = $"{m_PassName}-Output";
            destDesc.clearBuffer = false;
            TextureHandle destination = renderGraph.CreateTexture(destDesc);
            
            renderGraph.AddBlitPass(lowResTarget, destination, Vector2.one, Vector2.zero,filterMode: RenderGraphUtils.BlitFilterMode.ClampNearest, passName: $"{m_PassName}_UpSample");

            ressourceData.cameraColor = destination;
        }
    }

    public RenderPassEvent injectionPoint = RenderPassEvent.AfterRenderingOpaques;

    PixelArtRenderPass PixelArtPass;

    [Header("Pixel Art Resolution")] 
    [SerializeField] private int pixelWidth = 400;
    [SerializeField] private int pixelHeight = 225;
    
    public override void Create()
    {
        PixelArtPass = new PixelArtRenderPass();
        PixelArtPass.renderPassEvent = injectionPoint;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {

        PixelArtPass.Setup(pixelWidth,pixelHeight);
        renderer.EnqueuePass(PixelArtPass);
    }
}
