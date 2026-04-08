using UnityEngine;

namespace SunnySword.Abilities
{
    [CreateAssetMenu(menuName = "SunnySword/Abilities/Melee Cleave")]
    public class MeleeAbility : AttackAbilityData
    {
        public float areaRadius = 2f;
        public float offsetDistance = 1.5f;

        [Header("Visual")]
        public GameObject slashEffectPrefab;
        public float effectDuration = 0.2f;

        public override void Execute(GameObject caster, Vector2 targetPosition)
        {
            Vector2 castDirection = (targetPosition - (Vector2)caster.transform.position).normalized;
            Vector2 hitCenter = (Vector2)caster.transform.position + (castDirection * offsetDistance);

            if (slashEffectPrefab != null)
            {
                GameObject visual = Instantiate(slashEffectPrefab, hitCenter, Quaternion.identity);
                float angle = Mathf.Atan2(castDirection.y, castDirection.x) * Mathf.Rad2Deg;
                visual.transform.rotation = Quaternion.Euler(0, 0, angle);
                Destroy(visual, effectDuration);
            }

            Collider2D[] hits = Physics2D.OverlapCircleAll(hitCenter, areaRadius);
            int finalDamage = CalculateDamage(caster);

            foreach (var hit in hits)
            {
                if (hit.gameObject == caster) continue;

                Debug.Log($"{abilityName} acertou {hit.name} causando {finalDamage} de dano!");

                if (damagePopupPrefab != null)
                {
                    GameObject popup = Instantiate(damagePopupPrefab);
                    if (popup.TryGetComponent<UI.DamagePopup>(out UI.DamagePopup popupScript))
                    {
                        popupScript.Setup(finalDamage, hit.gameObject);
                    }
                }
            }
        }
    }
}