Shader "PixelPerfect/Water"
{
    Properties
    {
        [KeywordEnum(Linear, Gradient Texture)] _ColorMode ("[FOLDOUT(Colors){9}]Source{Colors}", Float) = 0.0
        _ColorShallow ("[_COLORMODE_LINEAR]Shallow", Color) = (0.35, 0.6, 0.75, 0.8) // Color alpha controls opacity
        _ColorDeep ("[_COLORMODE_LINEAR]Deep", Color) = (0.65, 0.9, 1.0, 1.0)
        [NoScaleOffset] _ColorGradient("[_COLORMODE_GRADIENT_TEXTURE]Gradient", 2D) = "white" {}
        _FadeDistance("Shallow Depth", Float) = 0.5
        _WaterDepth("Gradient Size", Float) = 5.0
        [Toggle(_PIXELART_DEPTH)] _PixelArtDepth("Use PixelArt Depth", Float) = 0.0
        _LightContribution("Light Color Contribution", Range(0, 1)) = 0
        _WaterClearness("Transparency", Range(0, 1)) = 0.3
        _ShadowStrength("Shadow Strength", Range(0, 1)) = 0.35

        [Toggle(_PLANAR_REFLECTIONS)] _PlanarReflections("[FOLDOUT(Reflections){4}]Enable Planar Reflections{Reflections}", Float) = 0.0
        _ReflectionStrength("Strength{Reflections}", Range(0, 1)) = 1.0
        _ReflectionColorInfluence("Color Influence{Reflections}", Range(0, 1)) = 1.0
        _ReflectionTint("Tint{Reflections}", Color) = (1, 1, 1, 1)

        [Toggle(_DETAIL_MAP)] _EnableDetailMap("[FOLDOUT(Detail Map){9}]Enable Detail Map{Detail}", Float) = 0.0
        [NoScaleOffset] _DetailMap("Texture{Detail}", 2D) = "white" {}
        _DetailColor("Color{Detail}", Color) = (1, 1, 1, 1)
        _DetailAmount("Amount{Detail}", Range(0, 1)) = 0.5
        _DetailSharpness("Sharpness{Detail}", Range(0, 1)) = 0.5
        _DetailScale("Scale{Detail}", Float) = 1.0
        _DetailSpeed("Speed{Detail}", Float) = 0.1
        _DetailDirection("Direction{Detail}", Range(-1.0, 1.0)) = 0
        _DetailStrength("Strength{Detail}", Range(0, 1)) = 0.5

        _CrestColor("[FOLDOUT(Crest){3}]Color{Crest}", Color) = (1.0, 1.0, 1.0, 0.9)
        _CrestSize("Size{Crest}", Range(0, 1)) = 0.1
        _CrestSharpness("Sharp transition{Crest}", Range(0, 1)) = 0.1

        [KeywordEnum(None, Round, Grid, Pointy)] _WaveMode ("[FOLDOUT(Wave Geometry){6}]Shape{Wave}", Float) = 1.0
        _WaveSpeed("[!_WAVEMODE_NONE]Speed{Wave}", Float) = 0.5
        _WaveAmplitude("[!_WAVEMODE_NONE]Amplitude{Wave}", Float) = 0.25
        _WaveFrequency("[!_WAVEMODE_NONE]Frequency{Wave}", Float) = 1.0
        _WaveDirection("[!_WAVEMODE_NONE]Direction{Wave}", Range(-1.0, 1.0)) = 0
        _WaveNoise("[!_WAVEMODE_NONE]Noise{Wave}", Range(0, 1)) = 0.25

        [KeywordEnum(None, Gradient Noise, Texture)] _FoamMode ("[FOLDOUT(Foam Surface){10}]Source{Foam Surface}", Float) = 1.0
        [NoScaleOffset] _NoiseMap("[_FOAMMODE_TEXTURE]Texture{Foam Surface}", 2D) = "white" {}
        _FoamColor("[!_FOAMMODE_NONE]Surface Color{Foam Surface}", Color) = (1, 1, 1, 1)
        _FoamAmount("[!_FOAMMODE_NONE]Amount{Foam Surface}", Range(0, 3)) = 0.25
        [Space]
        _FoamScale("[!_FOAMMODE_NONE]Scale{Foam Surface}", Range(0, 3)) = 1
        _FoamStretchX("[!_FOAMMODE_NONE]Stretch X{Foam Surface}", Range(0, 10)) = 1
        _FoamStretchY("[!_FOAMMODE_NONE]Stretch Y{Foam Surface}", Range(0, 10)) = 1
        [Space]
        _FoamSharpness("[!_FOAMMODE_NONE]Sharpness{Foam Surface}", Range(0, 1)) = 0.5
        [Space]
        _FoamSpeed("[!_FOAMMODE_NONE]Speed{Foam Surface}", Float) = 0.1
        _FoamDirection("[!_FOAMMODE_NONE]Direction{Foam Surface}", Range(-1.0, 1.0)) = 0

        [KeywordEnum(None, Gradient Noise, Texture)] _FoamShoreMode ("[FOLDOUT(Foam Shore){11}]Source{Foam Shore}", Float) = 1.0
        [NoScaleOffset] _FoamShoreNoiseMap("[_FOAMSHOREMODE_TEXTURE]Texture{Foam Shore}", 2D) = "white" {}
        _FoamShoreColor("[!_FOAMSHOREMODE_NONE]Shore Color{Foam Shore}", Color) = (1, 1, 1, 1)
        [Space]
        _FoamDepth("[!_FOAMSHOREMODE_NONE]Shore Depth{Foam Shore}", Float) = 0.5
        _FoamNoiseAmount("[!_FOAMSHOREMODE_NONE]Shore Blending{Foam Shore}", Range(0.0, 1.0)) = 1.0
        [Space]
        _FoamShoreScale("[!_FOAMSHOREMODE_NONE]Scale{Foam Shore}", Range(0, 3)) = 1
        _FoamShoreStretchX("[!_FOAMSHOREMODE_NONE]Stretch X{Foam Shore}", Range(0, 10)) = 1
        _FoamShoreStretchY("[!_FOAMSHOREMODE_NONE]Stretch Y{Foam Shore}", Range(0, 10)) = 1
        [Space]
        _FoamShoreSharpness("[!_FOAMSHOREMODE_NONE]Sharpness{Foam Shore}", Range(0, 1)) = 0.5
        [Space]
        _FoamShoreSpeed("[!_FOAMSHOREMODE_NONE]Speed{Foam Shore}", Float) = 0.1
        _FoamShoreDirection("[!_FOAMSHOREMODE_NONE]Direction{Foam Shore}", Range(-1.0, 1.0)) = 0

        _RefractionFrequency("[FOLDOUT(Refraction){4}]Frequency", Float) = 35
        _RefractionAmplitude("Amplitude", Range(0, 0.1)) = 0.01
        _RefractionSpeed("Speed", Float) = 0.1
        _RefractionScale("Scale", Float) = 1

        [Toggle(_WATER_FOG)] _WaterFog("[FOLDOUT(Fog){3}]Enable Fog{Fog}", Float) = 0.0
        _WaterFogColor("[_WATER_FOG]Fog Color{Fog}", Color) = (0.25, 0.55, 0.7, 0.8)
        _WaterFogDensity("[_WATER_FOG]Fog Density{Fog}", Range(0, 2)) = 0.35

        [Toggle(_PALETTE_QUANTIZATION)] _PaletteQuantization("[FOLDOUT(Palette Quantization){1}]Enable{Palette Quantization}", Float) = 0.0

        [HideInInspector] [ToggleOff] _Opaque("Opaque", Float) = 0.0
        [HideInInspector] _QueueOffset("Queue offset", Float) = 0.0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"
        }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        Lighting Off
        ZWrite[_ZWrite]

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
        ENDHLSL

        Pass
        {
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma target 2.0

            #pragma shader_feature_local _COLORMODE_LINEAR _COLORMODE_GRADIENT_TEXTURE
            #pragma shader_feature_local _FOAMMODE_NONE _FOAMMODE_GRADIENT_NOISE _FOAMMODE_TEXTURE
            #pragma shader_feature_local _FOAMSHOREMODE_NONE _FOAMSHOREMODE_GRADIENT_NOISE _FOAMSHOREMODE_TEXTURE
            #pragma shader_feature_local _WAVEMODE_NONE _WAVEMODE_ROUND _WAVEMODE_GRID _WAVEMODE_POINTY
            #pragma shader_feature_local _PIXELART_DEPTH
            #pragma shader_feature_local _PLANAR_REFLECTIONS
            #pragma shader_feature_local _DETAIL_MAP
            #pragma shader_feature_local _WATER_FOG
            #pragma shader_feature_local _PALETTE_QUANTIZATION

            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

            #pragma vertex vert
            #pragma fragment frag

            #if defined(_COLORMODE_GRADIENT_TEXTURE)
            TEXTURE2D(_ColorGradient);
            SAMPLER(sampler_ColorGradient);
            #endif

            TEXTURE2D(_NoiseMap);
            SAMPLER(sampler_NoiseMap);
            TEXTURE2D(_FoamShoreNoiseMap);
            SAMPLER(sampler_FoamShoreNoiseMap);

            #if defined(_DETAIL_MAP)
            TEXTURE2D(_DetailMap);
            SAMPLER(sampler_DetailMap);
            #endif

            #if defined(_PIXELART_DEPTH)
            TEXTURE2D(_PixelArtDepthTex);
            SAMPLER(sampler_PixelArtDepthTex);
            #endif

            // Planar Reflection Globals (set externally via Shader.SetGlobalTexture/Float)
            TEXTURE2D(_WaterReflectionTex);
            SAMPLER(sampler_WaterReflectionTex);
            float _FresnelPower;
            float _NormalStrength;
            float _WaterHeight;
            float4x4 _WaterPlaneWorldToLocal;

            // Palette Quantization Globals (set externally via Shader.SetGlobalTexture/Float)
            #if defined(_PALETTE_QUANTIZATION)
            TEXTURE2D(_PaletteTex);
            SAMPLER(sampler_PaletteTex);
            float _PaletteCount;
            #endif

            CBUFFER_START(UnityPerMaterial)
            float _FadeDistance, _WaterDepth;
            half _LightContribution;
            half _WaveFrequency, _WaveAmplitude, _WaveSpeed, _WaveDirection, _WaveNoise;
            half _WaterClearness, _CrestSize, _CrestSharpness, _ShadowStrength;
            half4 _CrestColor;
            half4 _FoamColor;
            half4 _FoamShoreColor;
            half _FoamAmount, _FoamScale, _FoamSharpness, _FoamStretchX, _FoamStretchY, _FoamSpeed, _FoamDirection;
            half _FoamDepth, _FoamNoiseAmount, _FoamShoreScale, _FoamShoreSharpness, _FoamShoreStretchX,
                 _FoamShoreStretchY, _FoamShoreSpeed, _FoamShoreDirection, _RefractionFrequency, _RefractionAmplitude,
                 _RefractionSpeed, _RefractionScale;
            half4 _WaterFogColor;
            half _WaterFogDensity;
            
            // Reflection Local Properties
            half _ReflectionStrength;
            half _ReflectionColorInfluence;
            half4 _ReflectionTint;

            // Detail Map Properties
            half4 _DetailColor;
            half _DetailAmount;
            half _DetailSharpness;
            half _DetailScale;
            half _DetailSpeed;
            half _DetailDirection;
            half _DetailStrength;

            float4 _NoiseMap_ST;
            half4 _ColorShallow, _ColorDeep;
            float4 _ColorGradient_ST;
            CBUFFER_END

            struct VertexInput
            {
                float4 positionOS : POSITION;
                float2 texcoord : TEXCOORD0;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct VertexOutput
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS : TEXCOORD6;
                float2 uv : TEXCOORD0;
                float4 screenPosition : TEXCOORD1;
                float waveHeight : TEXCOORD2;
                float3 normal : TEXCOORD3; 
                float3 viewDir : TEXCOORD4;
                half fogFactor : TEXCOORD5;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            float2 GradientNoise_Dir(float2 p)
            {
                p = p % 289;
                float x = (34 * p.x + 1) * p.x % 289 + p.y;
                x = (34 * x + 1) * x % 289;
                x = frac(x / 41) * 2 - 1;
                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
            }

            float GradientNoise(float2 UV, float Scale)
            {
                const float2 p = UV * Scale;
                const float ip = floor(p);
                float2 fp = frac(p);
                const float d00 = dot(GradientNoise_Dir(ip), fp);
                const float d01 = dot(GradientNoise_Dir(ip + float2(0, 1)), fp - float2(0, 1));
                const float d10 = dot(GradientNoise_Dir(ip + float2(1, 0)), fp - float2(1, 0));
                const float d11 = dot(GradientNoise_Dir(ip + float2(1, 1)), fp - float2(1, 1));
                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
            }

            inline void SampleDepths(float2 uv, VertexOutput i, out float scene_depth, out float surface_depth)
            {
                const float is_ortho = unity_OrthoParams.w;
                const float is_persp = 1.0 - unity_OrthoParams.w;
                #if defined(_PIXELART_DEPTH)
                scene_depth = SAMPLE_TEXTURE2D(_PixelArtDepthTex, sampler_PixelArtDepthTex, uv).r;
                surface_depth = lerp(_ProjectionParams.z, _ProjectionParams.y, i.screenPosition.z) * is_ortho + i.screenPosition.w * is_persp;
                #else
                const float depth_packed = SampleSceneDepth(uv);
                scene_depth = lerp(_ProjectionParams.z, _ProjectionParams.y, depth_packed) * is_ortho + LinearEyeDepth(depth_packed, _ZBufferParams) * is_persp;
                surface_depth = lerp(_ProjectionParams.z, _ProjectionParams.y, i.screenPosition.z) * is_ortho + i.screenPosition.w * is_persp;
                #endif
            }

            inline float DepthFade(float2 uv, VertexOutput i)
            {
                float scene_depth, surface_depth;
                SampleDepths(uv, i, scene_depth, surface_depth);
                return saturate((scene_depth - surface_depth - _FadeDistance) / _WaterDepth);
            }

            inline float WaterThickness(float2 uv, VertexOutput i)
            {
                float scene_depth, surface_depth;
                SampleDepths(uv, i, scene_depth, surface_depth);
                return max(scene_depth - surface_depth - _FadeDistance, 0.0);
            }

            inline float SineWave(float3 pos, float offset)
            {
                return sin(offset + _Time.z * _WaveSpeed + (pos.x * sin(offset + _WaveDirection * PI) + pos.z * cos(offset + _WaveDirection * PI)) * _WaveFrequency);
            }

            inline float WaveHeight(float2 texcoord, float3 position)
            {
                float s = 0;
                #if !defined(_WAVEMODE_NONE)
                    float2 noise_uv = texcoord * _WaveFrequency;
                    float noise = (GradientNoise(noise_uv, 1.0) * 2.0 - 1.0) * _WaveNoise;
                    s = SineWave(position, noise);
                    #if defined(_WAVEMODE_GRID)
                        s *= SineWave(position, HALF_PI + noise);
                    #endif
                    #if defined(_WAVEMODE_POINTY)
                        s = 1.0 - abs(s);
                    #endif
                #endif
                return s;
            }

            inline void AdditionalLights(float3 WorldPosition, out half3 Color, out half Attenuation) {
                Color = 0; Attenuation = 0;
                #ifdef _ADDITIONAL_LIGHTS
                const uint numAdditionalLights = GetAdditionalLightsCount();
                for (uint lightI = 0; lightI < numAdditionalLights; lightI++) {
                    Light light = GetAdditionalLight(lightI, WorldPosition, half4(1,1,1,1));
                    Color += light.color;
                    Attenuation += light.distanceAttenuation * light.shadowAttenuation;
                }
                Attenuation = saturate(Attenuation);
                #endif
            }

            #if defined(_PALETTE_QUANTIZATION)
            float3 PaletteRgbToXyz(float3 rgb)
            {
                rgb = pow(max(rgb, float3(0.0, 0.0, 0.0)), 2.2);
                float3x3 rgbToXyz = float3x3(
                    0.4124564, 0.3575761, 0.1804375,
                    0.2126729, 0.7151522, 0.0721750,
                    0.0193339, 0.1191920, 0.9503041
                );
                return mul(rgbToXyz, rgb);
            }

            float3 PaletteXyzToLab(float3 xyz)
            {
                xyz /= float3(0.95047, 1.0, 1.08883);
                float3 safeXyz = max(xyz, float3(0.0, 0.0, 0.0));
                float3 f = safeXyz > 0.008856 ? pow(safeXyz, 1.0 / 3.0) : (7.787 * xyz + 16.0 / 116.0);
                float l = 116.0 * f.y - 16.0;
                float a = 500.0 * (f.x - f.y);
                float b = 200.0 * (f.y - f.z);
                return float3(l, a, b);
            }

            float PaletteColorDistanceLab(float3 color1, float3 color2)
            {
                float3 lab1 = PaletteXyzToLab(PaletteRgbToXyz(color1));
                float3 lab2 = PaletteXyzToLab(PaletteRgbToXyz(color2));
                return length(lab1 - lab2);
            }

            float3 QuantizeToPalette(float3 inputColor)
            {
                int paletteCount = clamp((int)round(_PaletteCount), 0, 256);
                if (paletteCount <= 0)
                    return inputColor;

                float3 bestColor = inputColor;
                float bestDistance = 1e20;

                [loop]
                for (int idx = 0; idx < 256; idx++)
                {
                    if (idx >= paletteCount)
                        break;

                    float2 lutUV = float2((idx + 0.5f) / paletteCount, 0.5f);
                    float3 paletteColor = SAMPLE_TEXTURE2D_LOD(_PaletteTex, sampler_PaletteTex, lutUV, 0).rgb;

                    float dist = PaletteColorDistanceLab(inputColor, paletteColor);
                    if (dist < bestDistance)
                    {
                        bestDistance = dist;
                        bestColor = paletteColor;
                    }
                }

                return bestColor;
            }
            #endif

            VertexOutput vert(VertexInput i)
            {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID(i);
                UNITY_TRANSFER_INSTANCE_ID(i, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                const float3 originalPositionWS = TransformObjectToWorld(i.positionOS.xyz);
                const float s = WaveHeight(i.texcoord, originalPositionWS);
                o.waveHeight = s;
                o.positionWS = originalPositionWS;
                o.positionWS.y += s * _WaveAmplitude;

                o.positionHCS = TransformWorldToHClip(o.positionWS);
                o.screenPosition = ComputeScreenPos(o.positionHCS);
                o.uv = i.texcoord;
                o.viewDir = GetCameraPositionWS() - o.positionWS;

                const VertexNormalInputs normalInput = GetVertexNormalInputs(i.normalOS, i.tangentOS);
                const float sample_dist = 0.01;
                float3 pos_tangent = originalPositionWS + normalInput.tangentWS * sample_dist;
                pos_tangent.y += WaveHeight(i.texcoord, pos_tangent) * _WaveAmplitude;
                float3 pos_bitangent = originalPositionWS + normalInput.bitangentWS * sample_dist;
                pos_bitangent.y += WaveHeight(i.texcoord, pos_bitangent) * _WaveAmplitude;
                o.normal = normalize(cross(pos_tangent - o.positionWS, pos_bitangent - o.positionWS));

                o.fogFactor = ComputeFogFactor(o.positionHCS.z);
                return o;
            }

            half4 frag(VertexOutput i) : SV_TARGET
            {
                UNITY_SETUP_INSTANCE_ID(i);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
                
                // Base Refraction
                const float2 noise_uv_refr = i.uv * _RefractionFrequency + _Time.zz * _RefractionSpeed;
                const float noise11_refr = (GradientNoise(noise_uv_refr, _RefractionScale) * 2.0 - 1.0);
                const float2 screen_uv = i.screenPosition.xy / i.screenPosition.w;
                const float depth_fade_raw = DepthFade(screen_uv, i);
                float2 displaced_uv = screen_uv + noise11_refr * _RefractionAmplitude * depth_fade_raw;
                float depth_fade = depth_fade_raw;

                #if !defined(_PIXELART_DEPTH)
                depth_fade = DepthFade(displaced_uv, i);
                if (depth_fade <= 0.0f) displaced_uv = screen_uv;
                #endif

                const half3 scene_color = SampleSceneColor(displaced_uv);
                half3 c = scene_color;

                // Water Color
                half4 depth_color;
                half4 color_shallow;
                #if defined(_COLORMODE_LINEAR)
                depth_color = lerp(_ColorShallow, _ColorDeep, depth_fade);
                color_shallow = _ColorShallow;
                #elif defined(_COLORMODE_GRADIENT_TEXTURE)
                depth_color = SAMPLE_TEXTURE2D(_ColorGradient, sampler_ColorGradient, float2(depth_fade, 0.5));
                color_shallow = SAMPLE_TEXTURE2D(_ColorGradient, sampler_ColorGradient, float2(0.0, 0.5));
                #endif

                c = lerp(depth_color.rgb, c, _WaterClearness * depth_color.a);

                #if defined(_WATER_FOG)
                    float2 fog_uv = displaced_uv;
                    #if defined(_PIXELART_DEPTH)
                        fog_uv = screen_uv;
                    #endif
                    float fogThickness = WaterThickness(fog_uv, i);
                    float fogFactor = 1.0 - exp2(-fogThickness * _WaterFogDensity);
                    c = lerp(c, _WaterFogColor.rgb, saturate(fogFactor) * _WaterFogColor.a);
                #endif

                // Planar Reflections
                #if defined(_PLANAR_REFLECTIONS)
                    // Hardcoded flip on X to match reflection camera behavior
                    float2 reflect_uv = screen_uv;
                    reflect_uv.x = 1.0 - reflect_uv.x;

                    // Use same refraction distortion, scaled by depth to keep seams tight at waterline
                    reflect_uv += noise11_refr * _RefractionAmplitude * depth_fade;

                    half4 reflectionSample = SAMPLE_TEXTURE2D(_WaterReflectionTex, sampler_WaterReflectionTex, reflect_uv);
                    half3 reflection = reflectionSample.rgb;
                    half reflectionAlpha = reflectionSample.a;

                    float3 normalWS = normalize(i.normal);
                    float3 viewWS = normalize(i.viewDir);
                    float fresnel = pow(1.0 - saturate(dot(normalWS, viewWS)), _FresnelPower);

                    half3 reflectionTinted = lerp(reflection, reflection * _ReflectionTint.rgb, _ReflectionColorInfluence);
                    c = lerp(c, reflectionTinted, fresnel * _ReflectionStrength * reflectionAlpha);
                #endif

                // Detail Map - blends on top of water surface (after reflections)
                #if defined(_DETAIL_MAP)
                    float cs_detail = cos(_DetailDirection * PI);
                    float sn_detail = sin(_DetailDirection * PI);
                    float2 detail_uv = float2(i.uv.x * cs_detail - i.uv.y * sn_detail, i.uv.x * sn_detail + i.uv.y * cs_detail);
                    detail_uv = detail_uv * _DetailScale + _Time.yy * _DetailSpeed;
                    half detail_noise = SAMPLE_TEXTURE2D(_DetailMap, sampler_DetailMap, detail_uv).r;

                    // Threshold: higher amount = lower threshold = more detail visible
                    float d_blur = 1.0 - _DetailSharpness + 1e-6;
                    float threshold = 1.0 - _DetailAmount;
                    float detail_mask = smoothstep(threshold, threshold + d_blur, detail_noise);

                    c = lerp(c, _DetailColor.rgb, detail_mask * _DetailColor.a * _DetailStrength);
                #endif

                // Crest
                const half c_inv = 1.0f - _CrestSize;
                c = lerp(c, _CrestColor.rgb, smoothstep(c_inv, saturate(c_inv + (1.0f - _CrestSharpness)), i.waveHeight) * _CrestColor.a);

                // Foam - Surface
                #if !defined(_FOAMMODE_NONE)
                    float noise_surface;
                    #if defined(_FOAMMODE_TEXTURE)
                        const float2 rot_uv_surface = i.uv * cos(_FoamDirection * PI) + float2(i.uv.y, -i.uv.x) * sin(_FoamDirection * PI);
                        noise_surface = SAMPLE_TEXTURE2D(_NoiseMap, sampler_NoiseMap, (rot_uv_surface * 100.0 + _Time.zz * _FoamSpeed) * float2(_FoamStretchX, _FoamStretchY) / (_FoamScale * 100.0)).r;
                    #else
                        const float cs_surface = cos(_FoamDirection * PI);
                        const float sn_surface = sin(_FoamDirection * PI);
                        const float2 rot_uv_gn_surface = float2(i.uv.x * cs_surface - i.uv.y * sn_surface, i.uv.x * sn_surface + i.uv.y * cs_surface);
                        noise_surface = GradientNoise((rot_uv_gn_surface * 100.0 + _Time.zz * _FoamSpeed) * float2(_FoamStretchX, _FoamStretchY), _FoamScale);
                    #endif
                    float surface_blur = 1.0 - _FoamSharpness + 1e-6;
                    float foam_surface = smoothstep(0.5 - surface_blur * 0.5, 0.5 + surface_blur * 0.5, smoothstep(noise_surface, noise_surface + surface_blur, _FoamAmount));
                    c = lerp(c, _FoamColor.rgb, foam_surface * _FoamColor.a);
                #endif

                // Foam - Shore
                #if !defined(_FOAMSHOREMODE_NONE)
                    float noise_shore;
                    #if defined(_FOAMSHOREMODE_TEXTURE)
                        const float2 rot_uv_shore = i.uv * cos(_FoamShoreDirection * PI) + float2(i.uv.y, -i.uv.x) * sin(_FoamShoreDirection * PI);
                        noise_shore = SAMPLE_TEXTURE2D(_FoamShoreNoiseMap, sampler_FoamShoreNoiseMap, (rot_uv_shore * 100.0 + _Time.zz * _FoamShoreSpeed) * float2(_FoamShoreStretchX, _FoamShoreStretchY) / (_FoamShoreScale * 100.0)).r;
                    #else
                        const float cs_shore = cos(_FoamShoreDirection * PI);
                        const float sn_shore = sin(_FoamShoreDirection * PI);
                        const float2 rot_uv_gn_shore = float2(i.uv.x * cs_shore - i.uv.y * sn_shore, i.uv.x * sn_shore + i.uv.y * cs_shore);
                        noise_shore = GradientNoise((rot_uv_gn_shore * 100.0 + _Time.zz * _FoamShoreSpeed) * float2(_FoamShoreStretchX, _FoamShoreStretchY), _FoamShoreScale);
                    #endif
                    float shore_blur = 1.0 - _FoamShoreSharpness + 1e-6;
                    float shore_fade = saturate(depth_fade / _FoamDepth);
                    float foam_shore = smoothstep(0.5 - shore_blur * 0.5, 0.5 + shore_blur * 0.5, noise_shore);
                    foam_shore = saturate(smoothstep(0.4, 0.1, shore_fade) + smoothstep(1, 0.4, shore_fade) * foam_shore * _FoamNoiseAmount);
                    c = lerp(c, _FoamShoreColor.rgb, foam_shore * _FoamShoreColor.a);
                #endif

                // Lighting
                #if defined(_MAIN_LIGHT_SHADOWS) || defined(_MAIN_LIGHT_SHADOWS_CASCADE) || defined(_MAIN_LIGHT_SHADOWS_SCREEN)
                    VertexPositionInputs vInput = (VertexPositionInputs)0;
                    vInput.positionWS = i.positionWS;
                    float4 shadowCoord = GetShadowCoord(vInput);
                    half shadowAtten = MainLightRealtimeShadow(shadowCoord);
                    c = lerp(c, c * color_shallow.rgb, _ShadowStrength * (1.0h - shadowAtten));
                #endif

                c *= lerp(half3(1, 1, 1), _MainLightColor.rgb, _LightContribution);

                #if defined(_ADDITIONAL_LIGHTS)
                    half3 lCol; half lAtten;
                    AdditionalLights(i.positionWS, lCol, lAtten);
                    c += lerp(half3(1, 1, 1), lCol, _LightContribution) * lAtten;
                #endif

                c = MixFog(c, i.fogFactor);

                #if defined(_PALETTE_QUANTIZATION)
                c = QuantizeToPalette(c);
                #endif

                return half4(c, 1);
            }
            ENDHLSL
        }
    }
    CustomEditor "PixelWaterEditor"
}
