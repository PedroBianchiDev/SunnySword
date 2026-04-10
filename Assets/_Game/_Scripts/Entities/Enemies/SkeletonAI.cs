using UnityEngine;
using SunnySword.Abilities;
using SunnySword.Combat;
using SunnySword.Animation; 

namespace SunnySword.Enemies
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class SkeletonAI : MonoBehaviour, IDamageable
    {
        private enum State { Idle, Chase, Attack }
        private State currentState;

        [Header("Status do Inimigo")]
        public int maxHealth = 50;
        private int currentHealth;
        public float moveSpeed = 2.5f;
        public float chaseRange = 5f;
        public float attackRange = 1.2f;
        public float attackCooldown = 1.5f;

        [Header("Combate & UI")]
        public AttackAbilityData enemyAttackSkill;
        public GameObject damagePopupPrefab;
        [Tooltip("Em qual frame do ataque (0, 1, 2...) o dano deve sair?")]
        public int attackActionFrame = 2; 

        [Header("Animação (O Seu Cartucho!)")]
        public CharacterAnimationData animData;
        public float frameRate = 10f; 

        [Header("Referências")]
        public Transform playerTarget;

        private Rigidbody2D rb;
        private SpriteRenderer sr;
        private float lastAttackTime;
        private bool isDead = false;

        private Sprite[] currentAnimArray;
        private int currentFrame;
        private float animTimer;
        private bool isLoopingAnim;
        private bool hasDealtDamageThisAttack = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            currentState = State.Idle;
            currentHealth = maxHealth;

            if (animData != null) PlayAnimation(animData.idleSprites, true);
        }

        private void Start()
        {
            if (playerTarget == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) playerTarget = player.transform;
            }
        }

        private void Update()
        {
            if (isDead) return;

            UpdateAnimation(); 

            if (playerTarget == null) return;
            float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

            switch (currentState)
            {
                case State.Idle:
                    if (animData != null) PlayAnimation(animData.idleSprites, true);

                    if (distanceToPlayer <= chaseRange) currentState = State.Chase;
                    break;

                case State.Chase:
                    if (animData != null) PlayAnimation(animData.walkSprites, true);

                    if (distanceToPlayer <= attackRange) currentState = State.Attack;
                    else if (distanceToPlayer > chaseRange * 1.5f) currentState = State.Idle;
                    break;

                case State.Attack:
                    if (animData != null) PlayAnimation(animData.firstAttackSprite, false);
                    break;
            }
        }

        private void FixedUpdate()
        {
            if (currentState == State.Chase && playerTarget != null && !isDead)
            {
                Vector2 direction = (playerTarget.position - transform.position).normalized;
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                FlipSprite(direction.x);
            }
        }

        private void PlayAnimation(Sprite[] newAnimArray, bool loop)
        {
            if (currentAnimArray == newAnimArray || newAnimArray == null || newAnimArray.Length == 0) return;

            currentAnimArray = newAnimArray;
            isLoopingAnim = loop;
            currentFrame = 0;
            animTimer = 0f;
            sr.sprite = currentAnimArray[0];

            if (currentAnimArray == animData.firstAttackSprite)
            {
                hasDealtDamageThisAttack = false;
            }
        }

        private void UpdateAnimation()
        {
            if (currentAnimArray == null || currentAnimArray.Length == 0) return;

            animTimer += Time.deltaTime;
            float timePerFrame = 1f / frameRate;

            if (animTimer >= timePerFrame)
            {
                animTimer -= timePerFrame;
                currentFrame++;

                if (currentFrame >= currentAnimArray.Length)
                {
                    if (isLoopingAnim)
                    {
                        currentFrame = 0; 
                    }
                    else
                    {
                        currentFrame = currentAnimArray.Length - 1; 

                        if (currentState == State.Attack)
                        {
                            lastAttackTime = Time.time;
                            float distance = Vector2.Distance(transform.position, playerTarget.position);
                            currentState = (distance <= attackRange) ? State.Idle : State.Chase;
                        }
                    }
                }

                sr.sprite = currentAnimArray[currentFrame];

                if (currentAnimArray == animData.firstAttackSprite &&
                    currentFrame == attackActionFrame &&
                    !hasDealtDamageThisAttack)
                {
                    ExecuteAttack();
                    hasDealtDamageThisAttack = true; 
                }
            }
        }

        private void ExecuteAttack()
        {
            if (enemyAttackSkill != null && playerTarget != null)
            {
                enemyAttackSkill.Execute(gameObject, playerTarget.position);
            }
        }

        private void FlipSprite(float directionX)
        {
            if (directionX > 0) sr.flipX = false;
            else if (directionX < 0) sr.flipX = true;
        }

        public void TakeDamage(float amount)
        {
            if (isDead) return;

            int finalDamage = Mathf.RoundToInt(amount);

            currentHealth -= finalDamage;

            if (damagePopupPrefab != null)
            {
                GameObject popup = Instantiate(damagePopupPrefab);
                if (popup.TryGetComponent<SunnySword.UI.DamagePopup>(out var popupScript))
                {
                    popupScript.Setup(finalDamage, this.gameObject);
                }
            }

            if (currentHealth <= 0) Die();
        }

        private void Die()
        {
            isDead = true;
            rb.simulated = false;
            sr.color = Color.gray;
            Destroy(gameObject, 2f);
        }
    }
}