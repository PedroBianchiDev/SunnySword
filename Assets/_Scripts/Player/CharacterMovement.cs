using UnityEngine;

namespace SunnySword.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField]
        private float speed = 1;

        private Animator animator;
        private PlayerInput input;
        private new Rigidbody2D rigidbody;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            input = GetComponent<PlayerInput>();
            animator = GetComponent<Animator>();
        }

        public void Move(float horizontalInput, float verticalInput)
        {
            Vector2 direction = new(horizontalInput, verticalInput);
            rigidbody.linearVelocity = direction * speed;
        }

        public void Stop()
        {
            rigidbody.linearVelocity = Vector2.zero;
        }
    }
}
