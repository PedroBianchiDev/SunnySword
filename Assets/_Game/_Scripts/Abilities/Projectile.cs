using UnityEngine;

namespace SunnySword.Abilities
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class Projectile : MonoBehaviour
    {
        private int damageToDeal;
        private GameObject caster;
        private GameObject popupPrefab; 

        public void Setup(int damage, GameObject casterObj, GameObject popupVisual)
        {
            damageToDeal = damage;
            caster = casterObj;
            popupPrefab = popupVisual;

            Destroy(gameObject, 5f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject == caster) return;

            if (other.TryGetComponent<Combat.IDamageable>(out Combat.IDamageable damageableTarget))
            {
                damageableTarget.TakeDamage(damageToDeal);
            }

            Destroy(gameObject);
        }
    }
}