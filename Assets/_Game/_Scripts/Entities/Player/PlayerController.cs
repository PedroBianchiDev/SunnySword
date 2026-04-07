using SunnySword.Animation;
using SunnySword.Player;
using UnityEngine;

namespace SunnySword.Playerr
{
    [RequireComponent(typeof(PlayerInput), typeof(CharacterMovement))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private CharacterAnimationData animData;
        private SpriteAnimator spriteAnimator;
        private bool lastFlipX = false;

        [Header("Input")]
        private PlayerInput input;

        [Header("States")]
        private CharacterMovement movement;
        private Vector2 lastDirection = Vector2.down;

        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            movement = GetComponent<CharacterMovement>();
            spriteAnimator = GetComponent<SpriteAnimator>();
        }

        private void FixedUpdate()
        {
            movement.ProcessMove(input.MoveInput);
            HandleAnimation();
        }

        private void HandleAnimation()
        {
            Vector2 move = input.MoveInput;
            bool isMoving = move.sqrMagnitude > 0.01f;

            if (move.x != 0)
            {
                lastFlipX = (move.x < 0);
            }

            if (isMoving)
            {
                spriteAnimator.PlayAnimation(animData.walkSprites, lastFlipX);
            }
            else
            {
                spriteAnimator.PlayAnimation(animData.idleSprites, lastFlipX);
            }
        }
    }
}