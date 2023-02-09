Shader "CustomRP/Particles/Unlit"
{
    Properties
    {
        [MainTexture] _BaseMap("Texture", 2D) = "white" {}
        [MainColor] _BaseColor("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        [Toggle(_CLIPPING)] _Clipping ("Alpha Clipping", Float) = 0
        _Cutoff ("Alpha Cutoff", Range(0.0, 1.0)) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", Float) = 0
        [Enum(Off, 0, On, 1)] _ZWrite ("Z Write", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }

        Pass
        {
            Blend [_SrcBlend] [_DstBlend], One OneMinusSrcAlpha
            ZWrite [_ZWrite]
            HLSLPROGRAM
            #pragma shader_feature _CLIPPING
            #pragma shader_feature _FLIPBOOK_BLENDING
            #pragma multi_compile_instancing
            #pragma vertex UnlitPassVertex
			#pragma fragment UnlitPassFragment
            #include "../UnlitPass.hlsl"
            

            ENDHLSL
        }
    }
}
