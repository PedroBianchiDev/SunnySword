using UnityEngine;

namespace SunnySword.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public float HorizontalInput { get; private set; }
        public float VerticalInput { get; private set; }
        public bool AttackInput { get; private set; }
        public bool InteractionInput { get; private set; }

        private void Update()
        {
            HorizontalInput = Input.GetAxis("Horizontal");
            VerticalInput = Input.GetAxis("Vertical");

            if (Input.GetButtonDown("Fire1")) AttackInput = true;
            if (Input.GetButtonDown("Fire1")) InteractionInput = true;
        }
    }
}
