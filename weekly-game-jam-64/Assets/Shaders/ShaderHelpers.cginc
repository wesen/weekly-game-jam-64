float rand3(float3 co)
{
    return frac(sin(dot(co.xyz ,float3(12.9898,78.233,45.5432) )) * 43758.5453);
}
            
float rand2(float2 co)
{
    return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
}
            
float rand(float co)
{
    return frac(sin(co * 12.9898) * 43758.5453);
}

fixed4 glitch(sampler2D tex, float2 uv, float displacement, int tears, float tearsDistance) 
{
    float _ix = floor(uv.y * tears);
    uv.x += rand(_ix) * tearsDistance;

    fixed4 col = tex2D(tex, uv);

    uv += float2(displacement, 0);
    fixed4 ocol = tex2D(tex, uv);
    col.a = max(ocol.a, col.a);
    col.g = ocol.g;

    uv += float2(displacement, 0);
    ocol = tex2D(tex, uv);
    col.a = max(ocol.a, col.a);
    col.b = ocol.b;

    return col;

}

