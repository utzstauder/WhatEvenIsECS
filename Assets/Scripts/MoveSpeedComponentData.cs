using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

[System.Serializable]
public struct CubeComponent : IComponentData { }


[System.Serializable]
public struct MoveSpeedComponentData : IComponentData
{
    public float Value;
}

[MaterialProperty("_MyColor", MaterialPropertyFormat.Float4)]
public struct MyOwnColor : IComponentData
{
    public float4 Value;
}
