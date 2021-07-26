using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

public class ForwardMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities
            //.WithAll<MovementSpeedData>()
            .ForEach((ref Translation translation, ref Rotation rotation, in MovementSpeedData moveSpeed) =>
            {
                translation.Value += moveSpeed.Value * deltaTime * math.forward(rotation.Value);
            }
        ).ScheduleParallel();
    }
}
