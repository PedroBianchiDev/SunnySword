using SunnySword.Player;
using SunnySword.SkillsTree;
using System.Collections.Generic;
using UnityEngine;

namespace SunnySword.SkillsTree
{
    [RequireComponent(typeof(PlayerAbilities))]
    public class PlayerSkillTree : MonoBehaviour
    {
        [Header("Progresso")]
        public int availableSkillPoints = 3; 

        [Tooltip("Lista de nós que já foram desbloqueados")]
        public List<SkillNodeData> unlockedNodes = new List<SkillNodeData>();

        private PlayerAbilities playerAbilities;

        private void Awake()
        {
            playerAbilities = GetComponent<PlayerAbilities>();
        }

        public bool TryUnlockNode(SkillNodeData node)
        {
            if (unlockedNodes.Contains(node))
            {
                Debug.LogWarning("Você já desbloqueou esta habilidade!");
                return false;
            }

            if (availableSkillPoints < node.costInPoints)
            {
                Debug.LogWarning("Pontos de habilidade insuficientes!");
                return false;
            }

            foreach (SkillNodeData req in node.prerequisites)
            {
                if (!unlockedNodes.Contains(req))
                {
                    Debug.LogWarning($"Falta o pré-requisito: {req.nodeName}");
                    return false;
                }
            }

            availableSkillPoints -= node.costInPoints;
            unlockedNodes.Add(node);

            ApplyNodeReward(node);

            Debug.Log($"Nó '{node.nodeName}' desbloqueado com sucesso! Pontos restantes: {availableSkillPoints}");
            return true;
        }

        private void ApplyNodeReward(SkillNodeData node)
        {
            if (node.abilityToUnlock != null)
            {
                if (playerAbilities.skill1 == null)
                {
                    playerAbilities.skill1 = node.abilityToUnlock;
                    Debug.Log($"{node.abilityToUnlock.abilityName} equipada no Slot 1 (Q)!");
                }
                else if (playerAbilities.skill2 == null)
                {
                    playerAbilities.skill2 = node.abilityToUnlock;
                    Debug.Log($"{node.abilityToUnlock.abilityName} equipada no Slot 2 (E)!");
                }
                else
                {
                    Debug.Log("A habilidade foi desbloqueada, mas seus slots estão cheios. (Futuramente faremos a tela de equipar)");
                }
            }
        }
    }
}