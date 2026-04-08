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
            CurrentMana = Mathf.MoveTowards(CurrentMana, Data.maxMana, Data.manaRegenRate * Time.deltaTime);

            if (CanRegenerateStamina && CurrentStamina < Data.maxStamina)
            {
                CurrentStamina = Mathf.MoveTowards(CurrentStamina, Data.maxStamina, Data.staminaRegenRate * Time.deltaTime);
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
            OnStatsChanged?.Invoke();
        }

        public bool HasStamina => CurrentStamina > 0;
        public CharacterStatsData Data => data;
    }
}