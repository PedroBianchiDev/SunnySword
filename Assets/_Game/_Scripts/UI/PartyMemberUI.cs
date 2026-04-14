using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SunnySword.Stats;

namespace SunnySword.UI
{
    public class PartyMemberUI : MonoBehaviour
    {
        [Header("Referências de UI")]
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

            targetStats.OnStatsChanged += UpdateVisuals;
            targetStats.OnDeath += HandleDeathUI;

            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            if (targetStats == null) return;

            hpBarFill.fillAmount = (float)targetStats.CurrentHealth / targetStats.Data.maxHealth;
            manaBarFill.fillAmount = (float)targetStats.CurrentMana / targetStats.Data.maxMana;
        }

        private void HandleDeathUI()
        {
            Color deadColor = new Color(0.2f, 0.2f, 0.2f, 1f);
            portraitImage.color = deadColor;

            if (TryGetComponent<Image>(out var bgImage))
            {
                bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
            }

            nameText.color = Color.gray;

            Debug.Log($"[HUD] Card de {targetStats.Data.characterName} atualizado para estado de morte.");
        }

        private void OnDestroy()
        {
            if (targetStats != null)
            {
                targetStats.OnStatsChanged -= UpdateVisuals;
                targetStats.OnDeath -= HandleDeathUI;
            }
        }
    }
}