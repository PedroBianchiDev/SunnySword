using UnityEngine;

namespace SunnySword.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Painéis")]
        public GameObject skillTreePanel; 

        private void Start()
        {
            if (skillTreePanel != null)
            {
                skillTreePanel.SetActive(false);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                ToggleSkillTree();
            }
        }

        private void ToggleSkillTree()
        {
            if (skillTreePanel != null)
            {
                bool isCurrentlyOpen = skillTreePanel.activeSelf;
                skillTreePanel.SetActive(!isCurrentlyOpen);
            }
        }
    }
}