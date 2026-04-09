using UnityEngine;
using System.Collections.Generic;

namespace SunnySword.SkillsTree
{
    [CreateAssetMenu(menuName = "SunnySword/Skills/Skill Node")]
    public class SkillNodeData : ScriptableObject
    {
        [Header("Informações Base")]
        public string nodeName;
        [TextArea] public string description;
        public Sprite nodeIcon;
        public int costInPoints = 1;

        [Header("Pré-requisitos")]
        [Tooltip("As habilidades que o jogador DEVE ter comprado antes de poder comprar esta.")]
        public List<SkillNodeData> prerequisites;

        [Header("Recompensa")]
        [Tooltip("A habilidade ativa que este nó ensina (deixe vazio se for apenas um bônus passivo)")]
        public Abilities.AbilityData abilityToUnlock;


    }
}