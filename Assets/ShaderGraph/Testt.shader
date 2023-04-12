Shader "Custom/TransparentOpaqueShader" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _IsTransparent("Is Transparent", Range(0, 1)) = 0
    }

        SubShader{
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 100

            Pass {
                Blend SrcAlpha OneMinusSrcAlpha // 使用透明混合模式
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float _IsTransparent;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    // 根据 _IsTransparent 属性判断是否透明渲染
                    if (_IsTransparent > 0.5) {
                        // 透明渲染
                        fixed4 col = tex2D(_MainTex, i.uv);
                        col.a = 0.5; // 设置透明度为 0.5
                        return col;
                    }
     else {
                        // 不透明渲染
                        return tex2D(_MainTex, i.uv);
                    }
                }
                ENDCG
            }
        }

            SubShader{
                Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
                LOD 100

                Pass {
                    Blend One One // 使用不透明混合模式
                    CGPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag
                    #include "UnityCG.cginc"

                    struct appdata {
                        float4 vertex : POSITION;
                        float2 uv : TEXCOORD0;
                    };

                    struct v2f {
                        float2 uv : TEXCOORD0;
                        float4 vertex : SV_POSITION;
                    };

                    sampler2D _MainTex;
                    float _IsTransparent;

                    v2f vert(appdata v) {
                        v2f o;
                        o.vertex = UnityObjectToClipPos(v.vertex);
                        o.uv = v.uv;
                        return o;
                    }

                    fixed4 frag(v2f i) : SV_Target {
                        // 根据 _IsTransparent 属性判断是否透明渲染
                        if (_IsTransparent > 0.5) {
                            // 透明渲染
                            fixed4 col = tex2D(_MainTex, i.uv);
                            col.a = 0.5; // 设置透明度为 0.5
                            return col;
                        }
         else {
                            // 不透明渲染
                            return tex2D(_MainTex, i.uv);
                        }
                    }
                    ENDCG
                }
                }
}
