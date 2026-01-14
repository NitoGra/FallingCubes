using UnityEngine;

namespace Scripts
{
    public class Floor : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out ICube cube))
                cube.CollideToFloor();
        }
    }
}