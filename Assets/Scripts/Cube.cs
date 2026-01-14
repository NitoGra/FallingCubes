using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts
{
    public class Cube : MonoBehaviour, ICube, IPoolItem
    {
        private const int MinDisappearRangeInSeconds = 2;
        private const int MaxDisappearRangeInSeconds = 5;

        [SerializeField] private Renderer _renderer;
        private bool _isDisappeared = false;
        
        private Action<MonoBehaviour> _returnToPool ;
        
        public void Init(Action<MonoBehaviour> action)
        {
            _returnToPool = action;
        }
        
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
            _returnToPool.Invoke(this);
            _isDisappeared = false;
        }

        private void ChangeColor()
        {
            _renderer.material.color = Random.ColorHSV();
        }
    }
}