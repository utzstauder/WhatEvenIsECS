using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

[MaterialProperty("_MyColor", MaterialPropertyFormat.Float4)]
[GenerateAuthoringComponent]
public struct ColorData : IComponentData
{
    public float4 Value;
}
