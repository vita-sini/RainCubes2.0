using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour, IInitializable, IColorable
{
    private float _lifeTime = 5f;
    private SpawnerCube _spawner;
    private Rigidbody _rigidbody;
    private Renderer _renderer;

    private bool _colorChanged = false;
    public bool _isDeactivated = false;

    private Vector3 _lastPosition;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    public void Initialize(MonoBehaviour spawner, Vector3 zero)
    {
        SetVelocity(zero);
        SetStartColor();
        _spawner = (SpawnerCube)spawner;
    }

    public void SetColor()
    {
        if (_colorChanged == false)
        {
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            _renderer.material.color = randomColor;
            _colorChanged = true;
        }
    }

    public void SetStartColor()
    {
        _renderer.material.color = Color.white;
    }

    public void SetVelocity(Vector3 velocity)
    {
        _rigidbody.velocity = velocity;
    }

    public void StartCountdown()
    {
        Invoke(nameof(Deactivate), _lifeTime);
    }

    public void Deactivate()
    {
        if (!_isDeactivated)
        {
            _colorChanged = false;
            _spawner.ReturnItem(this);
            _isDeactivated = true;
        }
    }
}
