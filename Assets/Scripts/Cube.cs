using UnityEngine;
using Random = UnityEngine.Random;

public class Cube : MonoBehaviour
{
    private readonly int _disappearDelayStart = 2;
    private readonly int _disappearDelayEnd = 5;
    
    [SerializeField] private Renderer _renderer;
    private bool _isDisappears = false;
    
    private void OnEnable()
    {
        _renderer.material.color = Color.white;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(_isDisappears)
            return;
        
        if(other.gameObject.layer == gameObject.layer)
            return;

        _isDisappears = true;
        ColorChange();
        Invoke(nameof(Disappearance), Random.Range(_disappearDelayStart, _disappearDelayEnd));
    }

    private void Disappearance()
    {
        gameObject.SetActive(false);
        _isDisappears = false;
    }

    private void ColorChange()
    {
        _renderer.material.color = Random.ColorHSV();
    }
}