using UnityEngine;

public interface IInitializable
{
    void Initialize(MonoBehaviour spawner, Vector3 position);
}