using UnityEngine;
using SunnySword.Stats;

namespace SunnySword.Abilities
{
    public abstract class AttackAbilityData : AbilityData
    {
        [Header("Configurações de Alvo")]
        public LayerMask targetLayer;

        public float attackRange = 1.5f;

        [Header("Configurações de Dano")]
        public float damageMultiplier = 1.0f;
        public int flatBonusDamage = 0;

        [Header("Interface (UI)")]
        public GameObject damagePopupPrefab;

        public int CalculateDamage(GameObject caster)
        {
            if (caster.TryGetComponent<StatsHandler>(out var stats))
            {
                return Mathf.RoundToInt((stats.Data.baseDamage * damageMultiplier) + flatBonusDamage);
            }
            return flatBonusDamage;
        }
    }
}