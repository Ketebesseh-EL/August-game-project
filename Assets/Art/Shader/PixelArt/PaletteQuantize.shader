Shader "PixelArt/PaletteQuantize"
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
            #pragma vertex   Vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            int _PaletteSize;
            float4 _Palette[32];

            float LabF(float t)
            {
                float cubic  = pow(max(t, 1e-10), 1.0 / 3.0);
                float linApprox = (903.3 * t + 16.0) / 116.0;
                return lerp(linApprox, cubic, step(0.008856, t));
            }


            float3 RGBToLab(float3 rgb)
            {
                float3 lin = pow(max(rgb,0.0001),2.2);

                float X = dot(lin, float3(0.4124564, 0.3575761, 0.1804375));
                float Y = dot(lin, float3(0.2126729, 0.7151522, 0.0721750));
                float Z = dot(lin, float3(0.0193339, 0.1191920, 0.9503041));

                X /= 0.95047;
                Y /= 1.00000;
                Z /= 1.08883;

                float fx = LabF(X);
                float fy = LabF(Y);
                float fz = LabF(Z);

                return float3(
                    116.0 * (fy - 16.0),
                    500.0 *(fx - fy),
                    200.0 * (fy - fz)
                    );
            }

            float3 NearestPaletteColor (float3 inputRGB)
            {
                float3 inputLab = RGBToLab(inputRGB);
                float bestDist = 1e9;
                float3 bestColor = _Palette[0].rgb;

                [loop]
                for (int i = 0; i<_PaletteSize; i++)
                {
                    float3 palLab = RGBToLab(_Palette[i].rgb);
                    float3 diff = inputLab - palLab;
                    float3 dist = dot(diff, diff);

                    if (dist < bestDist)
                    {
                        bestDist  = dist;
                        bestColor = _Palette[i].rgb;
                    }
                }
                return bestColor;
        }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 color = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_PointClamp, IN.texcoord).rgb;
                float3 quantized = NearestPaletteColor(color);
                return half4(quantized, 1.0);
                }
            ENDHLSL
        }
    }
}