using SunnySword.SkillsTree;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems; 
using UnityEngine.UI;

namespace SunnySword.UI
{
    public class SkillNodeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Conexões de Lógica")]
        public SkillNodeData nodeData;
        public PlayerSkillTree playerTree;

        [Header("Elementos Visuais")]
        public Image nodeIcon;
        public TextMeshProUGUI costText;
        private Button buttonComponent;

        [Header("Cores de Estado")]
        public Color lockedColor = new Color(0.3f, 0.3f, 0.3f, 1f);
        public Color unlockedColor = Color.white;
        public Color unaffordableColor = new Color(0.5f, 0.2f, 0.2f, 1f);

        private void Awake()
        {
            buttonComponent = GetComponent<Button>();
            buttonComponent.onClick.AddListener(OnNodeClicked);
        }

        private void Start()
        {
            if (nodeData != null)
            {
                if (costText != null) costText.text = nodeData.costInPoints.ToString();

                if (nodeIcon != null && nodeData.nodeIcon != null)
                {
                    nodeIcon.sprite = nodeData.nodeIcon;
                }
            }
        }

        private void Update()
        {
            UpdateVisualState();
        }

        private void UpdateVisualState()
        {
            if (playerTree == null || nodeData == null) return;

            if (playerTree.unlockedNodes.Contains(nodeData))
            {
                nodeIcon.color = unlockedColor;
                buttonComponent.interactable = false;
            }
            else if (playerTree.availableSkillPoints < nodeData.costInPoints)
            {
                nodeIcon.color = unaffordableColor;
                buttonComponent.interactable = true;
            }
            else
            {
                nodeIcon.color = lockedColor;
                buttonComponent.interactable = true;
            }
        }

        private void OnNodeClicked()
        {
            if (playerTree != null && nodeData != null)
            {
                playerTree.TryUnlockNode(nodeData);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (nodeData != null && TooltipManager.Instance != null)
            {
                TooltipManager.Instance.ShowTooltip(nodeData);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (TooltipManager.Instance != null)
            {
                TooltipManager.Instance.HideTooltip();
            }
        }
    }
}