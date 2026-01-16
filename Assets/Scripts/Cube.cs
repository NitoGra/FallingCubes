using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cube : MonoBehaviour, ICube, ICounted, ISpawnable
{
    private const int MinDisappearRangeInSeconds = 2;
    private const int MaxDisappearRangeInSeconds = 5;
    
    [SerializeField] private float _spawnPositionX;
    [SerializeField] private float _spawnPositionZ;
    [SerializeField] private float _spawnPositionY;
    
    [SerializeField] private Renderer _renderer;
    private bool _isDisappeared = false;
    private Func<MonoBehaviour> _spawnBomb;
    
    private Vector3 SpawnPosition => new (Random.Range(-_spawnPositionX, _spawnPositionX), _spawnPositionY, Random.Range(-_spawnPositionZ, _spawnPositionZ));
    public Action DecreaseCount { get; set; }
    public Action<Vector3> Released { get; set; }

    private void OnDisable()
    {
        Released?.Invoke(transform.position);
        DecreaseCount?.Invoke();
    }

    public void Spawn(Vector3 position = default)
    {
        transform.position = SpawnPosition;
        _renderer.material.color = Color.white;
        gameObject.SetActive(true);
    }
    
    public void CollideToFloor()
    {
        if (_isDisappeared)
            return;

        _isDisappeared = true;
        ChangeColor();
        Invoke(nameof(Disappear), Random.Range(MinDisappearRangeInSeconds, MaxDisappearRangeInSeconds));
    }

    private void Disappear()
    {
        gameObject.SetActive(false);
        _isDisappeared = false;
    }
    
    private void ChangeColor()
    {
        _renderer.material.color = Random.ColorHSV();
    }
}