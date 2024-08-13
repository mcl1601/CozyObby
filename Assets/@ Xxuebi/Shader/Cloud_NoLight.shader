// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "@Xxuebi/Cloud_NoLight"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 5
		_Cloud_01_Scale("Cloud_01_Scale", Float) = 5
		_Cloud_01_High("Cloud_01_High", Float) = 1
		_Cloud_01_Remap("Cloud_01_Remap", Range( -1 , 0)) = -1
		_Cloud_01_Speed("Cloud_01_Speed", Float) = 0
		_Cloud_02_Scale("Cloud_02_Scale", Float) = 5
		_Cloud_02_High("Cloud_02_High", Float) = 1
		_Cloud_02_Remap("Cloud_02_Remap", Range( -1 , 0)) = -1
		_Cloud_02_Speed("Cloud_02_Speed", Float) = 0
		_Cloud_03_Scale("Cloud_03_Scale", Float) = 5
		_Cloud_03_High("Cloud_03_High", Float) = 1
		_Cloud_03_Remap("Cloud_03_Remap", Range( -1 , 0)) = -1
		_Cloud_03_Speed("Cloud_03_Speed", Float) = 0
		_Cloud_04_Scale("Cloud_04_Scale", Float) = 5
		_Cloud_04_High("Cloud_04_High", Float) = 1
		_Cloud_04_Remap("Cloud_04_Remap", Range( -1 , 0)) = -1
		_Cloud_04_Speed("Cloud_04_Speed", Float) = 0
		_Depth_Fade("Depth_Fade", Float) = 1
		[HDR]_Color_A("Color_A", Color) = (0,0,0,0)
		[HDR]_Color_B("Color_B", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf StandardCustomLighting keepalpha noshadow vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float _Cloud_01_Speed;
		uniform float _Cloud_01_Scale;
		uniform float _Cloud_01_Remap;
		uniform float _Cloud_01_High;
		uniform float _Cloud_02_Speed;
		uniform float _Cloud_02_Scale;
		uniform float _Cloud_02_Remap;
		uniform float _Cloud_02_High;
		uniform float _Cloud_03_Speed;
		uniform float _Cloud_03_Scale;
		uniform float _Cloud_03_Remap;
		uniform float _Cloud_03_High;
		uniform float _Cloud_04_Speed;
		uniform float _Cloud_04_Scale;
		uniform float _Cloud_04_Remap;
		uniform float _Cloud_04_High;
		uniform float4 _Color_A;
		uniform float4 _Color_B;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Depth_Fade;
		uniform float _EdgeLength;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float mulTime9 = _Time.y * 0.1;
			float temp_output_10_0 = ( _Cloud_01_Speed * mulTime9 );
			float2 temp_cast_0 = (temp_output_10_0).xx;
			float2 uv_TexCoord2 = v.texcoord.xy + temp_cast_0;
			float simplePerlin2D1 = snoise( uv_TexCoord2*_Cloud_01_Scale );
			simplePerlin2D1 = simplePerlin2D1*0.5 + 0.5;
			float2 temp_cast_1 = (( temp_output_10_0 * 0.5 )).xx;
			float2 uv_TexCoord29 = v.texcoord.xy + temp_cast_1;
			float simplePerlin2D28 = snoise( uv_TexCoord29*_Cloud_01_Scale );
			simplePerlin2D28 = simplePerlin2D28*0.5 + 0.5;
			float temp_output_64_0 = abs( (_Cloud_01_Remap + (( ( simplePerlin2D1 + simplePerlin2D28 ) / 2.0 ) - 0.0) * (1.0 - _Cloud_01_Remap) / (1.0 - 0.0)) );
			float temp_output_17_0 = ( mulTime9 * _Cloud_02_Speed );
			float2 temp_cast_2 = (temp_output_17_0).xx;
			float2 uv_TexCoord18 = v.texcoord.xy + temp_cast_2;
			float simplePerlin2D20 = snoise( uv_TexCoord18*_Cloud_02_Scale );
			simplePerlin2D20 = simplePerlin2D20*0.5 + 0.5;
			float2 temp_cast_3 = (( temp_output_17_0 * 0.5 )).xx;
			float2 uv_TexCoord37 = v.texcoord.xy + temp_cast_3;
			float simplePerlin2D38 = snoise( uv_TexCoord37*_Cloud_02_Scale );
			simplePerlin2D38 = simplePerlin2D38*0.5 + 0.5;
			float temp_output_46_0 = ( ( mulTime9 * 0.1 ) * _Cloud_03_Speed );
			float2 temp_cast_4 = (temp_output_46_0).xx;
			float2 uv_TexCoord50 = v.texcoord.xy + temp_cast_4;
			float simplePerlin2D52 = snoise( uv_TexCoord50*_Cloud_03_Scale );
			simplePerlin2D52 = simplePerlin2D52*0.5 + 0.5;
			float2 temp_cast_5 = (( temp_output_46_0 * 0.5 )).xx;
			float2 uv_TexCoord49 = v.texcoord.xy + temp_cast_5;
			float simplePerlin2D51 = snoise( uv_TexCoord49*_Cloud_03_Scale );
			simplePerlin2D51 = simplePerlin2D51*0.5 + 0.5;
			float temp_output_67_0 = ( mulTime9 * _Cloud_04_Speed );
			float2 temp_cast_6 = (temp_output_67_0).xx;
			float2 uv_TexCoord68 = v.texcoord.xy + temp_cast_6;
			float simplePerlin2D70 = snoise( uv_TexCoord68*_Cloud_04_Scale );
			simplePerlin2D70 = simplePerlin2D70*0.5 + 0.5;
			float2 temp_cast_7 = (( temp_output_67_0 * 0.5 )).xx;
			float2 uv_TexCoord77 = v.texcoord.xy + temp_cast_7;
			float simplePerlin2D78 = snoise( uv_TexCoord77*_Cloud_04_Scale );
			simplePerlin2D78 = simplePerlin2D78*0.5 + 0.5;
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( ( ( temp_output_64_0 * _Cloud_01_High ) + ( abs( (_Cloud_02_Remap + (( ( simplePerlin2D20 + simplePerlin2D38 ) / 2.0 ) - 0.0) * (1.0 - _Cloud_02_Remap) / (1.0 - 0.0)) ) * _Cloud_02_High ) + ( abs( (_Cloud_03_Remap + (( ( simplePerlin2D52 + simplePerlin2D51 ) / 2.0 ) - 0.0) * (1.0 - _Cloud_03_Remap) / (1.0 - 0.0)) ) * _Cloud_03_High ) + ( abs( (_Cloud_04_Remap + (( ( simplePerlin2D70 + simplePerlin2D78 ) / 2.0 ) - 0.0) * (1.0 - _Cloud_04_Remap) / (1.0 - 0.0)) ) * _Cloud_04_High ) ) / 4.0 ) * ase_vertexNormal );
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth33 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth33 = abs( ( screenDepth33 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Depth_Fade ) );
			c.rgb = 0;
			c.a = saturate( distanceDepth33 );
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			float mulTime9 = _Time.y * 0.1;
			float temp_output_10_0 = ( _Cloud_01_Speed * mulTime9 );
			float2 temp_cast_0 = (temp_output_10_0).xx;
			float2 uv_TexCoord2 = i.uv_texcoord + temp_cast_0;
			float simplePerlin2D1 = snoise( uv_TexCoord2*_Cloud_01_Scale );
			simplePerlin2D1 = simplePerlin2D1*0.5 + 0.5;
			float2 temp_cast_1 = (( temp_output_10_0 * 0.5 )).xx;
			float2 uv_TexCoord29 = i.uv_texcoord + temp_cast_1;
			float simplePerlin2D28 = snoise( uv_TexCoord29*_Cloud_01_Scale );
			simplePerlin2D28 = simplePerlin2D28*0.5 + 0.5;
			float temp_output_64_0 = abs( (_Cloud_01_Remap + (( ( simplePerlin2D1 + simplePerlin2D28 ) / 2.0 ) - 0.0) * (1.0 - _Cloud_01_Remap) / (1.0 - 0.0)) );
			float4 lerpResult6 = lerp( _Color_A , _Color_B , temp_output_64_0);
			o.Emission = lerpResult6.rgb;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
