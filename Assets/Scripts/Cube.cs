using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class Cube : MonoBehaviour, IConvertGameObjectToEntity
{
    public float Speed;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent<CubeComponent>(entity);
        dstManager.AddComponentData(entity, new MoveSpeedComponentData { Value = Speed });
        dstManager.AddComponentData(entity, new MyOwnColor { Value = new Unity.Mathematics.float4(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 1f) });
    }
}
