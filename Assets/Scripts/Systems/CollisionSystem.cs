using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

public class CollisionSystem : SystemBase
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
        var randomArray = World.GetExistingSystem<RandomSystem>().RandomArray;

        var ecb = ecbSystem.CreateCommandBuffer().ToConcurrent();

        var playerPositionArray = GetEntityQuery(
            typeof(PlayerControlledTag),
            typeof(Translation)
            ).ToComponentDataArray<Translation>(Allocator.TempJob);

        Entities
            .WithNone<PlayerControlledTag>()
            .WithDeallocateOnJobCompletion(playerPositionArray)
            .WithNativeDisableParallelForRestriction(randomArray)
            .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, ref Translation translation, ref UpSpeedData upSpeed) =>
            {
                for (int i = 0; i < playerPositionArray.Length; i++)
                {
                    if (math.distance(playerPositionArray[i].Value, translation.Value) <= collisionDistance)
                    {
                        //ecb.DestroyEntity(entityInQueryIndex, entity);

                        // assign random value to UpSpeedData component
                        var random = randomArray[nativeThreadIndex];

                        upSpeed.Value = random.NextFloat();

                        randomArray[nativeThreadIndex] = random;
                    }
                }

            }
            ).WithName("DestructionSystem_CheckForCollisionWithPlayer")
            .ScheduleParallel();

        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
