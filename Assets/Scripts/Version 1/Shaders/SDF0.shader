Shader "Custom/SDF_FullScreen_Cubes"
{
    Properties
    {
        _BaseColor("Cube Color", Color) = (1, 0, 0, 1)
        _Ambient("Ambient Intensity", Range(0, 1)) = 0.2
        _Smoothness("Smooth Blend", Range(0, 1)) = 0.1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            ZWrite Off
            ZTest Always // Full screen passes usually sit on top or handle depth manually

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            struct Attributes
            {
                uint vertexID : SV_VertexID;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _BaseColor;
            float _Ambient;
            float _Smoothness;

            // --- 1. SDF FUNCTIONS ---
            float sdCube(float3 p, float3 b) {
                float3 q = abs(p) - b;
                return length(max(q, 0.0)) + min(max(q.x, max(q.y, q.z)), 0.0);
            }

            float smin(float a, float b, float k) {
                float h = max(k - abs(a - b), 0.0) / k;
                return min(a, b) - h * h * k * (1.0 / 4.0);
            }

            float map(float3 p) {
                float3 size = float3(0.4, 0.4, 0.4);
                
                float d1 = sdCube(p - float3(-1.5, 0, 0), size);
                float d2 = sdCube(p - float3( 0.0, 0, 0), size);
                float d3 = sdCube(p - float3( 1.5, 0, 0), size);

                // Using smin so they blend! Set _Smoothness to 0 for hard edges
                return smin(d1, smin(d2, d3, _Smoothness), _Smoothness);
            }

            // --- 2. NORMAL ESTIMATION ---
            float3 calcNormal(float3 p) {
                const float h = 0.001;
                const float2 k = float2(1, -1);
                return normalize(k.xyy * map(p + k.xyy * h) +
                                 k.yyx * map(p + k.yyx * h) +
                                 k.yxy * map(p + k.yxy * h) +
                                 k.xxx * map(p + k.xxx * h));
            }

            // --- 3. VERTEX SHADER (Procedural Quad) ---
            Varyings vert(Attributes IN) {
                Varyings OUT;
                // Generate a full-screen triangle procedurally
                OUT.uv = float2((IN.vertexID << 1) & 2, IN.vertexID & 2);
                OUT.positionHCS = float4(OUT.uv * 2.0 - 1.0, 0.0, 1.0);
                #if UNITY_UV_STARTS_AT_TOP
                OUT.uv.y = 1.0 - OUT.uv.y;
                #endif
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target {
                // 1. Reconstruct World Space Ray
                float3 rayOrigin = _WorldSpaceCameraPos;
                
                // Convert UV to Screen Space Position
                float2 screenPos = IN.uv * 2.0 - 1.0;
                
                // Transform screen point to View Space then to World Space
                float4 target = mul(UNITY_MATRIX_I_P, float4(screenPos, 0, 1));
                float3 rayDir = mul((float3x3)UNITY_MATRIX_I_V, target.xyz);
                rayDir = normalize(rayDir);

                // 2. Raymarching Loop
                float distTravelled = 0.0;
                for (int i = 0; i < 100; i++) {
                    float3 currentPos = rayOrigin + rayDir * distTravelled;
                    float d = map(currentPos);
                    
                    if (d < 0.001) {
                        float3 normal = calcNormal(currentPos);
                        Light mainLight = GetMainLight();
                        
                        float diffuse = saturate(dot(normal, mainLight.direction));
                        float3 color = _BaseColor.rgb * (diffuse + _Ambient);
                        
                        return half4(color, 1.0);
                    }
                    
                    distTravelled += d;
                    if (distTravelled > 50.0) break; 
                }

                // 3. Background / Scene Integration
                // Since this is a full screen pass, we 'discard' to see the original scene
                discard;
                return 0;
            }
            ENDHLSL
        }
    }
}