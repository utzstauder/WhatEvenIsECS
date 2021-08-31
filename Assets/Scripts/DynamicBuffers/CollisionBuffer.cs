using Unity.Entities;

[InternalBufferCapacity(8)]
public struct CollisionBuffer : IBufferElementData
{
    public Entity Entity;
}