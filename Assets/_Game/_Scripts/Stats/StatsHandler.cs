using UnityEngine;
using System;
using SunnySword.Combat; 

namespace SunnySword.Stats
{
    public class StatsHandler : MonoBehaviour, IDamageable
    {
        [SerializeField] private CharacterStatsData data;

        [Header("UI do Combate")]
        public GameObject damagePopupPrefab; 

        public float CurrentHealth { get; private set; }
        public float CurrentMana { get; private set; }
        public float CurrentStamina { get; private set; }

        public bool CanRegenerateStamina { get; set; } = true;
        public bool IsBlocking { get; set; } = false;
        public float CurrentExp { get; private set; }
        public int CurrentLevel { get; private set; } = 1;
        public float ExpToNextLevel => CurrentLevel * 100f;

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
            if (IsBlocking)
            {        
                amount = amount * (1f - data.blockDamageReduction);
                Debug.Log($"[DEFESA] Ataque bloqueado! Dano reduzido para {amount}");
            }

            CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
            OnStatsChanged?.Invoke();

            if (damagePopupPrefab != null)
            {
                GameObject popup = Instantiate(damagePopupPrefab);
                if (popup.TryGetComponent<SunnySword.UI.DamagePopup>(out var popupScript))
                {
                    popupScript.Setup(Mathf.RoundToInt(amount), this.gameObject);
                }
            }

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

        public void AddExp(float amount)
        {
            CurrentExp += amount;
            Debug.Log($"[SISTEMA] Ganhou {amount} EXP! Total: {CurrentExp} / {ExpToNextLevel}");

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

            Debug.Log($"[SISTEMA] LEVEL UP! Bem-vindo ao Nível {CurrentLevel}!");

            Heal(data.maxHealth);
            CurrentMana = data.maxMana;
        }

        public void Heal(float amount)
        {
            CurrentHealth = Mathf.Min(data.maxHealth, CurrentHealth + amount);
            Debug.Log($"[CURA] Recuperou {amount} de vida!");
            OnStatsChanged?.Invoke();
        }

        public void LoadCharacterData(CharacterStatsData newData)
        {
            this.data = newData;

            CurrentHealth = Mathf.Min(CurrentHealth, data.maxHealth);
            CurrentMana = Mathf.Min(CurrentMana, data.maxMana);
            CurrentStamina = Mathf.Min(CurrentStamina, data.maxStamina);
        }

        public void TriggerStatsUpdate()
        {
            OnStatsChanged?.Invoke();
        }

        public bool HasStamina => CurrentStamina > 0;
        public CharacterStatsData Data => data;
    }
}