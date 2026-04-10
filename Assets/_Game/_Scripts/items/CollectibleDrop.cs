using UnityEngine;
using SunnySword.Stats;

namespace SunnySword.Items
{
    [RequireComponent(typeof(Collider2D))]
    public class CollectibleDrop : MonoBehaviour
    {
        public enum DropType { Health, Mana, ExpOrb }

        [Header("Configuração do Item")]
        public DropType type;
        public float amount = 20f;

        [Header("Efeitos")]
        public GameObject pickupEffect; 

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent<StatsHandler>(out StatsHandler stats))
                {
                    switch (type)
                    {
                        case DropType.Health:
                            stats.Heal(amount);
                            break;
                        case DropType.Mana:
                            break;
                        case DropType.ExpOrb:
                            stats.AddExp(amount);
                            break;
                    }

                    if (pickupEffect != null) Instantiate(pickupEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
        }
    }
}