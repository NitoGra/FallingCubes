using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

internal class Bomb: MonoBehaviour, ICounted, ISpawnable
{   
    private const int MinDisappearRangeInSeconds = 2;
    private const int MaxDisappearRangeInSeconds = 5;
    
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _explodingRadius;
    [SerializeField] private float _explodingForce;
    
    public Action DecreaseCount { get; set; }
    public Action<Vector3> Released { get; set; }

    private void OnDisable()
    {
        Released?.Invoke(transform.position);
        DecreaseCount?.Invoke();
    }
    
    public void Spawn(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        _renderer.material.color = Color.black;
        StartCoroutine(Disappear());
    }
    
    private IEnumerator Disappear()
    {
        float delay = Random.Range(MinDisappearRangeInSeconds, MaxDisappearRangeInSeconds);
        float elapsed = 0f;
        
        while (elapsed < delay && enabled)
        {
            elapsed += Time.deltaTime;
            _renderer.material.color = ChangeAlphaColor(_renderer.material.color, Mathf.Lerp(1f, 0f, elapsed / delay));
            yield return null;
        }

        ExplodeInRadius();
    }
    
    private void ExplodeInRadius()
    {   
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explodingRadius);

        foreach (var hitCollider in colliders)
            hitCollider.attachedRigidbody?.AddExplosionForce(_explodingForce, transform.position, _explodingRadius, 1, ForceMode.Impulse);
    
        gameObject.SetActive(false);
    }

    private Color ChangeAlphaColor(Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
}