using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Jobs;
using UnityEngine;

[UpdateBefore(typeof(EndFramePhysicsSystem))]
[UpdateAfter(typeof(StepPhysicsWorld))]
public class PhysicsCollisionSystem : SystemBase
{
    private const float collisionDistance = 2f;

    private EndSimulationEntityCommandBufferSystem ecbSystem;

    private BuildPhysicsWorld buildPhysicsWorld;

    protected override void OnCreate()
    {
        base.OnCreate();

        ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
    }

    protected override void OnUpdate()
    {
        var collisionWorld = buildPhysicsWorld.PhysicsWorld.CollisionWorld;

        Dependency = JobHandle.CombineDependencies(Dependency, buildPhysicsWorld.FinalJobHandle);

        var ecb = ecbSystem.CreateCommandBuffer().ToConcurrent();

        var playerPositionArray = GetEntityQuery(
            typeof(PlayerControlledTag),
            typeof(Translation)
            ).ToComponentDataArray<Translation>(Allocator.TempJob);



        Entities
            .WithNone<PlayerControlledTag>()
            .WithDeallocateOnJobCompletion(playerPositionArray)
            .WithReadOnly(collisionWorld)
            .ForEach((Entity entity, int entityInQueryIndex, int nativeThreadIndex, in Translation translation) =>
            {
                for (int i = 0; i < playerPositionArray.Length; i++)
                {
                    PointDistanceInput input = new PointDistanceInput {
                        Position = translation.Value,
                        MaxDistance = collisionDistance
                    };

                    NativeList<DistanceHit> allHits = new NativeList<DistanceHit>();

                    if (collisionWorld.CalculateDistance(input, ref allHits))
                    {
                        JobLogger.Log($"Hits = {allHits.Capacity}");
                        for (int h = 0; h < allHits.Capacity; h++)
                        {
                            if (HasComponent<PlayerControlledTag>(allHits[h].Entity))
                            {
                                ecb.DestroyEntity(entityInQueryIndex, entity);
                            }
                        }

                    }
                }
            }
            ).WithName("PhysicsCollisionSystem_CheckForCollisionWithPlayer")
            .ScheduleParallel();

        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
