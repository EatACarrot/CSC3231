// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/PlanetShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Curvature ("Curvature", Float) = 0.01
        _MountainTexture("MountainTexture", 2D) = "white" {}
        _WaterTexture("WaterTexture", 2D) = "white" {}
        _Height ("Height", Float) = 10.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag      

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float height : FOG;

                float3 normal : NORMAL;
                float3 worldPos : POSITIONT;
            };

            sampler2D _MainTex;
            sampler2D _MountainTexture;
            sampler2D _WaterTexture;

            float4 _MainTex_ST;
            uniform float _Curvature;
            uniform float _Height;

            float3 camPosition;
            float3 lightPosition;
            float4 lightColour;

            float lightIntensity;
            float lightRange;
            float specularPower;

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(UNITY_MATRIX_M, v.vertex).xyz;
                o.height = v.vertex.y;

                //curving the furthest z and x vertices to crate a horizon effect
                float4 vv = mul(unity_ObjectToWorld, v.vertex);
                vv.xyz -= _WorldSpaceCameraPos.xyz;
                vv = float4(0.0f,(( vv.z * vv.z) + (vv.x * vv.x)) * -_Curvature, 0.0f, 0.0f);
                v.vertex += mul(unity_WorldToObject, vv);
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                float3x3 normalMat = UNITY_MATRIX_M; //Built -in model matrix!
                o.normal = mul(normalMat, v.normal);
               
                return o;
            }

            float MixedAttenuation(float distance) {
                float distanceSqr = distance * distance;
                // switch to linear 80% along the range!
                float falloffStart = lightRange * 0.8f;

                float linearAtten = 1.0f - saturate(
                    (distance - falloffStart) / (lightRange - falloffStart));

                return saturate((lightRange / (lightRange + distanceSqr)) * linearAtten);

            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 textureColour;
                //blending and applying textures
                if (i.height > 7) {
                    textureColour = tex2D(_MountainTexture, i.uv);
                }
                else if (i.height <= 7 && i.height > 6) {
                    textureColour = lerp(tex2D(_MainTex, i.uv), tex2D(_MountainTexture, i.uv), i.height - 6);
                }
                else if(i.height <= 2 && i.height > 1){
                    textureColour = lerp(tex2D(_WaterTexture, i.uv), tex2D(_MainTex, i.uv), i.height - 1);
                }
                else if (i.height < 1) {
                    textureColour = tex2D(_WaterTexture, i.uv);
                }
                else {
                    textureColour = tex2D(_MainTex, i.uv);
                }
                
                float distance = length(lightPosition - i.worldPos);
                float3 incident = normalize(lightPosition - i.worldPos);
                float3 normal = normalize(i.normal);

                float diffuseAmount = saturate(dot(incident, normal));
                float4 diffuseColour = lightColour * lightIntensity * diffuseAmount;

                float3 camVec = normalize(camPosition - i.worldPos);
                float3 halfAngle = normalize(incident + camVec);
                float specAmount = saturate(dot(halfAngle, normal));
                specAmount = pow(specAmount, specularPower);

                float4 specularColour = lightColour * lightIntensity * specAmount;

                float attenuation = MixedAttenuation(distance);
                return ((diffuseColour * textureColour) + specularColour) * attenuation;
                
            }
            ENDCG
        }
    }
}
