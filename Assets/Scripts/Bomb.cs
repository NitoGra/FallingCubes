using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

internal class Bomb: MonoBehaviour, ICounted
{   
    private const int MinDisappearRangeInSeconds = 2;
    private const int MaxDisappearRangeInSeconds = 5;
    
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _explodingRadius;
    [SerializeField] private float _explodingForce;
    public Action DecreaseCount { get; set; }
    
    private void OnEnable()
    {
        _renderer.material.color = Color.black;
        StartCoroutine(Disappear());
    }
    
    private void OnDisable() => DecreaseCount?.Invoke();

    private IEnumerator Disappear()
    {
        float delay  = Random.Range(MinDisappearRangeInSeconds, MaxDisappearRangeInSeconds);
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
        var ds = Physics.OverlapSphere(transform.position, _explodingRadius);

        foreach (var hitCollider in ds)
            hitCollider.attachedRigidbody?.AddExplosionForce(_explodingForce, transform.position, _explodingRadius, 1, ForceMode.Impulse);
    
        gameObject.SetActive(false);
    }

    private Color ChangeAlphaColor(Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
}