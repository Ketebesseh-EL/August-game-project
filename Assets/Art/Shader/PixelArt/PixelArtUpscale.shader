Shader "PixelArt/Upscale"
{
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }
        ZWrite Off
        ZTest Always
        Cull Off
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
        half4 frag(Varyings IN) : SV_Target
            {
                return half4(SAMPLE_TEXTURE2D_X (_BlitTexture, sampler_PointClamp, IN.texcoord).rgb,1.0);
            }
            ENDHLSL
        }
    }
}
