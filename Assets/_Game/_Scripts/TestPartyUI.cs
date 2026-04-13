using UnityEngine;
using System.Collections.Generic;
using SunnySword.UI; 

public class TestPartyUI : MonoBehaviour
{
    [Header("Arraste o Player e os Aliados da Cena para cá")]
    public List<GameObject> membrosParaTestar;

    void Start()
    {
        Invoke(nameof(EnviarParaHUD), 0.5f);
    }

    void EnviarParaHUD()
    {
        if (PartyHUDManager.Instance != null)
        {
            PartyHUDManager.Instance.RefreshHUD(membrosParaTestar);
            Debug.Log("HUD Atualizada com sucesso!");
        }
    }
}