using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour, IInitializable, IColorable
{
    [SerializeField] private T _obj;

    private ObjectPool<T> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<T>(
        createFunc: Create,
        actionOnGet: ReceiveObject,
        actionOnRelease: (GameObject) => GameObject.gameObject.SetActive(false));
    }

    public virtual void ReceiveObject(T obj)
    {
        obj.gameObject.SetActive(true);
        obj.SetStartColor();
    }

    public virtual T Create()
    {
        T obj = Instantiate(_obj);
        obj.Initialize(this, Vector3.zero);
        return obj;
    }

    public virtual void ReturnItem(T obj)
    {
        _pool.Release(obj);
    }
}
