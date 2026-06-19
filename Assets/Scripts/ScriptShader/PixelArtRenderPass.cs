using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelArtRenderPass : ScriptableRenderPass, IDisposable
{
    private readonly int m_TargetWidth;
    private readonly int m_TargetHeight;
    private readonly Material m_Material;

    private RTHandle m_LowResHandle;
    public PixelArtRenderPass(int w, int h, Material mat)
    {
        m_TargetWidth = w;
        m_TargetHeight = h;
        m_Material = mat;

        renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        var desc = renderingData.cameraData.cameraTargetDescriptor;
        desc.width = m_TargetWidth;
        desc.height = m_TargetHeight;
        desc.depthBufferBits = 0;

        RenderingUtils.ReAllocateHandleIfNeeded(ref m_LowResHandle, desc, FilterMode.Point, TextureWrapMode.Clamp,
            name: "_PixelArtLowRes");
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (m_Material == null) return;

        var cmd = CommandBufferPool.Get("PixelArtPass");

        var cameraTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;
        
        Blitter.BlitCameraTexture(cmd, cameraTarget,m_LowResHandle);
        Blitter.BlitCameraTexture(cmd, m_LowResHandle, cameraTarget, m_Material, pass: 0);
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public void Dispose()
    {
        m_LowResHandle?.Release();
        m_LowResHandle = null;
    }

}
