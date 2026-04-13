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

        public void Attack(AttackAbilityData ability, Vector2 targetPos, Sprite[] animSprites, int hitFrame, float range = 1.5f)
        {
            if (IsAttacking) return;
            StartCoroutine(AttackRoutine(ability, targetPos, animSprites, hitFrame, range));
        }

        private IEnumerator AttackRoutine(AttackAbilityData ability, Vector2 targetPos, Sprite[] animSprites, int hitFrame, float range)
        {
            IsAttacking = true;

            bool flipX = targetPos.x < transform.position.x;
            float duration = spriteAnimator.PlayAnimation(animSprites, flipX);

            float delayUntilHit = hitFrame * 0.12f;
            yield return new WaitForSeconds(delayUntilHit);

            if (ability != null)
            {
                ability.Execute(this.gameObject, targetPos);
            }
            else
            {
                float dist = Vector2.Distance(transform.position, targetPos);
                if (dist <= range + 0.5f)
                {
                    if (targetPos != null) 
                    {
                        Collider2D hit = Physics2D.OverlapCircle(transform.position, range, LayerMask.GetMask("Player"));
                        if (hit != null && hit.TryGetComponent<IDamageable>(out var target))
                        {
                            target.TakeDamage(stats.Data.baseDamage);
                        }
                    }
                }
            }

            float remainingTime = duration - delayUntilHit;
            if (remainingTime > 0) yield return new WaitForSeconds(remainingTime);

            IsAttacking = false;
        }
    }
}