Shader "Custom/HDRP/FurShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _TopColor ("Top Color", Color) = (1, 1, 1, 1)
        _FurLength ("Fur Length", Range(0, 0.5)) = 0.1
        _FurDensity ("Fur Density", Range(0, 1)) = 0.5
        _NoiseTex ("Noise Texture", 2D) = "white" {}
    }
    
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "HDRenderPipeline" }
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "Forward" }
            
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            
            // HDRP includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
            
            // Properties
            float4 _BaseColor;
            float4 _TopColor;
            float _FurLength;
            float _FurDensity;
            sampler2D _NoiseTex;
            
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 positionWS : TEXCOORD2;
                float furLayer : TEXCOORD3;
            };
            
            Varyings Vert(Attributes input)
            {
                Varyings output;
                float3 positionOS = input.positionOS;
                
                // Offset position along normal for fur effect
                // Using a simple layer hack since we’re not doing full instancing
                float layer = input.positionOS.x * 0.1; // Fake layer based on position
                positionOS += input.normalOS * _FurLength * layer;
                output.positionWS = TransformObjectToWorld(positionOS);
                output.positionCS = TransformWorldToHClip(output.positionWS);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.uv = input.uv;
                output.furLayer = layer;
                return output;
            }
            
            float4 Frag(Varyings input) : SV_Target
            {
                // Sample noise for fur density
                float noise = tex2D(_NoiseTex, input.uv).r;
                clip(noise - (1.0 - _FurDensity * (1.0 - input.furLayer)));
                
                // Gradient from base to top
                float4 color = lerp(_BaseColor, _TopColor, input.furLayer);
                
                // HDRP lighting - Get main light
                Light mainLight = GetMainLight();
                float3 lightDir = mainLight.direction;
                float3 normal = normalize(input.normalWS);
                float diffuse = max(0, dot(normal, lightDir));
                
                return float4(color.rgb * diffuse * mainLight.color, color.a);
            }
            
            ENDHLSL
        }
    }
    Fallback "Hidden/InternalErrorShader"
}