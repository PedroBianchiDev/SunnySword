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

            Debug.Log($"Projétil acertou {other.name} causando {damageToDeal} de dano!");

            if (popupPrefab != null)
            {
                GameObject popup = Instantiate(popupPrefab);
                if (popup.TryGetComponent<UI.DamagePopup>(out UI.DamagePopup popupScript))
                {
                    popupScript.Setup(damageToDeal, other.gameObject);
                }
            }

            Destroy(gameObject);
        }
    }
}