using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs.LowLevel.Unsafe;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class RandomSystem : SystemBase
{
    private const uint Seed = 1;

    public NativeArray<Random> RandomArray { get; private set; }

    protected override void OnCreate()
    {
        Random[] rngArray = new Random[JobsUtility.MaxJobThreadCount];

        Random seed = new Random(Seed);

        for (int i = 0; i < JobsUtility.MaxJobThreadCount; i++)
        {
            rngArray[i] = new Random(seed.NextUInt());
        }

        RandomArray = new NativeArray<Random>(rngArray, Allocator.Persistent);
    }

    protected override void OnDestroy()
    {
        RandomArray.Dispose();
    }

    protected override void OnUpdate()
    {

    }
}
