using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(AttackTimer))]
    [RequireComponent(typeof(StateManager))]
    public class Controller : Actor
    {
        public PlayerInput playerInput = new PlayerInput();
        public AttackTimer attackTimer;
        
        private StateManager stateManager;

        void HandleInput(PlayerInput.Action action)
        {
            if (stateManager.canAttack)
            {
                stateManager.playerState.HandleInput(action);
            }
            else if (attackTimer.timer >= (stateManager.selectedMove.recoverFrame - 1f) / AttackTimer.FPS
                    && attackTimer.timer <= stateManager.selectedMove.recoverFrame / AttackTimer.FPS)
            {   
                // Attack buffering
                attackTimer.isQueued = true;
                attackTimer.action = action;
            }
        }

        void Start()
        {
            attackTimer = GetComponent<AttackTimer>();
            stateManager = GetComponent<StateManager>();
        }

        // Manages basic horizontal movement
        void FixedUpdate()
        {
            if ((!stateManager.canMove || stateManager.isAttacking) && !stateManager.selectedMove.movementAllowed)
                return;
            
            Run(playerInput.Horizontal);
            if (attackTimer.timer == 0)
                stateManager.CheckDirection(playerInput.Horizontal);
        }

        // Manages single-input key presses
        void Update()
        {
            if (playerInput.Jump)
            {
                HandleInput(PlayerInput.Action.Jump);
            }
            else if (playerInput.Melee)
            {
                HandleInput(PlayerInput.Action.Melee);
            }
            else if (playerInput.Magic)
            {
                HandleInput(PlayerInput.Action.Magic);
            }
            else if (playerInput.Utility)
            {
                HandleInput(PlayerInput.Action.Utility);
            }
        }
    }
}