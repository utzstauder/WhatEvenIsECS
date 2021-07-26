using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

public class RotationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        
        Entities
            .WithNone<PlayerControlledTag>()
            .ForEach((ref Rotation rotation, in RotationSpeedData rotationSpeed) =>
            {
                rotation.Value = math.mul(rotation.Value, quaternion.RotateY(deltaTime * math.radians(rotationSpeed.Value)));
            }
        ).ScheduleParallel();


        float horizontalInput = UnityEngine.Input.GetAxis("Horizontal");

        Entities
            .WithAll<PlayerControlledTag>()
            .ForEach((ref Rotation rotation, in RotationSpeedData rotationSpeed) =>
            {
                rotation.Value = math.mul(rotation.Value, quaternion.RotateY(horizontalInput * deltaTime * math.radians(rotationSpeed.Value)));
            }
        ).ScheduleParallel();
    }
}
