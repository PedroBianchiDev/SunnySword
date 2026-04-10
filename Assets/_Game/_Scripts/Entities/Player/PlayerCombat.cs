using UnityEngine;
using SunnySword.Stats;
using SunnySword.Abilities;

namespace SunnySword.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Referências")]
        public StatsHandler statsHandler;

        [Tooltip("Arraste aqui a habilidade do seu Clique do Mouse")]
        public AttackAbilityData basicAttack;

        [Tooltip("Arraste aqui a habilidade da tecla E")]
        public AttackAbilityData currentSkill;

        [Header("Teclas")]
        public KeyCode skillKey = KeyCode.E;

        public bool IsAttacking { get; set; }

        private void Update()
        {
            if (statsHandler == null) return;

            HandleSkill();
        }

        public void PerformAttack(Vector2 attackDirection, float duration)
        {
            if (IsAttacking) return;

            IsAttacking = true;

            if (basicAttack != null)
            {
                Vector2 targetPos = (Vector2)transform.position + attackDirection;
                basicAttack.Execute(this.gameObject, targetPos);
            }

            Invoke(nameof(ResetAttackState), duration);
        }

        private void ResetAttackState()
        {
            IsAttacking = false;
        }

        private void HandleSkill()
        {
            if (Input.GetKeyDown(skillKey) && !IsAttacking)
            {
                if (statsHandler.CurrentMana >= currentSkill.manaCost)
                {
                    statsHandler.UseMana(currentSkill.manaCost);
                    currentSkill.Execute(this.gameObject, transform.position);
                }
            }
        }
    }
}