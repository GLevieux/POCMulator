Shader "Unlit/POCStyle2"
{
    Properties
    {
        _BaseMap("Texture", 2D) = "white" {}
        _Ramp("Ramp", 2D) = "white" {}
        _NbHue("Nb Hue", Int) = 100
        _NbSat("Nb Sat", Int) = 100
        _NbVal("Nb Val", Int) = 3
        _ScreenStep("Screen Step", Int) = 20
        _ScreenStepDepth("Screen Step Depth", Range(0, 1)) = 0.1
        _ScreenStepScaleFactor("Screen Scale Factor", Range(0, 300)) = 30
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
          Cull off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
           
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 colOut : TEXCOORD4;
                float4 objPos : TEXCOORD5;
            };

            sampler2D _BaseMap;
            float4 _BaseMap_ST;
            int _NbHue;
            int _NbSat;
            int _NbVal;
            int _ScreenStep;
            float _ScreenStepDepth;
            float _ScreenStepScaleFactor;

            float3 RGBToHSV(float3 c)
            {
              float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
              float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
              float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));
              float d = q.x - min(q.w, q.y);
              float e = 1.0e-10;
              return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }

            float3 HSVToRGB(float3 c)
            {
              float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
              float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
              return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
            }

            float stepVal(float val, int nbStep) {
              val *= nbStep;
              val = round(val);
              val = (val / nbStep);
              return val;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.objPos = v.vertex; 
                float norm = _ScreenStepScaleFactor + o.vertex.w* _ScreenStepDepth;

                float4 wPos = mul(unity_ObjectToWorld, v.vertex);
                float distToCam = length(mul(UNITY_MATRIX_V, wPos));
                
                //ON veut pas modif ceux qui sont pret, quand on marche
                float distToCamNorm = clamp(distToCam / 100, 0, 1);


                o.colOut = float4(abs(o.vertex.z/norm), 0, 0, 1);

                o.vertex.x = lerp(o.vertex.x, stepVal(o.vertex.x / norm, _ScreenStep) * norm, distToCamNorm);
                o.vertex.y = lerp(o.vertex.y, stepVal(o.vertex.y/ norm, _ScreenStep ) * norm, distToCamNorm);
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                /*float4 col = tex2Dlod(_BaseMap, float4(v.uv,0,0));

                col.xyz = RGBToHSV(col.xyz);

                col.x = stepVal(col.x, _NbHue);
                col.y = stepVal(col.y, _NbSat);
                col.z = stepVal(col.z, _NbVal);

                o.colOut.xyz = HSVToRGB(col);
                o.colOut.w = 1;*/

   

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 wPos = mul(unity_ObjectToWorld, i.objPos); 

                float4 wPosStep = float4(
                  stepVal(wPos.x / 5000, 5) * 5000,
                  stepVal(wPos.y / 5000, 5) * 5000,
                  stepVal(wPos.z / 5000, 5) * 5000,
                  1);

                float distToCam = length(mul(UNITY_MATRIX_V, wPosStep));
                //Calc dist

                if (distToCam > 4000 && wPos.y > 10)
                  discard;
                   

                // sample the texture
                //fixed4 col = i.colOut;

                float4 col = tex2D(_BaseMap, i.uv);

                col.xyz = RGBToHSV(col.xyz);

                col.x = stepVal(col.x, _NbHue);
                col.y = stepVal(col.y, _NbSat);
                col.z = stepVal(col.z, _NbVal);

                col.xyz = HSVToRGB(col);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
