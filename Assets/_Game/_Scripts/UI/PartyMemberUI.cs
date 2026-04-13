using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SunnySword.Stats;

namespace SunnySword.UI
{
    public class PartyMemberUI : MonoBehaviour
    {
        [Header("Referências da UI")]
        public Image portraitImage;
        public TextMeshProUGUI nameText;
        public Image hpBarFill;
        public Image manaBarFill;

        private StatsHandler targetStats;

        public void Setup(StatsHandler stats)
        {
            if (stats == null) return;
            targetStats = stats;

            nameText.text = stats.Data.characterName;
            portraitImage.sprite = stats.Data.portrait;

            if (targetStats.CurrentHealth <= 0)
            {
                targetStats.ResetStats();
            }

            targetStats.OnStatsChanged += UpdateVisuals;
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            if (targetStats == null || targetStats.Data == null) return;

            if (hpBarFill != null)
                hpBarFill.fillAmount = targetStats.CurrentHealth / targetStats.Data.maxHealth;

            if (manaBarFill != null)
                manaBarFill.fillAmount = targetStats.CurrentMana / targetStats.Data.maxMana;
        }

        private void OnDestroy()
        {
            if (targetStats != null)
                targetStats.OnStatsChanged -= UpdateVisuals;
        }
    }
}