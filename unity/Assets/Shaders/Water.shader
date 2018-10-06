// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Water"
{
	Properties
	{
		_WaterColor("Water Color", Color) = (0,0,0,0)
		_WaterMetallic("Water Metallic", Range( 0 , 1)) = 0
		_WaterSmoothness("Water Smoothness", Range( 0 , 1)) = 0
		_FoamColor("Foam Color", Color) = (0,0,0,0)
		_FoamIntensity("Foam Intensity", Range( 0.01 , 10)) = 0
		_Flow("Flow", 2D) = "white" {}
		_FlowIntensity("Flow Intensity", Range( 0 , 1)) = 0
		_Normal("Normal", 2D) = "bump" {}
		_NormalIntensity("Normal Intensity", Float) = 0
		_WaterSpeed1("Water Speed 1", Float) = 1
		_WaterDirection1("Water Direction 1", Vector) = (0,0,0,0)
		_WaterSpeed2("Water Speed 2", Float) = 0.05
		_WaterDirection2("Water Direction 2", Vector) = (0.05,0.05,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform sampler2D _Normal;
		uniform float _NormalIntensity;
		uniform float _WaterSpeed1;
		uniform float2 _WaterDirection1;
		uniform float4 _Normal_ST;
		uniform float _WaterSpeed2;
		uniform float2 _WaterDirection2;
		uniform float4 _WaterColor;
		uniform float _FlowIntensity;
		uniform sampler2D _Flow;
		uniform float4 _FoamColor;
		uniform sampler2D _CameraDepthTexture;
		uniform float _FoamIntensity;
		uniform float _WaterMetallic;
		uniform float _WaterSmoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime58 = _Time.y * _WaterSpeed1;
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float2 panner54 = ( uv_Normal + mulTime58 * _WaterDirection1);
			float mulTime67 = _Time.y * _WaterSpeed2;
			float2 panner69 = ( uv_Normal + mulTime67 * _WaterDirection2);
			o.Normal = BlendNormals( UnpackScaleNormal( tex2D( _Normal, panner54 ) ,_NormalIntensity ) , UnpackScaleNormal( tex2D( _Normal, panner69 ) ,_NormalIntensity ) );
			float4 tex2DNode80 = tex2D( _Flow, panner54 );
			float4 tex2DNode79 = tex2D( _Flow, panner69 );
			float4 blendOpSrc88 = tex2DNode80;
			float4 blendOpDest88 = tex2DNode79;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth42 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth42 = abs( ( screenDepth42 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _FoamIntensity ) );
			float clampResult44 = clamp( ( 1.0 - distanceDepth42 ) , 0.0 , 1.0 );
			float4 lerpResult23 = lerp( ( _WaterColor + ( _FlowIntensity * ( saturate( ( 1.0 - ( 1.0 - blendOpSrc88 ) * ( 1.0 - blendOpDest88 ) ) )) ) ) , _FoamColor , clampResult44);
			o.Albedo = lerpResult23.rgb;
			o.Metallic = _WaterMetallic;
			o.Smoothness = _WaterSmoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14001
2088;57;1718;922;2857.975;1835.091;2.498677;True;True
Node;AmplifyShaderEditor.RangedFloatNode;56;-1972.142,-1186.426;Float;False;Property;_WaterSpeed1;Water Speed 1;9;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-1932.932,-817.3591;Float;False;Property;_WaterSpeed2;Water Speed 2;11;0;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;59;-1795.342,-1311.926;Float;False;Property;_WaterDirection1;Water Direction 1;10;0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;70;-1796.433,-1015.659;Float;False;Property;_WaterDirection2;Water Direction 2;12;0;0.05,0.05;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;67;-1580.934,-819.959;Float;False;1;0;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;58;-1573.343,-1192.926;Float;False;1;0;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;55;-1618.343,-1425.926;Float;False;0;62;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;69;-1330.934,-960.9591;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.5,1;False;1;FLOAT;0.25;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;54;-1323.343,-1333.926;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.5,1;False;1;FLOAT;0.25;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;78;-1211.5,-364.5995;Float;True;Property;_Flow;Flow;5;0;None;False;white;Auto;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-1059.41,327.9672;Float;False;Property;_FoamIntensity;Foam Intensity;4;0;0;0.01;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;79;-828.9666,-246.8971;Float;True;Property;_TextureSample2;Texture Sample 2;10;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;80;-831.2297,-482.3015;Float;True;Property;_TextureSample3;Texture Sample 3;10;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;88;-215.9675,-611.9111;Float;False;Screen;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DepthFade;42;-780.1938,332.3046;Float;False;True;1;0;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-711.8906,-652.3787;Float;False;Property;_FlowIntensity;Flow Intensity;6;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-1339.641,-1148.549;Float;False;Property;_NormalIntensity;Normal Intensity;8;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;43;-559.1935,331.3046;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-494.4642,-83.06365;Float;False;Property;_WaterColor;Water Color;0;0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;-342.8911,-464.3788;Float;False;2;2;0;FLOAT;0.0;False;1;COLOR;0.0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;62;-1439.016,-1671.299;Float;True;Property;_Normal;Normal;7;0;None;False;bump;Auto;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.ColorNode;14;-496.4319,136.3364;Float;False;Property;_FoamColor;Foam Color;3;0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;64;-1037.321,-1457.22;Float;True;Property;_TextureSample0;Texture Sample 0;11;0;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;65;-1045.021,-1156.821;Float;True;Property;_TextureSample1;Texture Sample 1;12;0;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;44;-387.1932,330.3046;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;81;-150.2959,-228.9444;Float;False;2;2;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;83;-498.74,-341.5648;Float;False;2;2;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-164.0408,338.6866;Float;False;Property;_WaterSmoothness;Water Smoothness;2;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;73;-681.2051,-1239.124;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;23;-122.3266,48.81489;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-171.6868,229.9723;Float;False;Property;_WaterMetallic;Water Metallic;1;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1195.13,15.94905;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Custom/Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;67;0;66;0
WireConnection;58;0;56;0
WireConnection;69;0;55;0
WireConnection;69;2;70;0
WireConnection;69;1;67;0
WireConnection;54;0;55;0
WireConnection;54;2;59;0
WireConnection;54;1;58;0
WireConnection;79;0;78;0
WireConnection;79;1;69;0
WireConnection;80;0;78;0
WireConnection;80;1;54;0
WireConnection;88;0;80;0
WireConnection;88;1;79;0
WireConnection;42;0;45;0
WireConnection;43;0;42;0
WireConnection;87;0;85;0
WireConnection;87;1;88;0
WireConnection;64;0;62;0
WireConnection;64;1;54;0
WireConnection;64;5;52;0
WireConnection;65;0;62;0
WireConnection;65;1;69;0
WireConnection;65;5;52;0
WireConnection;44;0;43;0
WireConnection;81;0;1;0
WireConnection;81;1;87;0
WireConnection;83;0;80;0
WireConnection;83;1;79;0
WireConnection;73;0;64;0
WireConnection;73;1;65;0
WireConnection;23;0;81;0
WireConnection;23;1;14;0
WireConnection;23;2;44;0
WireConnection;0;0;23;0
WireConnection;0;1;73;0
WireConnection;0;3;50;0
WireConnection;0;4;49;0
ASEEND*/
//CHKSM=E2A9EB31C59DC1815E0E37AAB81E4786CCB4042E