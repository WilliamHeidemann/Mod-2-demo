Shader "Custom/SDFSpherePass_VertexID"
{
    Properties
    {
        _Radius ("Sphere Radius", Range(0, 1)) = 0.5
        _Color ("Color", Color) = (1,1,1,1)
        _Ambient("Ambient Intensity", Range(0, 1)) = 0.2
        _Smoothness("Smooth Blend", Range(0, 1)) = 0.1
        _Size ("Size", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"
        }
        LOD 100
        ZWrite Off ZTest Always
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                uint vertexID : SV_VertexID;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _Radius;
            float4 _Color;
            float _Ambient;
            float _Smoothness;
            float _Size;
            
            StructuredBuffer<float4> _PositionsBuffer;
            int _PositionsCount;

            StructuredBuffer<float4> _SocketsBuffer;
            int _SocketsCount;
            
            // Simple SDF for a sphere
            float sdSphere(float3 position, float radius)
            {
                return length(position) - radius;
            }

            float sdCube(float3 p, float3 b)
            {
                float3 q = abs(p) - b;
                return length(max(q, 0.0)) + min(max(q.x, max(q.y, q.z)), 0.0);
            }

            float smoothMin(float a, float b, float k)
            {
                float h = max(k - abs(a - b), 0.0) / k;
                return min(a, b) - h * h * k * (1.0 / 4.0);
            }

            float map(float3 p)
            {
                float3 size = float3(_Size, _Size, _Size);
                
                float minDist = 1000.0;
                for (int i = 0; i < _PositionsCount; ++i)
                {
                    minDist = smoothMin(minDist, sdCube(p - _PositionsBuffer[i].xyz, size), _Smoothness);
                }

                for (int i = 0; i < _SocketsCount; ++i)
                {
                    minDist = smoothMin(minDist, sdSphere(p - _SocketsBuffer[i].xyz, _Radius), _Smoothness);
                }
 
                return minDist;
            }

            // Estimate normal via central differences
            float3 calcNormal(float3 p)
            {
                // A small offset for sampling
                float2 e = float2(0.001, 0);

                return normalize(float3(
                    map(p + e.xyy) - map(p - e.xyy),
                    map(p + e.yxy) - map(p - e.yxy),
                    map(p + e.yyx) - map(p - e.yyx)
                ));
            }

            Varyings vert(Attributes input)
            {
                Varyings output;

                // Generates a triangle that covers the whole screen
                // ID 0: (-1, -1), ID 1: (-1, 3), ID 2: (3, -1)
                float2 uv = float2((input.vertexID << 1) & 2, input.vertexID & 2);
                output.positionHCS = float4(uv * 2.0 - 1.0, 0.0, 1.0);
                output.uv = uv;

                // Handle vertical flip for certain APIs (DX11/12/Vulkan)
                if (_ProjectionParams.x < 0)
                    output.uv.y = 1.0 - output.uv.y;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // 1. Reconstruct World Space Ray
                // We convert screen UV to NDCPosition, then to World Space
                float3 positionWS = ComputeWorldSpacePosition(input.uv, 0.5, UNITY_MATRIX_I_VP);
                float3 rayOrigin = _WorldSpaceCameraPos;
                float3 rayDirection = normalize(positionWS - rayOrigin);

                // 2. Raymarching
                float marchDistance = 0.0;
                float maxDistance = 100.0;

                for (int i = 0; i < 80; i++)
                {
                    float3 samplePoint = rayOrigin + rayDirection * marchDistance;
                    float sampleDistance = map(samplePoint);

                    if (sampleDistance < 0.001) // Hit
                    {
                        float3 normal = calcNormal(samplePoint);
                        Light mainLight = GetMainLight();

                        // Shading
                        float diffuse = saturate(dot(normal, mainLight.direction));
                        float3 color = _Color.rgb * (diffuse + _Ambient);

                        return float4(color, 1.0);
                    }

                    marchDistance += sampleDistance;
                    if (marchDistance > maxDistance) break;
                }

                // If nothing is hit, we don't want to draw over the existing scene
                discard;
                return 0;
            }
            ENDHLSL
        }
    }
}