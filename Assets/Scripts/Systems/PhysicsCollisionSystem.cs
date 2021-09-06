using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class PhysicsCollisionSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem ecbSystem;

    private BuildPhysicsWorld buildPhysicsWorld;
    private ISimulation simulation;

    protected override void OnCreate()
    {
        base.OnCreate();

        ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
    }

    protected override void OnUpdate()
    {
        simulation = World.GetOrCreateSystem<StepPhysicsWorld>().Simulation;

        Entities.ForEach((DynamicBuffer<CollisionBuffer> collisionBuffer) => collisionBuffer.Clear()).Run();

        var collisionJob = new CollisionJob { Collisions = GetBufferFromEntity<CollisionBuffer>() };

        var collisionHandle = collisionJob.Schedule(simulation, ref buildPhysicsWorld.PhysicsWorld, Dependency);
        
        collisionHandle.Complete();
    }

    private struct CollisionJob : ICollisionEventsJob
    {
        public BufferFromEntity<CollisionBuffer> Collisions;
        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.Entities.EntityA;
            Entity entityB = collisionEvent.Entities.EntityB;
            
            if (Collisions.Exists(entityA))
            {
                Collisions[entityA].Add(new CollisionBuffer {Entity = entityB});
            }

            if (Collisions.Exists(entityB))
            {
                Collisions[entityB].Add(new CollisionBuffer {Entity = entityA});
            }
        }
    }
}
