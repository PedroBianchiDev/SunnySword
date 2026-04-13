using UnityEngine;
using SunnySword.Animation;
using SunnySword.Abilities;
using SunnySword.Stats;

namespace SunnySword.Player
{
    public class PartyMemberFollower : MonoBehaviour
    {
        [Header("Configuração de Posição")]
        public Transform playerTransform;
        public Vector2 formationOffset;
        public float followSpeed = 5f;
        public float stoppingDistance = 0.5f;

        [Header("Combate")]
        public AttackAbilityData combatAbility;
        public float attackCooldown = 2f;
        public LayerMask enemyLayer;
        private float nextAttackTime;

        private bool isAttacking;

        [Header("Visual")]
        public CharacterAnimationData animData;
        private SpriteAnimator spriteAnimator;
        private bool lastFlipX;

        private void Awake()
        {
            spriteAnimator = GetComponent<SpriteAnimator>();
        }

        private void Update()
        {
            if (playerTransform == null) return;

            if (isAttacking) return;

            HandleCombat();
            HandleMovement();
        }
        private void HandleCombat()
        {
            if (combatAbility == null || Time.time < nextAttackTime) return;

            float detectionRange = 5f;
            if (combatAbility is MeleeAbility melee) detectionRange = melee.offsetDistance + melee.areaRadius;

            Collider2D enemy = Physics2D.OverlapCircle(transform.position, detectionRange, enemyLayer);

            if (enemy != null && !enemy.CompareTag("Player"))
            {
                StartCoroutine(PerformAttackRoutine(enemy.transform.position));
            }
        }

        private System.Collections.IEnumerator PerformAttackRoutine(Vector3 targetPos)
        {
            isAttacking = true;

            lastFlipX = targetPos.x < transform.position.x;

            float animDuration = spriteAnimator.PlayAnimation(animData.firstAttackSprite, lastFlipX);

            int frameDeDisparo = 3; 
            float atrasoParaDisparar = frameDeDisparo * 0.1f; 

            float elapsed = 0f;
            bool jaDisparou = false;

            while (elapsed < animDuration)
            {
                elapsed += Time.deltaTime;

                spriteAnimator.PlayAnimation(animData.firstAttackSprite, lastFlipX);

                if (!jaDisparou && elapsed >= atrasoParaDisparar)
                {
                    combatAbility.Execute(this.gameObject, targetPos);
                    jaDisparou = true;
                }

                yield return null;
            }

            isAttacking = false;
            nextAttackTime = Time.time + attackCooldown;
        }

        private void HandleMovement()
        {
            if (isAttacking) return;

            bool playerFlipped = playerTransform.GetComponent<SpriteRenderer>().flipX;
            float currentOffsetX = playerFlipped ? -formationOffset.x : formationOffset.x;
            Vector3 targetPosition = playerTransform.position + new Vector3(currentOffsetX, formationOffset.y, 0);

            float distance = Vector2.Distance(transform.position, targetPosition);

            if (distance > stoppingDistance)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

                Vector2 moveDir = (targetPosition - transform.position).normalized;
                if (Mathf.Abs(moveDir.x) > 0.1f) lastFlipX = moveDir.x < 0;

                spriteAnimator.PlayAnimation(animData.walkSprites, lastFlipX);
            }
            else
            {
                spriteAnimator.PlayAnimation(animData.idleSprites, lastFlipX);
            }
        }
    }
}