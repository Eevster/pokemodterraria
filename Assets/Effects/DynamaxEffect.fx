sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float2 uTargetPosition;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;
float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;

// This is a shader. You are on your own with shaders. Compile shaders in an XNB project.

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);

    if (!any(color))
		return color;

    float4 colorAux = color;

    float2 pixelSize = float2(1.0/uImageSize0.x, 1.0/uImageSize0.y);

    if(color.a >= 0){
        float colorAlpha = 0;

        float4 colorL = tex2D(uImage0, float2(coords.x - pixelSize.x,coords.y));
        if(colorL.a <= 0){
            colorAlpha = 1.0;
        }else{
            float4 colorL2 = tex2D(uImage0, float2(coords.x - 2.0*pixelSize.x,coords.y));
            if(colorL2.a <= 0){
                if(colorAlpha < 0.5) colorAlpha = 0.5;
            }
        }

        float4 colorR = tex2D(uImage0, float2(coords.x + pixelSize.x,coords.y));
        if(colorR.a <= 0){
            colorAlpha = 1.0;
        }else{
            float4 colorR2 = tex2D(uImage0, float2(coords.x + 2.0*pixelSize.x,coords.y));
            if(colorR2.a <= 0){
                if(colorAlpha < 0.5) colorAlpha = 0.5;
            }
        }

        /*float4 colorU = tex2D(uImage0, float2(coords.x,coords.y - pixelSize.y));
        if(colorU.a <= 0){
            colorAlpha = 1.0;
        }else{
            float4 colorU2 = tex2D(uImage0, float2(coords.x,coords.y - 2.0*pixelSize.y));
            if(colorU2.a <= 0){
                if(colorAlpha < 0.5) colorAlpha = 0.5;
            }
        }

        
        float4 colorD = tex2D(uImage0, float2(coords.x,coords.y + pixelSize.y));
        if(colorD.a <= 0){
            colorAlpha = 1.0;
        }else{
            float4 colorD2 = tex2D(uImage0, float2(coords.x,coords.y + 2.0*pixelSize.y));
            if(colorD2.a <= 0){
                if(colorAlpha < 0.5) colorAlpha = 0.5;
            }
        }*/

        if(colorAlpha > 0){
            //color.r = (255.0-10.0*colorAlpha)/255;
            //color.g = (255.0-216.0*colorAlpha)/255;
            //color.b = (255.0-127.0*colorAlpha)/255;

            color.r = (245.0*colorAlpha)/255;
            color.g = (39.0*colorAlpha)/255;
            color.b = (128.0*colorAlpha)/255;

            color = colorAux + color;
            color.a = colorAux.a;
        }
    }

	return color * sampleColor;
}
    
technique Technique1
{
    pass DynamaxDyePass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}