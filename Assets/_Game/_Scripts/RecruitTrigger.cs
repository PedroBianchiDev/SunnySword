using UnityEngine;

public class RecruitTrigger : MonoBehaviour
{
    public GameObject allyToRecruit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. O código detectou QUALQUER colisão?
        Debug.Log($"Algo encostou no trigger: {other.name}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("O Player foi detectado!");

            if (PartyManager.Instance != null)
            {
                PartyManager.Instance.AddMember(allyToRecruit);
                Debug.Log("Chamada para AddMember enviada com sucesso.");
                gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("ERRO: O PartyManager não foi encontrado na cena!");
            }
        }
    }
}