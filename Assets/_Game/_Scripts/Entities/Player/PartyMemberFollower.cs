using SunnySword.Abilities;
using SunnySword.Animation;
using SunnySword.Combat; // Novo namespace
using SunnySword.Stats;
using UnityEngine;

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
        public float detectionRange = 5f;
        public LayerMask enemyLayer;
        public int attackHitFrame = 2;
        private bool isDead = false;

        [Header("Visual")]
        public CharacterAnimationData animData;

        private SpriteAnimator spriteAnimator;
        private Rigidbody2D rb;
        private EntityCombat entityCombat;
        private StatsHandler stats;
        private float nextAttackTime;
        private bool lastFlipX;

        private void Awake()
        {
            spriteAnimator = GetComponent<SpriteAnimator>();
            entityCombat = GetComponent<EntityCombat>();
            rb = GetComponent<Rigidbody2D>();
            stats = GetComponent<StatsHandler>();
        }

        void Start()
        {
            if (stats != null) stats.OnDeath += OnDeath;
        }

        private void Update()
        {
            if (isDead || playerTransform == null) return;

            if (entityCombat.IsAttacking)
            {
                rb.linearVelocity = Vector2.zero; 
                return;
            }

            HandleCombat();

            if (entityCombat.IsAttacking) return;

            HandleMovement();
        }

        private void HandleCombat()
        {
            if (combatAbility == null || Time.time < nextAttackTime) return;

            Collider2D enemy = Physics2D.OverlapCircle(transform.position, detectionRange, enemyLayer);

            if (enemy != null)
            {
                entityCombat.Attack(combatAbility, enemy.transform.position, animData.firstAttackSprite, attackHitFrame);
                nextAttackTime = Time.time + attackCooldown;
            }
        }

        void OnDeath()
        {
            isDead = true;

            if (rb != null) rb.linearVelocity = Vector2.zero;

            Invoke("DeactivateAfterDeath", 0.1f);
        }

        private void HandleMovement()
        {
            if (isDead) return;

            bool playerFlipped = playerTransform.GetComponent<SpriteRenderer>().flipX;
            float currentOffsetX = playerFlipped ? -formationOffset.x : formationOffset.x;
            Vector3 targetPosition = playerTransform.position + new Vector3(currentOffsetX, formationOffset.y, 0);

            float distance = Vector2.Distance(transform.position, targetPosition);

            if (distance > stoppingDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);

                Vector2 moveDir = (targetPosition - transform.position).normalized;
                if (Mathf.Abs(moveDir.x) > 0.1f) lastFlipX = moveDir.x < 0;

                spriteAnimator.PlayAnimation(animData.walkSprites, lastFlipX);
            }
            else
            {
                spriteAnimator.PlayAnimation(animData.idleSprites, lastFlipX);
            }
        }

        private void DeactivateAfterDeath()
        {
            gameObject.SetActive(false);
            Debug.Log($"[Party] {gameObject.name} foi removido da cena por morte.");
        }
    }
}