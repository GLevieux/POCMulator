Shader "Unlit/POCSky"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorDay("Color Day", Color) = (0,0,1,1)
        _DayLerp("Day Lerp", Range(0,1)) = 0.2
          _SunDir("Sun Dir", Vector) = (1,2,0.5)
          _SunSize("Sun Size", Range(0,0.999999)) = 0.2
    
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off

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
                float4 wPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _ColorDay;
            float _DayLerp;
            float _SunSize;
            float3 _SunDir;

            float2 random(float2 p)
            {
              return frac(sin(float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)))) * 43758.5453);
            }

            float rand(float2 n) {
              return frac(sin(dot(n, float2(12.9898, 4.1414))) * 43758.5453);
            }

            float noise(float2 p, float scale) {
              p *= scale;
              float2 ip = floor(p);
              float2 u = frac(p);
              u = u * u * (3.0 - 2.0 * u);

              float res = lerp(
                lerp(rand(ip), rand(ip + float2(1.0, 0.0)), u.x),
                lerp(rand(ip + float2(0.0, 1.0)), rand(ip + float2(1.0, 1.0)), u.x), u.y);
              return res * res;
            }

            float WorleyNoise(float2 uv, float scale)
            {
              // Tile the space (scale increases count of cells)
              float2 i_st = floor(uv * scale);
              float2 f_st = frac(uv * scale);
              float dist;
              float minDist = 1.0;
              for (float y = -1; y <= 1; y++)
              {
                for (float x = -1; x <= 1; x++)
                {
                  // Neighbor place in the grid
                  float2 neighbor = float2(x, y);

                  // Distance to the random position from current + neighbor place in the grid
                  dist = length(neighbor + random(i_st + neighbor) - f_st);

                  // Keep the closer distance
                  minDist = min(minDist, dist);
                }
              }
              return minDist;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.wPos = v.vertex;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //Sun
                float3 dir = normalize(i.wPos.xyz);
                float3 sunDir = normalize(_SunDir);

                float3 vSunDir = normalize(UnityObjectToViewPos(sunDir)); 
                

                float dayLerp = _DayLerp; 

                float angle = max(0, dot(sunDir, dir));
                angle = pow(angle, 1000 * pow((1 - _SunSize),3)) * 100;
                angle = 1 - step(angle, 0.9);

                fixed4 sunCol = saturate(fixed4(1, 1, 1, 1) * angle);
                
                
                float red = WorleyNoise(i.uv, 40)/2; // Always the same origin and result
                float blue = WorleyNoise(i.uv*float2(0.2,0.1), 45) / 3  ; // Always the same origin and result

                int nbLev = 3;
                
                float density[3] = {8,100,200};
                float sizes[3] = { 0.998,0.98,0.98 };
                float power[3] = { 3,0.3,0.1 };
                float perlins[3] = { 10,15,8 };
                fixed4 colStars = fixed4(0, 0, 0, 1);
                for (int id = 0; id < nbLev; id++) {
                  float minDist = WorleyNoise(i.uv, density[id]); // Always the same origin and result
                  
                  fixed4 colStar = fixed4(1 + red-blue, 1-red-blue, 1-red + blue, 1) * power[id] * step(sizes[id], 1.0 - saturate(minDist));
                  colStar *= (noise(i.uv, perlins[id]) + noise(i.uv, perlins[id]*20))/2;
                  
                  colStars += colStar;

                }

                fixed4 col = lerp(colStars, _ColorDay, dayLerp); 



                
                
                sunCol += col;

                col = lerp(col, sunCol, saturate(1 - (dayLerp * 0.6)));

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
