using UnityEngine;
using Random = UnityEngine.Random;

public class Cube : MonoBehaviour, ICube
{
    private const int MinDisappearRangeInSeconds = 2;
    private const int MaxDisappearRangeInSeconds = 5;
    
    [SerializeField] private Renderer _renderer;
    private bool _isDisappeared = false;
    
    private void OnEnable()
    {
        _renderer.material.color = Color.white;
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