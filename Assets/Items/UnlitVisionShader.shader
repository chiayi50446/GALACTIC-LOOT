Shader "Unlit/UnlitVisionShader"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 0, 1) // 預設黃色
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" }
        Pass
        {
            ZTest Always
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha
            Lighting Off
            Color[_Color]
        }
    }
}
