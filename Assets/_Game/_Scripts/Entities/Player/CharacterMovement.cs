using UnityEngine;

namespace SunnySword.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField]
        private float speed = 5;
        private new Rigidbody2D rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        public void ProcessMove(Vector2 direction)
        {
            Vector2 velocity = direction.normalized * speed;
            rigidbody.linearVelocity = velocity;
        }
    }
}
