
using UnityEngine;
using SunnySword.Stats;

namespace SunnySword.Abilities
{
    public abstract class AbilityData : ScriptableObject
    {
        public string abilityName;
        public float manaCost;
        public float cooldown;

        public abstract void Execute(GameObject caster, Vector2 targetPosition);
    }
}