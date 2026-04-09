using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SunnySword.Abilities; 

namespace SunnySword.UI
{
    public class TooltipUI : MonoBehaviour
    {
        public static TooltipUI Instance { get; private set; }

        [Header("Elementos de Texto e Imagem")]
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI costText;
        public Image iconImage;

        [Header("Configurações")]
        public Vector2 offset = new Vector2(15f, -15f); 

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            HideTooltip();
        }

        private void Update()
        {
            if (gameObject.activeSelf)
            {
                transform.position = (Vector2)Input.mousePosition + offset;
            }
        }

        public void ShowTooltip(AbilityData ability)
        {
            if (ability == null) return;

            nameText.text = ability.abilityName;
            descriptionText.text = ability.description;
            costText.text = $"Mana: {ability.manaCost}";

            if (ability.icon != null)
            {
                iconImage.sprite = ability.icon;
                iconImage.color = Color.white; 
            }
            else
            {
                iconImage.color = new Color(1, 1, 1, 0); 
            }

            gameObject.SetActive(true); 
        }

        public void HideTooltip()
        {
            gameObject.SetActive(false); 
        }
    }
}