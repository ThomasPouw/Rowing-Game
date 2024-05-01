Shader "TCCavy/WaveShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { 
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque" 
            "UniversalMaterialType" = "Lit"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "../GetWaveDisplacement.hlsl"

            #define PI 3.14159265358979323846


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal: TEXCOORD1;
                //UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;

            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _CustomTime;

            v2f vert (appdata IN)
            {
                v2f OUT;
                float3 normal;
                float3 pos;
                float y_offset;
                GetWaveDisplacement_float(IN.vertex, _Time.y, pos, normal, y_offset);
                OUT.normal = normal;
                OUT.vertex = UnityObjectToClipPos(pos);
                //o.vertex = pos;
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return OUT;
            }




            //End of Vertex.











            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
