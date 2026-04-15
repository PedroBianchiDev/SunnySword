using UnityEngine;

namespace SunnySword.Abilities
{
    [CreateAssetMenu(menuName = "SunnySword/Abilities/Ranged Projectile")]
    public class ProjectileAbility : AttackAbilityData
    {
        public GameObject projectilePrefab;
        public float projectileSpeed = 10f;

        public override void Execute(GameObject caster, Vector2 targetPosition)
        {
            Vector2 direction = (targetPosition - (Vector2)caster.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            GameObject projGO = Instantiate(projectilePrefab, caster.transform.position, Quaternion.Euler(0, 0, angle));

            if (projGO.TryGetComponent<Projectile>(out var proj))
            {
                proj.Setup(CalculateDamage(caster), projectileSpeed, caster, targetLayer);
            }
        }
    }
}