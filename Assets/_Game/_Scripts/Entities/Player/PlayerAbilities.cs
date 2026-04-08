using UnityEngine;
using SunnySword.Stats;

namespace SunnySword.Player
{
    [RequireComponent(typeof(PlayerInput), typeof(StatsHandler))]
    public class PlayerAbilities : MonoBehaviour
    {
        [Header("Habilidades Equipadas")]
        public Abilities.AbilityData skill1;
        public Abilities.AbilityData skill2;

        private PlayerInput input;
        private StatsHandler stats;

        private float skill1CooldownTimer;
        private float skill2CooldownTimer;

        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            stats = GetComponent<StatsHandler>();

            // Assina os eventos de input
            input.OnSkill1Pressed += () => TryCastSkill(skill1, ref skill1CooldownTimer);
            input.OnSkill2Pressed += () => TryCastSkill(skill2, ref skill2CooldownTimer);
        }

        private void Update()
        {
            if (skill1CooldownTimer > 0) skill1CooldownTimer -= Time.deltaTime;
            if (skill2CooldownTimer > 0) skill2CooldownTimer -= Time.deltaTime;
        }

        private void TryCastSkill(Abilities.AbilityData skill, ref float cooldownTimer)
        {
            if (skill == null) return;

            if (cooldownTimer > 0)
            {
                Debug.Log($"{skill.abilityName} está em recarga!");
                return;
            }

            if (stats.CurrentMana < skill.manaCost)
            {
                Debug.Log("Mana insuficiente!");
                return;
            }

            stats.UseMana(skill.manaCost);
            cooldownTimer = skill.cooldown;
            skill.Execute(gameObject, input.MouseWorldPosition);
        }
    }
}