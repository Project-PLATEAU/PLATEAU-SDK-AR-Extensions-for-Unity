Shader "Custom/ZWrite"
{
    Properties
    {
        _MainTex("Base (RGB)",2D) = "white" {}
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        LOD 200

        Pass {
            ZWrite On
            ColorMask 0
        }


    }
}