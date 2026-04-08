using UnityEngine;
using SunnySword.Stats;

namespace SunnySword.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMovement : MonoBehaviour
    {
        private new Rigidbody2D rigidbody;
        private StatsHandler statsHandler;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            statsHandler = GetComponent<StatsHandler>();
        }

        public void ProcessMove(Vector2 direction)
        {
            Vector2 velocity = direction.normalized * statsHandler.Data.baseMoveSpeed;
            rigidbody.linearVelocity = velocity;
        }
    }
}
