using System;
using System.Collections;
using UnityEngine;

namespace SunnySword.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private float attackRange = 1.2f;
        [SerializeField] private int damage = 1;
        [SerializeField] private float attackDelay = 0.3f;

        public bool IsAttacking { get; private set; }

        public void PerformAttack(Vector2 direction, float duration)
        {
            if (IsAttacking) return;
            StartCoroutine(AttackRoutine(direction, duration));
        }

        private IEnumerator AttackRoutine(Vector2 direction, float duration)
        {
            IsAttacking = true;

            yield return new WaitForSeconds(duration);

            Vector2 attackPosition = (Vector2)transform.position + direction.normalized * 0.8f;
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRange);

            foreach (Collider2D enemy in hitEnemies) { }

            IsAttacking = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }

}
