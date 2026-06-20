using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;

public class PixelArtRenderFeature : ScriptableRendererFeature
{

    
    class PixelArtPass : ScriptableRenderPass
    {
        const string m_PassName = "PixelArtPass";


        // This class stores the data needed by the RenderGraph pass.
        // It is passed as a parameter to the delegate function that executes the RenderGraph pass.
        public class PassData
        {
            public TextureHandle src;
            public TextureHandle dst;
        }

        // This static method is passed as the RenderFunc delegate to the RenderGraph render pass.
        // It is used to execute draw commands.
        static void ExecutePass(PassData data, RasterGraphContext context)
        {
        }

        // RecordRenderGraph is where the RenderGraph handle can be accessed, through which render passes can be added to the graph.
        // FrameData is a context container through which URP resources can be accessed and managed.
        public override void RecordRenderGraph(RenderGraph rg, ContextContainer frameData)
        {
            const string passName = "Render Pixel Art Pass";
            
            var res = frameData.Get<UniversalResourceData>();

            
            
            // This adds a raster render pass to the graph, specifying the name and the data type that will be passed to the ExecutePass function.
            using (var dsBuilder  = rg.AddRasterRenderPass<PassData>(
                       "Pixelate - Colour Downsample (with Transparents)", 
                       out var ds))
            {
                ds.src = res.activeColorTexture;
                ds.dst = lowColorRt;
                
                dsBuilder.UseTexture(ds.src, AccessFlags.Read);
                
                // Use this scope to set the required inputs and outputs of the pass and to
                // setup the passData with the required properties needed at pass execution time.

                // Make use of frameData to access resources and camera data through the dedicated containers.
                // Eg:
                // UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();

                // Setup pass inputs and outputs through the builder interface.
                // Eg:
                // builder.UseTexture(sourceTexture);
                // TextureHandle destination = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraData.cameraTargetDescriptor, "Destination Texture", false);
                
                // This sets the render target of the pass to the active color texture. Change it to your own render target as needed.
                dsBuilder.SetRenderAttachment(ds.dst, 0, AccessFlags.Write);

                // Assigns the ExecutePass function to the render pass delegate. This will be called by the render graph when executing the pass.
                dsBuilder.SetRenderFunc((PassData data, RasterGraphContext ctx) =>
                {
                  BlitTexture(ctx.cmd, data.src, new Vector4(1f, 1f, 0f, 0f),0f, false);  
                });
                dsBuilder.SetGlobalTextureAfterPass(lowColorRT, kColorID);
                _settings.lowColorTexture = lowColorRT;
            }
        }
    }

    PixelArtPass m_ScriptablePass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new PixelArtPass();

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }
}
