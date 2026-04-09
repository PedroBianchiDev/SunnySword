
using UnityEngine;
using SunnySword.Stats;

namespace SunnySword.Abilities
{
    public abstract class AbilityData : ScriptableObject
    {
        [Header("Informações Gerais")]
        public string abilityName;
        [TextArea(3, 5)] public string description; 
        public Sprite icon;

        [Header("Status")]
        public float manaCost;
        public float cooldown;

        public abstract void Execute(GameObject caster, Vector2 targetPosition);
    }
}