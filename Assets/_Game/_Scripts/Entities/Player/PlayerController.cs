using SunnySword.Animation;
using SunnySword.Player;
using SunnySword.Stats;
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
        private StatsHandler statsHandler;
        private CharacterMovement movement;
        private PlayerCombat combat;
        private PlayerShield shield;
        private bool isNextAttack = false;

        private Vector2 lastDirection = Vector2.down;

        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            movement = GetComponent<CharacterMovement>();
            spriteAnimator = GetComponent<SpriteAnimator>();
            combat = GetComponent<PlayerCombat>();
            shield = GetComponent<PlayerShield>();
            statsHandler = GetComponent<StatsHandler>();

            input.OnAttackPressed += HandleAttack;
        }

        private void FixedUpdate()
        {
            bool canShield = input.IsShieldHeld && statsHandler.CurrentStamina > 0;
            shield.SetBlocking(canShield);

            statsHandler.IsBlocking = canShield;

            if (shield.IsBlocking)
            {
                statsHandler.CanRegenerateStamina = false; 
                statsHandler.ConsumeBlockStamina();        
            }
            else
            {
                statsHandler.CanRegenerateStamina = true;  
            }

            if (combat.IsAttacking || shield.IsBlocking)
            {
                movement.ProcessMove(Vector2.zero);
                if (!combat.IsAttacking) HandleAnimation();
                return;
            }

            movement.ProcessMove(input.MoveInput);
            HandleAnimation();
        }

        private void OnDestroy() => input.OnAttackPressed -= HandleAttack;

        private void HandleAttack()
        {
            if (combat.IsAttacking) return;

            Sprite[] selectedAttack = isNextAttack ? animData.secondAttackSprite : animData.firstAttackSprite;

            isNextAttack = !isNextAttack;

            float animDuration = spriteAnimator.PlayAnimation(selectedAttack, lastFlipX);
            combat.PerformAttack(lastDirection, animDuration);
        }

        private void HandleAnimation()
        {
            if (combat.IsAttacking) return;

            if (shield.IsBlocking)
            {
         
                if (input.MoveInput.x != 0)
                {
                    lastFlipX = (input.MoveInput.x < 0);
                }

                spriteAnimator.PlayAnimation(animData.defenseSprite, lastFlipX);
                return;
            }

            Vector2 move = input.MoveInput;
            if (move.sqrMagnitude > 0.01f)
            {
                lastDirection = move;
                lastFlipX = (move.x < 0);
                spriteAnimator.PlayAnimation(animData.walkSprites, lastFlipX);
            }
            else
            {
                spriteAnimator.PlayAnimation(animData.idleSprites, lastFlipX);
            }
        }
    }
}