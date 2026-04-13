using SunnySword.Abilities;
using SunnySword.Animation;
using SunnySword.Combat;
using SunnySword.Stats;
using UnityEngine;

namespace SunnySword.Enemies
{
    public class SkeletonAI : MonoBehaviour
    {
        [Header("Configurações de Movimento")]
        public float moveSpeed = 2.5f;
        public float chaseRange = 10f;
        public float attackRange = 1.8f;
        public float attackCooldown = 2f;

        [Header("Detecção")]
        public LayerMask playerLayer; 
        public float searchInterval = 0.5f; 

        [Header("Combate")]
        public AttackAbilityData enemyAttackSkill;
        public int attackActionFrame = 2;

        [Header("Animação")]
        public CharacterAnimationData animData;

        private SpriteAnimator spriteAnimator;
        private Transform currentTarget;
        private Rigidbody2D rb;
        private StatsHandler stats;
        private EntityCombat entityCombat;

        private bool isDead = false;
        private float nextAttackTime;
        private float nextSearchTime;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            stats = GetComponent<StatsHandler>();
            spriteAnimator = GetComponent<SpriteAnimator>();
            entityCombat = GetComponent<EntityCombat>();
        }

        private void Start()
        {
            if (stats != null) stats.OnDeath += Die;
        }

        private void Update()
        {
            if (isDead) return;

            if (currentTarget == null || Time.time >= nextSearchTime)
            {
                FindTarget();
                nextSearchTime = Time.time + searchInterval;
            }

            if (currentTarget == null)
            {
                StopEnemy();
                return;
            }

            if (entityCombat != null && entityCombat.IsAttacking)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            float distance = Vector2.Distance(transform.position, currentTarget.position);

            if (distance <= attackRange)
            {
                AttackPlayer();
            }
            else if (distance <= chaseRange)
            {
                ChasePlayer();
            }
            else
            {
                StopEnemy();
            }
        }

        private void FindTarget()
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, chaseRange, playerLayer);

            float closestDistance = Mathf.Infinity;
            Transform tempTarget = null;

            foreach (var t in targets)
            {
                float dist = Vector2.Distance(transform.position, t.transform.position);

                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    tempTarget = t.transform;
                }
            }

            currentTarget = tempTarget;
        }

        private void ChasePlayer()
        {
            Vector2 direction = (currentTarget.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
            spriteAnimator.PlayAnimation(animData.walkSprites, direction.x < 0);
        }

        private void AttackPlayer()
        {
            rb.linearVelocity = Vector2.zero;

            if (Time.time >= nextAttackTime)
            {
                if (entityCombat != null)
                {
                    entityCombat.Attack(enemyAttackSkill, currentTarget.position, animData.firstAttackSprite, attackActionFrame);
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
        }

        private void StopEnemy()
        {
            rb.linearVelocity = Vector2.zero;
            spriteAnimator.PlayAnimation(animData.idleSprites, false);
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
            spriteAnimator.StopAnimation();
            GetComponent<SpriteRenderer>().color = Color.gray;
            Destroy(gameObject, 2f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }
}