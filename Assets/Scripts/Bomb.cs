using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
public class Bomb : MonoBehaviour, IInitializable, IColorable
{
    [SerializeField] private ParticleSystem _effect;

    private float _explosionRadius = 40;
    private float _explosionForce = 700;
    private float _lifeTime;

    private Rigidbody _rigidbody;
    private Renderer _renderer;
    private SpawnerBomb _spawner;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    public void Setup()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutAndDetonate());
    }

    public void Initialize(MonoBehaviour spawnerBomb, Vector3 zero)
    {
        SetVelocity(zero);
        SetStartColor();
        _spawner = (SpawnerBomb)spawnerBomb;
        _lifeTime = Random.Range(2f, 5f);
    }

    public void SetStartColor()
    {
        _renderer.material.color = Color.black;
    }

    public void SetVelocity(Vector3 velocity)
    {
        _rigidbody.velocity = velocity;
    }

    private void Detonate()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider hit in hits)
        {
            Rigidbody explodableObject = hit.GetComponent<Rigidbody>();

            if (explodableObject != null)
            {
                explodableObject.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }
        }

        Instantiate(_effect, transform.position, transform.rotation);

        _spawner.ReturnItem(this);
    }

    private IEnumerator FadeOutAndDetonate()
    {
        float elapsedTime = 0;

        while (elapsedTime < _lifeTime)
        {
            _renderer.material.DOFade(0f, _lifeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Detonate();
    }
}
