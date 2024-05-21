using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnerBomb : Spawner<Bomb>
{
    [SerializeField] private Bomb _bomb;
    [SerializeField] private SpawnerCube spawnerCube;
    private ObjectPool<Bomb> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Bomb>(
        createFunc: Create,
        actionOnGet: ReceiveObject,
        actionOnRelease: (bomb) => bomb.gameObject.SetActive(false));
    }

    private void Start()
    {
        spawnerCube.CubeDeactivated += Activation;
    }

    private void Activation(Vector3 position)
    {
        Bomb newBomb = _pool.Get(); 
        newBomb.transform.position = position; 
        ReceiveObject(newBomb); 
    }

    public override void ReturnItem(Bomb bomb)
    {
        base.ReturnItem(bomb);
    }

    public override Bomb Create()
    {
        Bomb bomb = Instantiate(_bomb);
        bomb.Initialize(this, Vector3.zero);
        return bomb;
    }

    public override void ReceiveObject(Bomb bomb)
    {
        bomb.gameObject.SetActive(true);
        bomb.Initialize(this, Vector3.zero);
        bomb.Setup();
        bomb.SetStartColor();
    }
}
