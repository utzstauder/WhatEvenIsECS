using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class CubeSpawner : MonoBehaviour
{
    public GameObject Prefab;
    public int NumberOfObjects = 1000;
    public Vector3 Area = new Vector3(100, 100, 100);

    EntityManager entityManager;

    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        var entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(Prefab, settings);

        System.Random random = new System.Random(1);

        for (int i = 0; i < NumberOfObjects; i++)
        {
            Vector3 position = new Vector3(
                    (float)(random.NextDouble() * Area.x * 2 - Area.x),
                    (float)(random.NextDouble() * Area.y * 2 - Area.y),
                    (float)(random.NextDouble() * Area.z * 2 - Area.z)
                );

            var entityInstance = entityManager.Instantiate(entity);

            entityManager.SetComponentData(entityInstance, new Translation { Value = position });
        }
    }
}
