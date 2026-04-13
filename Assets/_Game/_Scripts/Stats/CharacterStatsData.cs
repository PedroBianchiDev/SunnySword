using UnityEngine;

namespace SunnySword.Stats
{
    [CreateAssetMenu(fileName = "New Character Stats", menuName = "SunnySword/Stats/CharacterData")]
    public class CharacterStatsData : ScriptableObject
    {
        [Header("Identidade")]
        public string characterName;
        public Sprite portrait;

        [Header("Life")]
        public float maxHealth = 100f;

        [Header("Mana")]
        public float maxMana = 50f;
        public float manaRegenRate = 2f;

        [Header("Stamina")]
        public float maxStamina = 100f;
        public float staminaRegenRate = 15f; 
        public float blockStaminaCost = 10f;
        public float staminaRegenDelay = 1f;

        [Header("Movement")]
        public float baseMoveSpeed = 5f;

        [Header("Combat")]
        public int baseDamage = 10;

        [Range(0f, 1f)]
        [Tooltip("Porcentagem de dano defendido (0.2 = 20%, 1.0 = 100% de defesa)")]
        public float blockDamageReduction = 0.2f;
    }
}