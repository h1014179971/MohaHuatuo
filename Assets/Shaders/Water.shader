// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Water"
{
	Properties
	{
		_Power("Power", Range( 0 , 1)) = 1
		_Speed("Speed", Vector) = (0.05,0,0,0)
		_Scale("Scale", Float) = 4.56
		_Strength("Strength", Range( 0 , 3)) = 0.4488235
		_BaseColor("BaseColor", Color) = (0.06452473,0.4528064,0.5471698,0)
		_DepthColor("DepthColor", Color) = (0.03114989,0.9433962,0.9069305,0)
		_ColorLerp("ColorLerp", Float) = 2.85
		_OpLerp("OpLerp", Float) = 0.1
		_Distortion("Distortion", Range( 0 , 0.1)) = 0
		_Brightness("Brightness", Range( -1 , 0)) = 0
		[HDR]_EmissionColor("EmissionColor", Color) = (1,0,0,0)
		_Tiling("Tiling", Vector) = (1,2,0,0)
		_Angle("Angle", Float) = 0
		_Fresnel("Fresnel", Vector) = (-0.1,1,0.5,0)
		_FresnelColor("FresnelColor", Color) = (0.7772522,0,1,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float4 screenPos;
			float2 uv_texcoord;
		};

		uniform float3 _Fresnel;
		uniform float4 _DepthColor;
		uniform float4 _BaseColor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _ColorLerp;
		uniform float4 _FresnelColor;
		uniform float4 _EmissionColor;
		uniform float2 _Tiling;
		uniform float2 _Speed;
		uniform float _Scale;
		uniform float _Angle;
		uniform float _Distortion;
		uniform float _Power;
		uniform float _Strength;
		uniform float _Brightness;
		uniform float _OpLerp;


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


		float2 voronoihash14( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi14( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash14( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			
			 		}
			 	}
			}
			return F1;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV132 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode132 = ( _Fresnel.x + _Fresnel.y * pow( 1.0 - fresnelNdotV132, _Fresnel.z ) );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth50 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth50 = abs( ( screenDepth50 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _ColorLerp ) );
			float clampResult49 = clamp( distanceDepth50 , 0.0 , 1.0 );
			float4 lerpResult27 = lerp( _DepthColor , _BaseColor , clampResult49);
			float4 temp_output_134_0 = ( fresnelNode132 * lerpResult27 );
			float4 Color107 = ( temp_output_134_0 + ( ( 1.0 - temp_output_134_0 ).g * _FresnelColor ) );
			o.Albedo = Color107.rgb;
			float2 uv_TexCoord9 = i.uv_texcoord * _Tiling + ( _Speed * _Time.y );
			float simplePerlin2D120 = snoise( uv_TexCoord9*5.0 );
			simplePerlin2D120 = simplePerlin2D120*0.5 + 0.5;
			float mulTime15 = _Time.y * _Angle;
			float time14 = mulTime15;
			float2 voronoiSmoothId0 = 0;
			float simplePerlin2D12 = snoise( i.uv_texcoord*5.0 );
			simplePerlin2D12 = simplePerlin2D12*0.5 + 0.5;
			float2 coords14 = ( uv_TexCoord9 + ( simplePerlin2D12 * _Distortion ) ) * _Scale;
			float2 id14 = 0;
			float2 uv14 = 0;
			float voroi14 = voronoi14( coords14, time14, id14, uv14, 0, voronoiSmoothId0 );
			float temp_output_17_0 = pow( voroi14 , _Power );
			float clampResult21 = clamp( ( temp_output_17_0 * _Strength ) , 0.0 , 1.0 );
			o.Emission = ( _EmissionColor * ( step( simplePerlin2D120 , 0.7 ) * ( ( 1.0 - step( clampResult21 , 0.07282352 ) ) + _Brightness ) ) ).rgb;
			float screenDepth31 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth31 = abs( ( screenDepth31 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _OpLerp ) );
			float clampResult34 = clamp( distanceDepth31 , 0.0 , 1.0 );
			o.Alpha = ( 0.5 * clampResult34 );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				float3 worldNormal : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18912
412;50;1164;673;-1168.396;892.7628;1;True;True
Node;AmplifyShaderEditor.Vector2Node;6;-1289.171,-975.9045;Inherit;False;Property;_Speed;Speed;1;0;Create;True;0;0;0;False;0;False;0.05,0;0.01,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-1361.171,-642.9045;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;8;-1291.171,-793.9045;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;106;433.7537,-1782.791;Inherit;False;2131.262;705.7775;Color;15;145;144;107;139;136;132;134;30;50;27;26;135;49;25;146;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1110.171,-875.9045;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-1188.89,-499.0959;Inherit;False;Property;_Distortion;Distortion;8;0;Create;True;0;0;0;False;0;False;0;0.015;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;12;-1108.171,-632.9045;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;129;-1081.794,-1079.422;Inherit;False;Property;_Tiling;Tiling;13;0;Create;True;0;0;0;False;0;False;1,2;1,2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;131;-802.6333,-801.7391;Inherit;False;Property;_Angle;Angle;14;0;Create;True;0;0;0;False;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-865.1713,-612.9045;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;524.2838,-1358.144;Inherit;False;Property;_ColorLerp;ColorLerp;6;0;Create;True;0;0;0;False;0;False;2.85;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-885.1713,-1048.905;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-631.1713,-494.9045;Inherit;False;Property;_Scale;Scale;2;0;Create;True;0;0;0;False;0;False;4.56;70;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-603.1713,-742.9045;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;15;-607.1713,-844.9045;Inherit;False;1;0;FLOAT;1.79;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;50;726.7183,-1359.565;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-465.1713,-513.9046;Inherit;False;Property;_Power;Power;0;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;135;715.3154,-1742.837;Inherit;False;Property;_Fresnel;Fresnel;15;0;Create;True;0;0;0;False;0;False;-0.1,1,0.5;-0.1,1,0.5;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;25;982.5983,-1541.071;Inherit;False;Property;_BaseColor;BaseColor;4;0;Create;True;0;0;0;False;0;False;0.06452473,0.4528064,0.5471698,0;0,1,0.8542311,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;49;1049.575,-1365.72;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;14;-429.1713,-820.9045;Inherit;True;0;0;1;0;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.ColorNode;26;979.3995,-1709.791;Inherit;False;Property;_DepthColor;DepthColor;5;0;Create;True;0;0;0;False;0;False;0.03114989,0.9433962,0.9069305,0;0,0.02742356,0.2735849,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;27;1313.96,-1522.383;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;17;-179.1713,-654.9045;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;132;1250.316,-1737.837;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0.43;False;2;FLOAT;0.54;False;3;FLOAT;1.56;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-219.1713,-878.9045;Inherit;False;Property;_Strength;Strength;3;0;Create;True;0;0;0;False;0;False;0.4488235;0.3;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;1500.316,-1619.837;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;95.8288,-810.9045;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;144;1713.315,-1735.837;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;21;299.6637,-994.1406;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;130;230.3668,-707.7391;Inherit;False;Constant;_Float0;Float 0;15;0;Create;True;0;0;0;False;0;False;0.07282352;0;0;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;136;1738.315,-1370.837;Inherit;False;Property;_FresnelColor;FresnelColor;16;0;Create;True;0;0;0;False;0;False;0.7772522,0,1,0;0.246,0,0.811,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;145;1873.315,-1621.837;Inherit;True;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.StepOpNode;105;578.9196,-775.016;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;-0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1649.553,125.9705;Inherit;False;Property;_OpLerp;OpLerp;7;0;Create;True;0;0;0;False;0;False;0.1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;139;1994.315,-1348.837;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;119;710.3075,-507.5798;Inherit;False;Property;_Brightness;Brightness;11;0;Create;True;0;0;0;False;0;False;0;0;-1;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;116;842.3075,-674.5798;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;120;676.4963,-988.8679;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;31;-1455.553,-10.02954;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;123;1057.876,-856.1953;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.7;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;1016.302,-530.9731;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;-1.08;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;146;2178.474,-1479.806;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;122;1275.269,-597.0475;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;107;2365.09,-1469.746;Inherit;False;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;34;-973.5533,39.97051;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;458.1444,265.4546;Inherit;False;Constant;_WaterOp;WaterOp;10;0;Create;True;0;0;0;False;0;False;0.5;0.68;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;127;1298.206,-774.422;Inherit;False;Property;_EmissionColor;EmissionColor;12;1;[HDR];Create;True;0;0;0;False;0;False;1,0,0,0;0.5754716,2,1.398574,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CameraDepthFade;109;-2080.621,-2070.565;Inherit;False;3;2;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;23;278.6637,-492.1406;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DdyOpNode;79;512.2084,-2072.326;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;24;510.6637,-443.1406;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DdxOpNode;78;510.2084,-2157.327;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;85;1202.208,-2192.327;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;87;1635.421,-900.2505;Inherit;False;85;Normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComputeScreenPosHlpNode;89;-1993.509,-2526.226;Inherit;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;128;1544.206,-652.422;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;108;1684.371,-779.6434;Inherit;False;107;Color;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ZBufferParams;93;-2277.509,-2328.226;Inherit;True;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;96;-1987.509,-2281.226;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldPosInputsNode;77;247.1834,-2158.354;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;110;-1719.621,-2034.565;Inherit;True;Property;_TextureSample0;Texture Sample 0;10;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;44;844.8961,-103.9474;Inherit;False;2;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-281.9148,-141.4416;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;81;884.2084,-2109.326;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;35;-768.5533,-45.02952;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-1151.553,231.9705;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;36;-485.2726,1.763836;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CrossProductOpNode;80;655.2084,-2141.327;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-45.24474,-194.1798;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;40;267.4255,-208.4145;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;91;-1730.509,-2497.226;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;43;120.1533,96.68085;Inherit;False;Constant;_WaveOp;WaveOp;8;0;Create;True;0;0;0;False;0;False;0;0.65;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LinearDepthNode;94;-1737.509,-2276.226;Inherit;False;0;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;82;1014.208,-2187.327;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-174.9976,-377.7151;Inherit;False;Constant;_WaterRE;WaterRE;0;0;Create;True;0;0;0;False;0;False;2.85;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;853.7278,167.3842;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;88;669.3842,-1978.249;Inherit;False;Property;_Offset;Offset;9;0;Create;True;0;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;134.6637,-451.1406;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;502.7692,-13.4585;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;45;1831.468,-634.7703;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;True;0;False;Opaque;;Transparent;ForwardOnly;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;0;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;0;6;0
WireConnection;7;1;8;0
WireConnection;12;0;13;0
WireConnection;11;0;12;0
WireConnection;11;1;51;0
WireConnection;9;0;129;0
WireConnection;9;1;7;0
WireConnection;10;0;9;0
WireConnection;10;1;11;0
WireConnection;15;0;131;0
WireConnection;50;0;30;0
WireConnection;49;0;50;0
WireConnection;14;0;10;0
WireConnection;14;1;15;0
WireConnection;14;2;16;0
WireConnection;27;0;26;0
WireConnection;27;1;25;0
WireConnection;27;2;49;0
WireConnection;17;0;14;0
WireConnection;17;1;18;0
WireConnection;132;1;135;1
WireConnection;132;2;135;2
WireConnection;132;3;135;3
WireConnection;134;0;132;0
WireConnection;134;1;27;0
WireConnection;19;0;17;0
WireConnection;19;1;20;0
WireConnection;144;0;134;0
WireConnection;21;0;19;0
WireConnection;145;0;144;0
WireConnection;105;0;21;0
WireConnection;105;1;130;0
WireConnection;139;0;145;1
WireConnection;139;1;136;0
WireConnection;116;0;105;0
WireConnection;120;0;9;0
WireConnection;31;0;33;0
WireConnection;123;0;120;0
WireConnection;28;0;116;0
WireConnection;28;1;119;0
WireConnection;146;0;134;0
WireConnection;146;1;139;0
WireConnection;122;0;123;0
WireConnection;122;1;28;0
WireConnection;107;0;146;0
WireConnection;34;0;31;0
WireConnection;23;0;22;0
WireConnection;79;0;77;0
WireConnection;24;0;23;0
WireConnection;78;0;77;0
WireConnection;85;0;82;0
WireConnection;128;0;127;0
WireConnection;128;1;122;0
WireConnection;96;1;93;1
WireConnection;110;1;109;0
WireConnection;44;0;120;0
WireConnection;44;1;42;0
WireConnection;37;0;36;0
WireConnection;81;0;80;0
WireConnection;81;1;88;0
WireConnection;35;0;34;0
WireConnection;32;0;33;0
WireConnection;36;0;35;0
WireConnection;36;1;32;0
WireConnection;80;0;78;0
WireConnection;80;1;79;0
WireConnection;38;0;37;0
WireConnection;40;0;38;0
WireConnection;91;0;89;0
WireConnection;82;0;81;0
WireConnection;46;0;47;0
WireConnection;46;1;34;0
WireConnection;22;0;17;0
WireConnection;22;1;29;0
WireConnection;42;0;40;0
WireConnection;42;1;36;0
WireConnection;42;2;43;0
WireConnection;45;0;108;0
WireConnection;45;2;128;0
WireConnection;45;9;46;0
ASEEND*/
//CHKSM=7DAB87276C910A2F07DC1AB69E882AF19DBD532D