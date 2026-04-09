using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SunnySword.UI
{
    public class TooltipManager : MonoBehaviour
    {
        public static TooltipManager Instance { get; private set; }

        [Header("Referências da UI")]
        public GameObject tooltipWindow; 
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI costText;
        public Image iconImage;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            HideTooltip(); 
        }

        private void Update()
        {
            if (tooltipWindow.activeSelf)
            {
                transform.position = Input.mousePosition + new Vector3(15f, -15f, 0f);
            }
        }

        public void ShowTooltip(SkillsTree.SkillNodeData nodeData)
        {
            nameText.text = nodeData.nodeName;
            descriptionText.text = nodeData.description;
            costText.text = $"Custo: {nodeData.costInPoints}P";

            if (nodeData.nodeIcon != null)
            {
                iconImage.sprite = nodeData.nodeIcon;
                iconImage.gameObject.SetActive(true);
            }
            else
            {
                iconImage.gameObject.SetActive(false); 
            }

            tooltipWindow.SetActive(true);
        }

        public void HideTooltip()
        {
            tooltipWindow.SetActive(false);
        }
    }
}