using UnityEngine;
using System.Collections.Generic;
using SunnySword.Stats;

namespace SunnySword.UI
{
    public class PartyHUDManager : MonoBehaviour
    {
        public static PartyHUDManager Instance { get; private set; }

        [Header("Configurações")]
        public GameObject memberCardPrefab; 
        public Transform container;         

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void RefreshHUD(List<GameObject> currentParty)
        {
            Debug.Log($"[HUD] RefreshHUD chamado! Quantidade de membros na lista: {currentParty.Count}");

            foreach (Transform child in container) Destroy(child.gameObject);

            if (currentParty.Count == 0)
            {
                Debug.LogWarning("[HUD] A lista de membros está VAZIA. Nada será criado.");
                return;
            }

            foreach (GameObject member in currentParty)
            {
                Debug.Log($"[HUD] Tentando criar card para: {member.name}");

                if (member.TryGetComponent<StatsHandler>(out var stats))
                {
                    GameObject newCard = Instantiate(memberCardPrefab, container);
                    newCard.GetComponent<PartyMemberUI>().Setup(stats);
                    Debug.Log($"[HUD] Card criado com sucesso para {member.name}");
                }
                else
                {
                    Debug.LogError($"[HUD] O objeto {member.name} não tem o componente StatsHandler!");
                }
            }
        }
    }
}