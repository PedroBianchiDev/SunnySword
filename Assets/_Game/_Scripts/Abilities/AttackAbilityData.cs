using UnityEngine;
using SunnySword.Stats;

namespace SunnySword.Abilities
{
    public abstract class AttackAbilityData : AbilityData
    {
        [Header("Configurações de Dano")]
        public float damageMultiplier = 1.0f;
        public int flatBonusDamage = 0;

        [Header("Interface (UI)")]
        public GameObject damagePopupPrefab; 

        protected int CalculateDamage(GameObject caster)
        {
            if (caster.TryGetComponent(out StatsHandler stats))
            {
                return Mathf.RoundToInt((stats.Data.baseDamage * damageMultiplier) + flatBonusDamage);
            }
            return flatBonusDamage;
        }
    }
}