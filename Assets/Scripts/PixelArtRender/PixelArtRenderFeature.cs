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

        static readonly int s_PaletteTexId = Shader.PropertyToID("_PaletteTex");
        static readonly int s_PaletteSizeId = Shader.PropertyToID("_PaletteSize");

        int pixelWidth;
        int pixelHeight;
        Material m_PaletteColorMaterial;
        Texture2D m_PaletteTexture;

        public void Setup(int m_PixelWidth, int m_PixelHeight, Material paletteMaterial, Texture2D paletteTexture)
        {
            pixelWidth = m_PixelWidth;
            pixelHeight = m_PixelHeight;
            m_PaletteColorMaterial = paletteMaterial;
            m_PaletteTexture = paletteTexture;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            if (m_PaletteColorMaterial == null || m_PaletteTexture == null)
            {
                Debug.LogWarning("Besoin d'une palette et d'un material pour le render pixel");
                return;
            }

            var resourceData = frameData.Get<UniversalResourceData>();
            if (resourceData.isActiveTargetBackBuffer)
            {
                Debug.LogError("Skipping");
                return;
            }

            TextureHandle source = resourceData.activeColorTexture;

            TextureDesc lowResDesc = renderGraph.GetTextureDesc(source);
            lowResDesc.name = $"{m_PassName}-LowRes";
            lowResDesc.width = pixelWidth;
            lowResDesc.height = pixelHeight;
            lowResDesc.filterMode = FilterMode.Point;
            lowResDesc.clearBuffer = false;
            TextureHandle lowResTarget = renderGraph.CreateTexture(lowResDesc);

            renderGraph.AddBlitPass(
                source, lowResTarget, Vector2.one, Vector2.zero,
                filterMode: RenderGraphUtils.BlitFilterMode.ClampNearest,
                passName: $"{m_PassName}_Downsample");

            TextureDesc paletteDesc = renderGraph.GetTextureDesc(lowResTarget);
            paletteDesc.name = $"{m_PassName}-Palette";
            TextureHandle paletteTarget = renderGraph.CreateTexture(paletteDesc);

            m_PaletteColorMaterial.SetTexture(s_PaletteTexId, m_PaletteTexture);
            m_PaletteColorMaterial.SetFloat(s_PaletteSizeId, m_PaletteTexture.width);

            RenderGraphUtils.BlitMaterialParameters paletteBlitParams =
                new(lowResTarget, paletteTarget, m_PaletteColorMaterial, 0);
            renderGraph.AddBlitPass(paletteBlitParams, passName: $"{m_PassName}_PaletteMatch");

            TextureDesc destDesc = renderGraph.GetTextureDesc(source);
            destDesc.name = $"{m_PassName}-Output";
            destDesc.clearBuffer = true;
            TextureHandle destination = renderGraph.CreateTexture(destDesc);

            renderGraph.AddBlitPass(
                paletteTarget, destination, Vector2.one, Vector2.zero,
                filterMode: RenderGraphUtils.BlitFilterMode.ClampNearest,
                passName: $"{m_PassName}_Upsample");

            resourceData.cameraColor = destination;
        }
    }

    [Header("Pixel Art Resolution")]
    [SerializeField, Min(1)] private int pixelWidth = 400;
    [SerializeField, Min(1)] private int pixelHeight = 225;

    [Header("Palette Settings")]
    [SerializeField] private Texture2D m_paletteTexture;
    [SerializeField] private Material m_PaletteColorMaterial;

    PixelArtRenderPass PixelArtPass;
    public RenderPassEvent injectionPoint = RenderPassEvent.AfterRenderingOpaques;

    public override void Create()
    {
        PixelArtPass = new PixelArtRenderPass
        {
            renderPassEvent = injectionPoint,
            requiresIntermediateTexture = true
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (m_PaletteColorMaterial == null || m_paletteTexture == null)
            return;

        PixelArtPass.Setup(pixelWidth, pixelHeight, m_PaletteColorMaterial, m_paletteTexture);
        renderer.EnqueuePass(PixelArtPass);
    }
}
