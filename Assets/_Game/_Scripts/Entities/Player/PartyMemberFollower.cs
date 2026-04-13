using UnityEngine;
using SunnySword.Animation;
using SunnySword.Abilities;
using SunnySword.Combat; // Novo namespace

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

        [Header("Visual")]
        public CharacterAnimationData animData;

        private SpriteAnimator spriteAnimator;
        private Rigidbody2D rb;
        private EntityCombat entityCombat;
        private float nextAttackTime;
        private bool lastFlipX;

        private void Awake()
        {
            spriteAnimator = GetComponent<SpriteAnimator>();
            entityCombat = GetComponent<EntityCombat>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (playerTransform == null) return;

            // 1. PRIORIDADE TOTAL AO COMBATE
            // Se o motor de combate diz que está atacando, paramos o script aqui mesmo.
            if (entityCombat.IsAttacking)
            {
                rb.linearVelocity = Vector2.zero; // Garante que ele não deslize atacando
                return;
            }

            // 2. TENTA ATACAR
            HandleCombat();

            // 3. SE COMEÇOU UM ATAQUE AGORA, SAI DO UPDATE
            if (entityCombat.IsAttacking) return;

            // 4. SÓ MOVE SE NÃO ESTIVER FAZENDO NADA DE COMBATE
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

        private void HandleMovement()
        {
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