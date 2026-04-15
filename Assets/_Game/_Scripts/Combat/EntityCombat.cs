using UnityEngine;
using System.Collections;
using SunnySword.Abilities;
using SunnySword.Animation;
using SunnySword.Stats;

namespace SunnySword.Combat
{
    public class EntityCombat : MonoBehaviour
    {
        private SpriteAnimator spriteAnimator;
        private StatsHandler stats;
        public bool IsAttacking { get; private set; }

        private void Awake()
        {
            spriteAnimator = GetComponent<SpriteAnimator>();
            stats = GetComponent<StatsHandler>();
        }

        public void Attack(AttackAbilityData ability, Vector2 targetPos, Sprite[] animSprites, int hitFrame)
        {
            if (IsAttacking || ability == null) return;
            StartCoroutine(AttackRoutine(ability, targetPos, animSprites, hitFrame));
        }

        private IEnumerator AttackRoutine(AttackAbilityData ability, Vector2 targetPos, Sprite[] animSprites, int hitFrame)
        {
            IsAttacking = true;

            bool flipX = targetPos.x < transform.position.x;

            float duration = 0f;
            if (spriteAnimator != null && animSprites != null)
            {
                duration = spriteAnimator.PlayAnimation(animSprites, flipX);
            }

            float delayUntilHit = hitFrame * 0.12f;
            yield return new WaitForSeconds(delayUntilHit);

            ExecuteAbilityImpact(ability, targetPos);

            float remainingTime = duration - delayUntilHit;
            if (remainingTime > 0) yield return new WaitForSeconds(remainingTime);

            IsAttacking = false;
        }

        private void ExecuteAbilityImpact(AttackAbilityData ability, Vector2 targetPos)
        {
            if (ability is ProjectileAbility)
            {
                ability.Execute(this.gameObject, targetPos);

                return;
            }

            int finalDamage = ability.CalculateDamage(this.gameObject);

            Collider2D[] hits = Physics2D.OverlapCircleAll(targetPos, ability.attackRange, ability.targetLayer);

            foreach (Collider2D hit in hits)
            {
                if (hit.gameObject == gameObject) continue;

                if (hit.TryGetComponent<IDamageable>(out var target))
                {
                    target.TakeDamage(finalDamage);
                    Debug.Log($"[Combate] {gameObject.name} atingiu {hit.name} com {ability.abilityName}. Dano: {finalDamage}");
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

        }
    }
}