-103;240;1357;742;2937.04;723.3721;3.344106;True;True
Node;AmplifyShaderEditor.SimpleTimeNode;9;-3644.749,380.8059;Inherit;False;1;0;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-3122.601,-154.8688;Inherit;False;Property;_Cloud_01_Speed;Cloud_01_Speed;9;0;Create;True;0;0;False;0;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;60;-3154.75,29.66475;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-3101.369,1636.242;Inherit;False;Property;_Cloud_04_Speed;Cloud_04_Speed;21;0;Create;True;0;0;False;0;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;61;-3287.359,791.5584;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;81;-3358.11,1227.547;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-2907.714,-146.6351;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-2838.594,1580.002;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-3093.907,900.5049;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-3043.404,370.4293;Inherit;False;Property;_Cloud_02_Speed;Cloud_02_Speed;13;0;Create;True;0;0;False;0;0;0.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-3086.539,1079.048;Inherit;False;Property;_Cloud_03_Speed;Cloud_03_Speed;17;0;Create;True;0;0;False;0;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-2663.323,33.72205;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-2826.001,974.7687;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-2831.865,343.1494;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;-2677.823,1905.303;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-2540.347,-91.47507;Inherit;False;Property;_Cloud_01_Scale;Cloud_01_Scale;6;0;Create;True;0;0;False;0;5;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-2527.167,-231.8504;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-2524.145,21.72162;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;68;-2532.35,1562.054;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-2671.095,668.4502;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-2539.584,1723.879;Inherit;False;Property;_Cloud_04_Scale;Cloud_04_Scale;18;0;Create;True;0;0;False;0;5;300;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;77;-2543.162,1883.606;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-2665.23,1300.069;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;1;-2218.981,-228.5956;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;70;-2239.148,1586.953;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;28;-2219.41,18.53522;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;37;-2536.434,646.7531;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;48;-2525.39,1118.645;Inherit;False;Property;_Cloud_03_Scale;Cloud_03_Scale;14;0;Create;True;0;0;False;0;5;50;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;78;-2249.96,1908.506;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-2531.255,487.026;Inherit;False;Property;_Cloud_02_Scale;Cloud_02_Scale;10;0;Create;True;0;0;False;0;5;15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;49;-2530.569,1278.372;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-2525.622,325.2007;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;50;-2519.757,956.8201;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;38;-2243.232,671.6531;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;43;-1911.955,-71.60588;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;71;-1952.615,1740.442;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;51;-2237.367,1303.272;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;20;-2232.421,350.1007;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;52;-2226.556,981.72;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;41;-1945.887,503.5892;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-1940.022,1135.208;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;44;-1785.779,-74.28712;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-1871.193,1883.865;Inherit;False;Property;_Cloud_04_Remap;Cloud_04_Remap;20;0;Create;True;0;0;False;0;-1;0;-1;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;72;-1826.906,1739.045;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-1923.638,153.4551;Inherit;False;Property;_Cloud_01_Remap;Cloud_01_Remap;8;0;Create;True;0;0;False;0;-1;0;-1;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;55;-1814.313,1133.812;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;63;-1607.384,-81.35738;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-1838.42,1267.803;Inherit;False;Property;_Cloud_03_Remap;Cloud_03_Remap;16;0;Create;True;0;0;False;0;-1;-0.5;-1;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-1864.465,647.0121;Inherit;False;Property;_Cloud_02_Remap;Cloud_02_Remap;12;0;Create;True;0;0;False;0;-1;-1;-1;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;73;-1655.991,1610.563;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;42;-1820.178,502.1926;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-1513.547,1886.3;Inherit;False;Property;_Cloud_04_High;Cloud_04_High;19;0;Create;True;0;0;False;0;1;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;25;-1649.263,373.7101;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-1427.467,130.4501;Inherit;False;Property;_Cloud_01_High;Cloud_01_High;7;0;Create;True;0;0;False;0;1;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;64;-1442.567,-88.51882;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;56;-1643.398,1005.329;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;74;-1469.311,1605.223;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;57;-1456.719,999.9899;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-1232.8,1655.414;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;27;-1462.583,368.3705;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-1165.68,-7.660308;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-1506.819,651.0469;Inherit;False;Property;_Cloud_02_High;Cloud_02_High;11;0;Create;True;0;0;False;0;1;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-1500.954,1282.666;Inherit;False;Property;_Cloud_03_High;Cloud_03_High;15;0;Create;True;0;0;False;0;1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;83;-953.0746,1256.439;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-1220.208,1050.18;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-1226.073,418.5609;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;36;-1068.832,132.435;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;16;-961.4509,321.327;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-961.1797,462.6573;Inherit;False;Constant;_Divide;Divide;5;0;Create;True;0;0;False;0;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-400.1862,276.9109;Inherit;False;Property;_Depth_Fade;Depth_Fade;22;0;Create;True;0;0;False;0;1;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;14;-747.1135,342.9206;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;33;-234.9256,266.0451;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;8;-322.7855,35.8365;Inherit;False;Property;_Color_B;Color_B;24;1;[HDR];Create;True;0;0;False;0;0,0,0,0;1.046,1.046,1.046,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;5;-435.3455,471.1623;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;7;-328.0851,-158.2632;Inherit;False;Property;_Color_A;Color_A;23;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.7231293,0.8785685,0.9191176,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-238.7525,363.4362;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;34;7.700761,266.9626;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;6;-11.19762,125.4099;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;195.0251,95.11949;Float;False;True;-1;6;ASEMaterialInspector;0;0;CustomLighting;@Xxuebi/Cloud_NoLight;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;1;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;False;Custom;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;5;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;60;0;9;0
WireConnection;61;0;9;0
WireConnection;81;0;9;0
WireConnection;10;0;11;0
WireConnection;10;1;60;0
WireConnection;67;0;81;0
WireConnection;67;1;66;0
WireConnection;62;0;61;0
WireConnection;30;0;10;0
WireConnection;46;0;62;0
WireConnection;46;1;45;0
WireConnection;17;0;9;0
WireConnection;17;1;21;0
WireConnection;76;0;67;0
WireConnection;2;1;10;0
WireConnection;29;1;30;0
WireConnection;68;1;67;0
WireConnection;39;0;17;0
WireConnection;77;1;76;0
WireConnection;47;0;46;0
WireConnection;1;0;2;0
WireConnection;1;1;3;0
WireConnection;70;0;68;0
WireConnection;70;1;69;0
WireConnection;28;0;29;0
WireConnection;28;1;3;0
WireConnection;37;1;39;0
WireConnection;78;0;77;0
WireConnection;78;1;69;0
WireConnection;49;1;47;0
WireConnection;18;1;17;0
WireConnection;50;1;46;0
WireConnection;38;0;37;0
WireConnection;38;1;19;0
WireConnection;43;0;1;0
WireConnection;43;1;28;0
WireConnection;71;0;70;0
WireConnection;71;1;78;0
WireConnection;51;0;49;0
WireConnection;51;1;48;0
WireConnection;20;0;18;0
WireConnection;20;1;19;0
WireConnection;52;0;50;0
WireConnection;52;1;48;0
WireConnection;41;0;20;0
WireConnection;41;1;38;0
WireConnection;53;0;52;0
WireConnection;53;1;51;0
WireConnection;44;0;43;0
WireConnection;72;0;71;0
WireConnection;55;0;53;0
WireConnection;63;0;44;0
WireConnection;63;3;65;0
WireConnection;73;0;72;0
WireConnection;73;3;79;0
WireConnection;42;0;41;0
WireConnection;25;0;42;0
WireConnection;25;3;26;0
WireConnection;64;0;63;0
WireConnection;56;0;55;0
WireConnection;56;3;54;0
WireConnection;74;0;73;0
WireConnection;57;0;56;0
WireConnection;75;0;74;0
WireConnection;75;1;80;0
WireConnection;27;0;25;0
WireConnection;12;0;64;0
WireConnection;12;1;13;0
WireConnection;83;0;75;0
WireConnection;59;0;57;0
WireConnection;59;1;58;0
WireConnection;23;0;27;0
WireConnection;23;1;24;0
WireConnection;36;0;12;0
WireConnection;16;0;36;0
WireConnection;16;1;23;0
WireConnection;16;2;59;0
WireConnection;16;3;83;0
WireConnection;14;0;16;0
WireConnection;14;1;15;0
WireConnection;33;0;35;0
WireConnection;4;0;14;0
WireConnection;4;1;5;0
WireConnection;34;0;33;0
WireConnection;6;0;7;0
WireConnection;6;1;8;0
WireConnection;6;2;64;0
WireConnection;0;2;6;0
WireConnection;0;9;34;0
WireConnection;0;11;4;0
ASEEND*/
//CHKSM=951C4DFD3F35520B47077AD0C673BF17E9F55C4C