using UnityEngine;

namespace SunnySword.Player
{
    public class PlayerShield : MonoBehaviour
    {
        public bool IsBlocking { get; private set; }

        public void SetBlocking(bool blocking)
        {
            IsBlocking = blocking;
        }
    }
}