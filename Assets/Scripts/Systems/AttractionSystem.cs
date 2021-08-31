using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

// [UpdateBefore(typeof(EndFramePhysicsSystem))]
// [UpdateAfter(typeof(StepPhysicsWorld))]
public class AttractionSystem : SystemBase
{
    private const float AttractionStrength = 5f;
    private const float MaxDistanceSqrd = 10f;
    
    protected override void OnUpdate()
    {
        var playerPositionArray = GetEntityQuery(
            typeof(PlayerControlledTag),
            typeof(Translation)
        ).ToComponentDataArray<Translation>(Allocator.TempJob);
        
        Entities
            .WithNone<PlayerControlledTag>()
            .WithDeallocateOnJobCompletion(playerPositionArray)
            .ForEach((ref PhysicsVelocity physicsVelocity, in Translation translation) =>
            {
                for (int i = 0; i < playerPositionArray.Length; i++)
                {
                    float3 diff = playerPositionArray[i].Value - translation.Value;
                    float distSqrd = math.lengthsq(diff);
                    if (distSqrd < MaxDistanceSqrd)
                    {
                        physicsVelocity.Linear += AttractionStrength * (diff / math.sqrt(distSqrd));
                    }
                }
            }
            ).WithName("Attraction_NonPlayerCubes")
            .ScheduleParallel();

    }
}
