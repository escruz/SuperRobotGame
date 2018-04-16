// RoboShader
// Built from a simple diffuse shader with an outline

Shader "Robo/RoboShader" {

	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Tint ("Tint", Color) = (1,1,1,1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_OutlineThickness ("Outline Thickness", Range(0,0.1)) = 0.02
	}

	SubShader {

		// diffuse surface shader
		Tags { "RenderType" = "Opaque" }
		CGPROGRAM
		#pragma surface surf Lambert finalcolor:tintedColor

		struct Input {
			float2 uv_MainTex;
		};
		sampler2D _MainTex;
		float4 _Color;
		float4 _Tint;

		void tintedColor( Input IN, SurfaceOutput o, inout fixed4 color) {
			color *= _Tint;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * _Color;
		}
		ENDCG

		// outline surface shader with custom vertex function
		Lighting Off
		Cull Front // turn inside out so it will be rendered behind diffuse
		ZTest Less

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		struct Input {
			float2 uv_MainTex;
		};
		float _OutlineThickness;
		float4 _OutlineColor;
		void vert (inout appdata_full v) {
			v.vertex.xyz *= (1+_OutlineThickness); // scale
			//v.vertex.xyz += v.normal * _OutlineThickness; // offset
		}
		sampler2D _MainTex;
		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = _OutlineColor;
		}
		ENDCG

	}

	FallBack "Diffuse"
	CustomEditor "RoboShaderGUI"

}