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
            .WithNone<PlayerControlledTag>()
            .ForEach((ref Translation translation, ref Rotation rotation, in MovementSpeedData moveSpeed) =>
            {
                translation.Value += moveSpeed.Value * deltaTime * math.forward(rotation.Value);
            }
            ).WithName("Movement_NoPlayer")
            .ScheduleParallel();


        float forwardInput = UnityEngine.Input.GetAxis("Vertical");

        Entities
            .WithAll<PlayerControlledTag>()
            .ForEach((ref Translation translation, ref Rotation rotation, in MovementSpeedData moveSpeed) =>
            {

                translation.Value += forwardInput * moveSpeed.Value * deltaTime * math.forward(rotation.Value);
            }
            ).WithName("Movement_Player")
            .Schedule();
    }
}
