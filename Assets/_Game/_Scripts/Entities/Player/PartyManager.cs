using UnityEngine;
using System.Collections.Generic;
using SunnySword.Stats;

namespace SunnySword.Player
{
    public class PartyManager : MonoBehaviour
    {
        [Header("Configuração do Grupo")]
        public List<CharacterStatsData> partyMembers = new List<CharacterStatsData>();
        public int activeMemberIndex = 0;

        [Header("Referências")]
        private StatsHandler playerStats;
        private PlayerCombat playerCombat;

        private void Awake()
        {
            playerStats = GetComponent<StatsHandler>();
            playerCombat = GetComponent<PlayerCombat>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchMember(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchMember(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchMember(2);
        }

        public void SwitchMember(int index)
        {
            if (index < 0 || index >= partyMembers.Count || index == activeMemberIndex) return;

            activeMemberIndex = index;
            CharacterStatsData nextMember = partyMembers[activeMemberIndex];

            playerStats.LoadCharacterData(nextMember);

            Debug.Log($"[PARTY] Trocado para: {nextMember.name}");

            playerStats.TriggerStatsUpdate();
        }
    }
}