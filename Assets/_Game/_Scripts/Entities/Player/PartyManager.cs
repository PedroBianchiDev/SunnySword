using UnityEngine;
using System.Collections.Generic;
using SunnySword.UI;
using SunnySword.Player; 

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance { get; private set; }

    [Header("Configurações")]
    public GameObject playerObject;
    public List<GameObject> activeParty = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (playerObject != null && !activeParty.Contains(playerObject))
        {
            activeParty.Add(playerObject);
        }

        UpdateHUD();
    }

    public void AddMember(GameObject memberSource)
    {
        if (memberSource == null) return;

        GameObject finalMember = memberSource;

        if (memberSource.scene.name == null)
        {
            finalMember = Instantiate(memberSource, playerObject.transform.position, Quaternion.identity);
        }

        if (!activeParty.Contains(finalMember))
        {
            activeParty.Add(finalMember);

            if (finalMember.TryGetComponent<PartyMemberFollower>(out var follower))
            {
                follower.playerTransform = playerObject.transform;
                follower.enabled = true;
            }

            Debug.Log($"[Party] {finalMember.name} adicionado com sucesso.");
            UpdateHUD();
        }
    }

    public void RemoveMember(GameObject member)
    {
        if (activeParty.Contains(member))
        {
            activeParty.Remove(member);
            UpdateHUD();
        }
    }

    public void UpdateHUD()
    {
        if (PartyHUDManager.Instance != null)
        {
            List<GameObject> soAliados = new List<GameObject>(activeParty);

            if (playerObject != null) soAliados.Remove(playerObject);

            PartyHUDManager.Instance.RefreshHUD(soAliados);
        }
    }
}