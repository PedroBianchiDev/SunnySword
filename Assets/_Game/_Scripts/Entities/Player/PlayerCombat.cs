using UnityEngine;
using System.Collections;
using SunnySword.Stats; 

namespace SunnySword.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private float attackRange = 1.2f;

        private StatsHandler statsHandler;

        [Header("Effects")]
        public GameObject damagePopupPrefab;

        public bool IsAttacking { get; private set; }

        private void Awake()
        {
            statsHandler = GetComponent<StatsHandler>();
        }

        public void PerformAttack(Vector2 direction, float duration)
        {
            if (IsAttacking) return;
            StartCoroutine(AttackRoutine(direction, duration));
        }

        private IEnumerator AttackRoutine(Vector2 direction, float duration)
        {
            IsAttacking = true;

            yield return new WaitForSeconds(duration * 0.5f);

            Vector2 attackPosition = (Vector2)transform.position + direction.normalized * 0.8f;
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRange);

            int damageToDeal = statsHandler.Data.baseDamage;


            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log($"Acertou: {enemy.name} com {damageToDeal} de dano!");

                if (damagePopupPrefab != null)
                {
                    GameObject popup = Instantiate(damagePopupPrefab);

                    if (popup.TryGetComponent<UI.DamagePopup>(out UI.DamagePopup popupScript))
                    {
                        popupScript.Setup(damageToDeal, enemy.gameObject);
                    }
                }
            }

            yield return new WaitForSeconds(duration * 0.5f);
            IsAttacking = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}