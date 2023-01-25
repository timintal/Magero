Shader "Unlit/InstancedUnlit"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha 
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing


            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                 float4 color    : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                half4 color    : COLOR;
            };

            float4 _Colors[1023]; 
            

            v2f vert (appdata v, uint instanceID: SV_InstanceID)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.color = half4(0,0,0,0);
                
                #ifdef UNITY_INSTANCING_ENABLED
                    o.color = _Colors[instanceID];
                #endif
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = i.color;
                // apply fog
                return col;
            }
            ENDCG
        }
    }
}
