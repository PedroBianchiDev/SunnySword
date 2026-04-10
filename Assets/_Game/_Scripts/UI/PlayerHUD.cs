using UnityEngine;
using UnityEngine.UI;
using SunnySword.Stats;
using TMPro;

namespace SunnySword.UI
{
    public class PlayerHUD : MonoBehaviour
    {
        [Header("Player Ref")]
        [SerializeField] private StatsHandler playerStats;

        [Header("Bars UI")]
        [SerializeField] private Image healthBarFill;
        [SerializeField] private Image manaBarFill;
        [SerializeField] private Image staminaBarFill;
        [SerializeField] private Image expBarFill; 
        [SerializeField] private TextMeshProUGUI levelText;

        private void OnEnable()
        {
            if (playerStats != null)
            {
                playerStats.OnStatsChanged += UpdateBars;
            }
        }

        private void OnDisable()
        {
            if (playerStats != null)
            {
                playerStats.OnStatsChanged -= UpdateBars;
            }
        }

        private void Start()
        {
            UpdateBars();
        }

        private void UpdateBars()
        {
            if (playerStats == null || playerStats.Data == null) return;

            if (healthBarFill != null)
                healthBarFill.fillAmount = playerStats.CurrentHealth / playerStats.Data.maxHealth;

            if (manaBarFill != null)
                manaBarFill.fillAmount = playerStats.CurrentMana / playerStats.Data.maxMana;

            if (expBarFill != null)
            {
                expBarFill.fillAmount = playerStats.CurrentExp / playerStats.ExpToNextLevel;
            }

            if (staminaBarFill != null)
                staminaBarFill.fillAmount = playerStats.CurrentStamina / playerStats.Data.maxStamina;

            if (levelText != null)
            {
                levelText.text = $"{playerStats.CurrentLevel}";
            }
        }
    }
}