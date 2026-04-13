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

        [Header("Dados do Membro (Scriptable Objects)")]
        public CharacterStatsData statsData;      
        public AttackAbilityData combatAbility;    

        [Header("Configurações de Combate")]
        public float attackCooldown = 2f;
        public LayerMask enemyLayer;
        private float nextAttackTime;

        [Header("Visual")]
        public CharacterAnimationData animData;
        private SpriteAnimator spriteAnimator;
        private bool lastFlipX;

        private void Awake()
        {
            spriteAnimator = GetComponent<SpriteAnimator>();

            if (playerTransform != null)
            {
                Collider2D myCollider = GetComponent<Collider2D>();
                Collider2D playerCollider = playerTransform.GetComponent<Collider2D>();

                if (myCollider != null && playerCollider != null)
                {
                    Physics2D.IgnoreCollision(myCollider, playerCollider);
                }
            }
        }

        private void Update()
        {
            if (playerTransform == null) return;

            HandleCombat();
            HandleMovement();
        }

        private void HandleCombat()
        {
            if (combatAbility == null || Time.time < nextAttackTime) return;

            float detectionRange = 5f;
            if (combatAbility is MeleeAbility melee)
            {
                detectionRange = melee.offsetDistance + melee.areaRadius;
            }

            Collider2D[] potentialTargets = Physics2D.OverlapCircleAll(transform.position, detectionRange, enemyLayer);

            Collider2D targetEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (var col in potentialTargets)
            {
                if (col.CompareTag("Player") || col.gameObject == gameObject) continue;

                float distanceToEnemy = Vector2.Distance(transform.position, col.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    targetEnemy = col;
                }
            }

            if (targetEnemy != null)
            {
                combatAbility.Execute(this.gameObject, targetEnemy.transform.position);
                nextAttackTime = Time.time + attackCooldown;
            }
        }

        private void HandleMovement()
        {
            bool playerFlipped = playerTransform.GetComponent<SpriteRenderer>().flipX;
            float currentOffsetX = playerFlipped ? -formationOffset.x : formationOffset.x;
            Vector3 relativeOffset = new Vector3(currentOffsetX, formationOffset.y, 0);

            Vector3 targetPosition = playerTransform.position + relativeOffset;
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
                spriteAnimator.PlayAnimation(animData.idleSprites, playerFlipped);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (combatAbility is MeleeAbility melee)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, melee.offsetDistance + melee.areaRadius);
            }
        }
    }
}