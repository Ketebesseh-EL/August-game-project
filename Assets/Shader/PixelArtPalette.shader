Shader "Custom/PixelArtPalette"
{
    Properties
    {
        _PaletteTex ("Palette Texture", 2D) = "white" {}
        _PaletteSize ("Palette Size", Float) = 16
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100
        ZWrite Off
        Cull Off

        Pass
        {
            Name "PixelArtPalettePass"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            // Blit.hlsl supplies the Vert function, Attributes, and Varyings
            // structs expected by AddBlitPass / Blitter. Do not declare your own.
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            TEXTURE2D(_PaletteTex);
            SAMPLER(sampler_PaletteTex);
            float _PaletteSize;

            // OKLab conversion (from https://bottosson.github.io/posts/oklab/)

            // Accurate sRGB EOTF (not a gamma-2.2 approximation).
            // NOTE: If _MainTex / _PaletteTex are imported with "sRGB (Color Texture)"
            // checked, Unity already linearizes on sample and calling this again will
            // double-convert. Only use this if your textures are imported as Linear/raw,
            // or disable sRGB read on import and keep this function.
            float3 srgb_to_linear(float3 srgb)
            {
                float3 lower = srgb / 12.92;
                float3 higher = pow((srgb + 0.055) / 1.055, 2.4);
                float3 cutoff = step(srgb, 0.04045);
                return lerp(higher, lower, cutoff);
            }

            float3 linear_to_oklab(float3 lin)
            {
                // Linear RGB to LMS
                float3 lms = float3(
                    0.4122214708 * lin.r + 0.5363325363 * lin.g + 0.0514459929 * lin.b,
                    0.2119034982 * lin.r + 0.6806995451 * lin.g + 0.1073969566 * lin.b,
                    0.0883024619 * lin.r + 0.2817188376 * lin.g + 0.6299787005 * lin.b
                );
                // Non-linearity (cube root)
                float3 lms_nonlinear = pow(abs(lms), 1.0/3.0) * sign(lms);
                // LMS to OKLab
                return float3(
                    0.2104542553 * lms_nonlinear.r + 0.7936177850 * lms_nonlinear.g - 0.0040720468 * lms_nonlinear.b,
                    1.9779984951 * lms_nonlinear.r - 2.4285922050 * lms_nonlinear.g + 0.4505937099 * lms_nonlinear.b,
                    0.0259040371 * lms_nonlinear.r + 0.7827717662 * lms_nonlinear.g - 0.8086757660 * lms_nonlinear.b
                );
            }

            float3 find_closest_palette_color(float3 target_oklab)
            {
                float3 closest_color = float3(0, 0, 0);
                float min_distance = 1e10;
                int paletteCount = (int)_PaletteSize;

                for (int i = 0; i < paletteCount; i++)
                {
                    float2 palette_uv = float2((i + 0.5) / _PaletteSize, 0.5);
                    float3 palette_color = SAMPLE_TEXTURE2D_LOD(_PaletteTex, sampler_PaletteTex, palette_uv, 0).rgb;
                    float3 palette_linear = srgb_to_linear(palette_color);
                    float3 palette_oklab = linear_to_oklab(palette_linear);
                    float3 delta = target_oklab - palette_oklab;
                    float distance = dot(delta, delta);

                    if (distance < min_distance)
                    {
                        min_distance = distance;
                        closest_color = palette_color;
                    }
                }
                return closest_color;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float2 uv = input.texcoord.xy;

                // AddBlitPass / BlitMaterialParameters bind the source texture as
                // _BlitTexture, not _MainTex.
                float3 srgb_color = SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearClamp, uv, _BlitMipLevel).rgb;
                float3 linear_color = srgb_to_linear(srgb_color);
                float3 oklab_color = linear_to_oklab(linear_color);
                float3 matched_color = find_closest_palette_color(oklab_color);
                return half4(matched_color, 1.0);
            }
            ENDHLSL
        }
    }
}
