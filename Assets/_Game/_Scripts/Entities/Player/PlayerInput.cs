using System;
using UnityEngine;

namespace SunnySword.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; }
        public event Action OnAttackPressed;
        public event Action OnInteractPressed;

        public bool IsShieldHeld { get; private set; }

        private void Update()
        {
            MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (Input.GetButtonDown("Fire1")) OnAttackPressed?.Invoke();

            IsShieldHeld = Input.GetButton("Fire2");
        }
    }
}
