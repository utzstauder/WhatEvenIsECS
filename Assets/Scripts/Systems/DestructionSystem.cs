using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

public class DestructionSystem : SystemBase
{
    private const float collisionDistance = 2f;

    private EndSimulationEntityCommandBufferSystem ecbSystem;

    protected override void OnCreate()
    {
        base.OnCreate();

        ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var ecb = ecbSystem.CreateCommandBuffer().ToConcurrent();

        var playerEntityArray = GetEntityQuery(typeof(PlayerControlledTag), typeof(Translation)).ToComponentDataArray<Translation>(Unity.Collections.Allocator.TempJob);

        Entities
            .WithNone<PlayerControlledTag>()
            .WithDeallocateOnJobCompletion(playerEntityArray)
            .ForEach((Entity entity, int entityInQueryIndex, ref Translation translationOther) =>
            {
                for (int i = 0; i < playerEntityArray.Length; i++)
                {
                    if (math.distance(playerEntityArray[i].Value, translationOther.Value) <= collisionDistance)
                    {
                        ecb.DestroyEntity(entityInQueryIndex, entity);
                    }
                }

            }
            ).WithName("DestructionSystem_CheckForCollisionWithPlayer")
            .ScheduleParallel();

        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
