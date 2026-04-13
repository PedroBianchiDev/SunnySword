using System;
using UnityEngine;

namespace SunnySword.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; }
        public Vector2 MouseWorldPosition { get; private set; }
        public bool IsShieldHeld { get; private set; }

        public event Action OnAttackPressed;
        public event Action OnInteractPressed;

        public event Action OnSkill1Pressed;
        public event Action OnSkill2Pressed;

        private Camera mainCamera;


        private void Awake()
        {
            mainCamera = Camera.main;
        }


        private void Update()
        {
            MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            MouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetButtonDown("Fire1")) OnAttackPressed?.Invoke();

            IsShieldHeld = Input.GetButton("Fire2");

            if (Input.GetKeyDown(KeyCode.Q)) OnSkill1Pressed?.Invoke();
            if (Input.GetKeyDown(KeyCode.E)) OnSkill2Pressed?.Invoke();

            if (Input.GetKeyDown(KeyCode.F)) OnInteractPressed?.Invoke();

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GetComponent<SkillsTree.PlayerSkillTree>().TryUnlockNode(Resources.Load<SkillsTree.SkillNodeData>("Node_Guerreiro_Cleave"));
            }
        }
    }
}
