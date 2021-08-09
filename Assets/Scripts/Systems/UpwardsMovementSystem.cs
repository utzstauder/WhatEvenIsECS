using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class UpwardsMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities
            .WithNone<PlayerControlledTag>()
            .ForEach((ref Translation translation, in Rotation rotation, in UpSpeedData upSpeed) =>
            {
                translation.Value += upSpeed.Value * deltaTime * math.up();
            }
            ).WithName("Movement_NoPlayer")
            .ScheduleParallel();
    }
}
