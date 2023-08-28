Shader "CustomShaders/HealthBar"
{
    Properties
    {
        [Space(10)]
        [KeywordEnum(Circle,Box, Rhombus)] _shape("Shape",Float) = 0
        _healthNormalized("Health Normalized", Range(0,1)) = 0.0
        _lowHealthThreshold("Low Health Threshold", Range(0,1)) = 0.2
        _fillColor("Fill Start Color", Color) = (0,0,0,0)

        [Space(10)]
        _waveAmp("Fill Wave Amplitude", float) = 0.01
        _waveFreq("Fill Wave Frequency", float) = 8
        _waveSpeed("Fill Wave Speed", float) = 0.5

        [Space(10)]
        _backgroundColor("Background Color", Color) = (0,0,0,0.25)
        _borderWidth("Border Width", Range(0,0.4)) = 0
        _borderColor("Border Color", Color) = (0.1,0.1,0.1,1)
        
        [Space(10)]
        _segmentCount("Number of Segments", int) = 10
        _segmentSpacing("Spacing Between Segments", Range(0, 0.1)) = 0.01
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            HLSLPROGRAM

            #pragma shader_feature _SHAPE_CIRCLE _SHAPE_BOX _SHAPE_RHOMBUS

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "MyFunctions.hlsl"
            
            CBUFFER_START(UnityPerMaterial)
            float _healthNormalized;
            float4 _fillColor, _backgroundColor, _borderColor;
            float _lowHealthThreshold, _borderWidth;
            float _waveAmp, _waveFreq, _waveSpeed;
            int _segmentCount;
            float _segmentSpacing;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float4 positionOS : TEXCOORD1;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.positionOS = IN.positionOS;
                return OUT;
            }
            float4 frag(Varyings IN) : SV_Target
            {
                float3 _objectScale = GetObjectScale();

                float minScale = min(_objectScale.x, _objectScale.y);
                float margin = minScale * 0.1;
                float3 _shapeElongation = (_objectScale - minScale)/2;
                float3 p = (IN.positionOS) * _objectScale;
                float3 q = Elongate(p, _shapeElongation);

                float halfSize = minScale/2 - margin;

                #if _SHAPE_CIRCLE
                float healthBarSDF = CircleSDF(q, halfSize);
                #endif

                #if _SHAPE_BOX
                float healthBarSDF = BoxSDF(q, halfSize);
                #endif

                #if _SHAPE_RHOMBUS
                float healthBarSDF = RhombusSDF(q, float2(halfSize, halfSize));
                #endif

                float healthBarMask = GetSmoothMask(healthBarSDF);
    
                // Border Mask
                float borderSDF = healthBarSDF + _borderWidth * _objectScale.y;
                float borderMask =  1 - GetSmoothMask(borderSDF);

                // Segmented Bars Logic inside health bar (excluding border)
                float segmentWidth = 1.0f / _segmentCount;
                float segmentPos = fmod(IN.uv.y, segmentWidth + _segmentSpacing);
                float segmentMask = segmentPos > segmentWidth ? 0.0f : 1.0f; 

                float waveOffset = _waveAmp * cos(_waveFreq * (IN.uv.x + _Time.y * _waveSpeed)) * min(1.3f * sin(PI * _healthNormalized), 1);
                float marginNormalizedY = margin / _objectScale.y;
                float borderNormalizedY = _borderWidth;
                float fillOffset = marginNormalizedY + borderNormalizedY;
                float healthMapped = lerp(fillOffset - 0.01f, 1 - fillOffset, _healthNormalized);
                float fillSDF = IN.uv.y - healthMapped + waveOffset;

                float fillMask = GetSmoothMask(fillSDF) * step(IN.uv.y, _healthNormalized) * segmentMask;

                // Combine everything
                float4 outColor = healthBarMask * (fillMask * (1 - borderMask) * _fillColor + (1 - fillMask) * (1 - borderMask) * _backgroundColor + borderMask * _borderColor);

                outColor *= float4(2 - healthBarSDF/(minScale/2).xxx, 1);

                if (_healthNormalized < _lowHealthThreshold) 
                {
                    float flash = 0.1*cos(6*_Time.y) + 0.1;
                    outColor.xyz += flash;
                } 

                return outColor;
            }
            ENDHLSL
        }
    }
}