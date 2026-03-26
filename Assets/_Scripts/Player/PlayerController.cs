using UnityEngine;

namespace SunnySword.Player
{
    [RequireComponent(typeof(PlayerInput), typeof(CharacterMovement))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerInput input;
        private CharacterMovement characterMovement;

        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            characterMovement = GetComponent<CharacterMovement>();
        }

        private void FixedUpdate()
        {
            characterMovement.Move(input.HorizontalInput, input.VerticalInput);
        }
    }
}
