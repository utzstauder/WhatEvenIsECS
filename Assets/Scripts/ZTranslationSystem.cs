using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

public class ZTranslationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float dT = Time.DeltaTime;

        Entities
            //.WithName("Translate_Z")
            .WithAll<CubeComponent>()
            .ForEach((ref Translation translation, in MoveSpeedComponentData moveSpeed) =>
            {
                translation.Value += new float3(0, 0, moveSpeed.Value * dT);
            }
        ).ScheduleParallel();
    }
}
