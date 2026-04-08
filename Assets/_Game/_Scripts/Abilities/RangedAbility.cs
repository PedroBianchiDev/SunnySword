using UnityEngine;

namespace SunnySword.Abilities
{
    [CreateAssetMenu(menuName = "SunnySword/Abilities/Ranged Projectile")]
    public class RangedAbility : AttackAbilityData
    {
        public GameObject projectilePrefab;
        public float projectileSpeed = 10f;

        public override void Execute(GameObject caster, Vector2 targetPosition)
        {
            Vector2 castDirection = (targetPosition - (Vector2)caster.transform.position).normalized;
            int finalDamage = CalculateDamage(caster);

            GameObject proj = Instantiate(projectilePrefab, caster.transform.position, Quaternion.identity);

            if (proj.TryGetComponent(out Projectile projectileScript))
            {
                projectileScript.Setup(finalDamage, caster, damagePopupPrefab);
            }

            if (proj.TryGetComponent(out Rigidbody2D rb))
            {
                rb.linearVelocity = castDirection * projectileSpeed;
            }

            float angle = Mathf.Atan2(castDirection.y, castDirection.x) * Mathf.Rad2Deg;
            proj.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}