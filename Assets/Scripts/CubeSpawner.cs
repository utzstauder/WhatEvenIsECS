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

        for (int i = 0; i < NumberOfObjects; i++)
        {
            Vector3 position = new Vector3(
                    Random.Range(-Area.x, Area.x),
                    Random.Range(-Area.y, Area.y),
                    Random.Range(-Area.z, Area.z)
                );

            var entityInstance = entityManager.Instantiate(entity);

            entityManager.SetComponentData(entityInstance, new Translation { Value = position });
        }
    }
}
