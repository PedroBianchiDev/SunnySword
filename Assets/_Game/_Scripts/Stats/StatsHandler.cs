using UnityEngine;
using System;
using SunnySword.Combat;

namespace SunnySword.Stats
{
    public class StatsHandler : MonoBehaviour, SunnySword.Combat.IDamageable
    {
        [SerializeField] private CharacterStatsData data;
        public CharacterStatsData Data => data;
        private bool IsDead;

        [Header("UI & Feedback")]
        public GameObject damagePopupPrefab;

        public float CurrentHealth { get; private set; }
        public float CurrentMana { get; private set; }
        public float CurrentStamina { get; private set; }

        public float CurrentExp { get; private set; }
        public int CurrentLevel { get; private set; } = 1;
        public float ExpToNextLevel => CurrentLevel * 100f;

        public bool CanRegenerateStamina { get; set; } = true;
        public bool IsBlocking { get; set; } = false;

        private float currentStaminaRegenDelay;

        public event Action OnStatsChanged;
        public event Action OnDeath;

        private void Awake()
        {
            if (data == null) return;
            ResetStats();
        }

        private void Update()
        {
            RegenerateResources();
        }

        public void ResetStats()
        {
            if (data == null) return;
            CurrentHealth = data.maxHealth;
            CurrentMana = data.maxMana;
            CurrentStamina = data.maxStamina;

            OnStatsChanged?.Invoke();
        }

        private void RegenerateResources()
        {
            float previousMana = CurrentMana;
            float previousStamina = CurrentStamina;

            CurrentMana = Mathf.MoveTowards(CurrentMana, data.maxMana, data.manaRegenRate * Time.deltaTime);

            if (currentStaminaRegenDelay > 0)
            {
                currentStaminaRegenDelay -= Time.deltaTime;
            }
            else if (CanRegenerateStamina && !IsBlocking)
            {
                CurrentStamina = Mathf.MoveTowards(CurrentStamina, data.maxStamina, data.staminaRegenRate * Time.deltaTime);
            }

            if (CurrentMana != previousMana || CurrentStamina != previousStamina)
            {
                OnStatsChanged?.Invoke();
            }
        }

        public void TakeDamage(float amount)
        {
            Debug.Log($"[DANO] {gameObject.name} foi atingido! Dano bruto: {amount}");

            if (CurrentHealth <= 0) return;

            int finalDamage = Mathf.RoundToInt(amount);

            if (IsBlocking)
            {
                finalDamage = Mathf.RoundToInt(finalDamage * (1f - data.blockDamageReduction));
                ConsumeBlockStamina();
            }

            CurrentHealth -= finalDamage;

            if (damagePopupPrefab != null)
            {
                GameObject popup = Instantiate(damagePopupPrefab);
                if (popup.TryGetComponent<SunnySword.UI.DamagePopup>(out var popupScript))
                {
                    popupScript.Setup(finalDamage, this.gameObject);
                }
            }

            OnStatsChanged?.Invoke();

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }


        public void ConsumeStamina(float amount)
        {
            CurrentStamina = Mathf.Max(0, CurrentStamina - amount);
            currentStaminaRegenDelay = data.staminaRegenDelay;
            OnStatsChanged?.Invoke();
        }

        public void ConsumeBlockStamina() => ConsumeStamina(data.blockStaminaCost);

        public void Heal(float amount)
        {
            CurrentHealth = Mathf.Min(data.maxHealth, CurrentHealth + amount);
            OnStatsChanged?.Invoke();
        }

        public void UseMana(float amount)
        {
            CurrentMana = Mathf.Max(0, CurrentMana - amount);
            OnStatsChanged?.Invoke();
        }


        public void AddExp(float amount)
        {
            CurrentExp += amount;
            if (CurrentExp >= ExpToNextLevel)
            {
                LevelUp();
            }
            OnStatsChanged?.Invoke();
        }

        private void LevelUp()
        {
            CurrentExp -= ExpToNextLevel;
            CurrentLevel++;
            Heal(data.maxHealth);
            CurrentMana = data.maxMana;
            Debug.Log($"[LEVEL UP] Nível {CurrentLevel} alcançado!");
        }


        public void LoadCharacterData(CharacterStatsData newData)
        {
            this.data = newData;

            ResetStats();
            OnStatsChanged?.Invoke();
        }

        public void TriggerStatsUpdate()
        {
            OnStatsChanged?.Invoke();
        }

        private void Die()
        {
            IsDead = true;
            OnDeath?.Invoke();
        }

    }
}