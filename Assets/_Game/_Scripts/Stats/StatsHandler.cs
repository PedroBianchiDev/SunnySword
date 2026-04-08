using UnityEngine;
using System;

namespace SunnySword.Stats
{
    public class StatsHandler : MonoBehaviour
    {
        [SerializeField] private CharacterStatsData data;

        public float CurrentHealth { get; private set; }
        public float CurrentMana { get; private set; }
        public float CurrentStamina { get; private set; }

        public bool CanRegenerateStamina { get; set; } = true;

        private float currentStaminaRegenDelay;

        public event Action OnStatsChanged;

        private void Awake()
        {
            if (data == null) return;

            CurrentHealth = data.maxHealth;
            CurrentMana = data.maxMana;
            CurrentStamina = data.maxStamina;
        }

        private void Update()
        {
            RegenerateResources();
        }

        private void RegenerateResources()
        {
            CurrentMana = Mathf.MoveTowards(CurrentMana, data.maxMana, data.manaRegenRate * Time.deltaTime);

            if (currentStaminaRegenDelay > 0)
            {
                currentStaminaRegenDelay -= Time.deltaTime;
            }
            else if (CanRegenerateStamina && CurrentStamina < data.maxStamina)
            {
                CurrentStamina = Mathf.MoveTowards(CurrentStamina, data.maxStamina, data.staminaRegenRate * Time.deltaTime);
            }

            OnStatsChanged?.Invoke();
        }

        public void TakeDamage(float amount)
        {
            CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
            OnStatsChanged?.Invoke();
            if (CurrentHealth <= 0) Debug.Log("Died");
        }

        public void UseMana(float amount)
        {
            CurrentMana = Mathf.Max(0, CurrentMana - amount);
            OnStatsChanged?.Invoke();
        }

        public void ConsumeBlockStamina()
        {
            CurrentStamina = Mathf.Max(0, CurrentStamina - data.blockStaminaCost * Time.deltaTime);
            currentStaminaRegenDelay = data.staminaRegenDelay;
            OnStatsChanged?.Invoke();
        }

        public bool HasStamina => CurrentStamina > 0;
        public CharacterStatsData Data => data;
    }
}