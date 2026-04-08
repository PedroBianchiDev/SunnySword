using UnityEngine;

namespace SunnySword.Stats
{
    [CreateAssetMenu(fileName = "New Character Stats", menuName = "SunnySword/Stats/CharacterData")]
    public class CharacterStatsData : ScriptableObject
    {
        [Header("Life")]
        public float maxHealth = 100f;

        [Header("Mana")]
        public float maxMana = 50f;
        public float manaRegenRate = 2f;

        [Header("Stamina")]
        public float maxStamina = 100f;
        public float staminaRegenRate = 15f; 
        public float blockStaminaCost = 10f; 

        [Header("Movement")]
        public float baseMoveSpeed = 5f;

        [Header("Combat")]
        public int baseDamage = 10;
    }
}