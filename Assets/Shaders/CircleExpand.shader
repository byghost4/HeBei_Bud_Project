Shader "ACME/CircleExpand"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)

        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
        _Expand("Expand", range(0,1)) = 1
        _Spacing("Spacing", range(0.1,0.2)) = 0.15
        _ColorMask("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
    }

        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }

            Stencil
            {
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }

            Cull Off
            Lighting Off
            ZWrite Off
            ZTest[unity_GUIZTestMode]
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask[_ColorMask]

            Pass
            {
                Name "Default"
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0

                #include "UnityCG.cginc"
                #include "UnityUI.cginc"

                #pragma multi_compile __ UNITY_UI_CLIP_RECT
                #pragma multi_compile __ UNITY_UI_ALPHACLIP

                struct appdata_t
                {
                    float4 vertex   : POSITION;
                    float4 color    : COLOR;
                    float2 texcoord : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct v2f
                {
                    float4 vertex   : SV_POSITION;
                    fixed4 color : COLOR;
                    float2 texcoord  : TEXCOORD0;
                    float4 worldPosition : TEXCOORD1;
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                sampler2D _MainTex;
                fixed4 _Color;
                fixed4 _TextureSampleAdd;
                float4 _ClipRect;
                float4 _MainTex_ST;
                float _Expand;
                float _Spacing;


                v2f vert(appdata_t v)
                {
                    v2f OUT;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                    OUT.worldPosition = v.vertex;
                    OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                    OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                    OUT.color = v.color * _Color;
                    return OUT;
                }
                float expand;
                fixed4 frag(v2f IN) : SV_Target
                {
                    //计算 uv点 到 中间点 的向量    (向外扩，反过来就是向里缩)  
                    float2 dv = IN.texcoord - float2(0.5, 0.5);
                    //归一化  
                    float L = length(dv);
                    float2 dv1 = dv / L;


                    expand = frac(_Time.y / 2) * _Expand;

                    float ratio = lerp(0, 1, 1 / expand);
                    float2 uv1 = float2(0.5, 0.5) + dv * ratio;
                    float2 uv2 = uv1 + dv1 * _Spacing;
                    float2 uv3 = uv1 + dv1 * _Spacing * 2;
                    float4 color1 = tex2D(_MainTex, uv1);
                    float4 color2 = tex2D(_MainTex, uv2);
                    float4 color3 = tex2D(_MainTex, uv3);
                    float4 color = color1 + color2 + color3;



                    #ifdef UNITY_UI_CLIP_RECT
                    color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                    #endif

                    #ifdef UNITY_UI_ALPHACLIP
                    clip(color.a - 0.001);
                    #endif


                    color *= (1 - L / 0.707);

                    return color;
                }
            ENDCG
            }
        }
}