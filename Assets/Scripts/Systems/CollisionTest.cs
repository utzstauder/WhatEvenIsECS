using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

[UpdateAfter(typeof(PhysicsCollisionSystem))]
public class CollisionTest : SystemBase
{
    private EndSimulationEntityCommandBufferSystem ecbSystem;

    protected override void OnCreate()
    {
        base.OnCreate();

        ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var ecb = ecbSystem.CreateCommandBuffer().ToConcurrent();
        
        Entities
            .WithNone<PlayerControlledTag>()
            .ForEach((Entity entity, int entityInQueryIndex, DynamicBuffer<CollisionBuffer> collisionBuffer) =>
            {
                if (collisionBuffer.Length > 0)
                {
                    ecb.DestroyEntity(entityInQueryIndex, entity);
                }
            }
            ).WithName("CollisionTest_DestroyOnCollision")
            .ScheduleParallel();

        ecbSystem.AddJobHandleForProducer(Dependency);
    }
}
