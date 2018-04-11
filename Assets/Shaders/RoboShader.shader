// RoboShader
// Built from a simple diffuse shader with an outline

Shader "Robo/RoboShader" {

	Properties {

		[NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_OutlineThickness ("Outline Thickness", Range(0,0.1)) = 0.02

	}

	SubShader {

		// outline pass
		Pass {

			Cull Front // turn inside out so it wont render in front of diffuse
			
			CGPROGRAM
            #pragma vertex vert
			#pragma fragment frag
						
			struct appdata
            {
                float2 uv : TEXCOORD0;
				float4 vertex : POSITION;
            };

			struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

			sampler2D _MainTex;
			fixed4 _OutlineColor;
			float _OutlineThickness;

			// vertex shader
			v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex*(1+_OutlineThickness)); // scale the vertex
                o.uv = v.uv;
                return o;
            }

			// fragment shader
			fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
				col *= _OutlineColor;
                return col;
            }
			ENDCG
		}

		// diffuse pass
		Pass {

            Tags {"LightMode"="ForwardBase"}
        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc" // for UnityObjectToWorldNormal
            #include "UnityLightingCommon.cginc" // for _LightColor0

			sampler2D _MainTex;
			fixed4 _Color;

            struct v2f {
                float2 uv : TEXCOORD0;
                fixed4 diff : COLOR0;
                float4 vertex : SV_POSITION;
            };

			// vertex shader
            v2f vert (appdata_base v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                // get vertex normal in world space
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                // dot product between normal and light direction for
                // standard diffuse (Lambert) lighting
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                // factor in the light color
                o.diff = nl * _LightColor0;
                return o;
            }

			// fragment shader
            fixed4 frag (v2f i) : SV_Target {
                // sample texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // multiply by lighting
                col *= i.diff;
				// multiply by _Color
				col *= _Color;
                return col;
            }
            ENDCG
        }

	}

}