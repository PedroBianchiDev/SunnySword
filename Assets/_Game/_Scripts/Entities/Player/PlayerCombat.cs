using UnityEngine;
using SunnySword.Stats;
using SunnySword.Abilities;
using SunnySword.Combat; 

namespace SunnySword.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        private EntityCombat entityCombat;
        private StatsHandler statsHandler;

        public LayerMask enemyLayer;

        [Header("Configurações de Ataque")]
        public AttackAbilityData basicAttack;

        private void Awake()
        {
            statsHandler = GetComponent<StatsHandler>();
            entityCombat = GetComponent<EntityCombat>();

            if (entityCombat == null)
            {
                Debug.LogError($"EntityCombat não encontrado no {gameObject.name}! Adicione o componente no Inspector.");
            }
        }

        public void PerformAttack(Vector2 attackDirection, Sprite[] animSprites, int hitFrame)
        {
            if (basicAttack != null && entityCombat != null)
            {
                Vector2 targetPos = (Vector2)transform.position + attackDirection;

                entityCombat.Attack(basicAttack, targetPos, animSprites, hitFrame);
            }
        }

        public bool IsAttacking => entityCombat != null && entityCombat.IsAttacking;
    }
}