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
                for (int i = 0; i < collisionBuffer.Length; i++)
                {
                    if (collisionBuffer[i].Entity.CompareTo(entity) == 0) continue;
                    if (HasComponent<PlayerControlledTag>(collisionBuffer[i].Entity))
                    {
                        ecb.DestroyEntity(entityInQueryIndex, entity);
                    }
                }
            }
            ).WithName("CollisionTest_DestroyOnCollision")
            .ScheduleParallel();

        ecbSystem.AddJobHandleForProducer(Dependency);
    }
}
