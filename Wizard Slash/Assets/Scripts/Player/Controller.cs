using UnityEngine;
using ExtensionMethods;

namespace Player
{
    [RequireComponent(typeof(AttackTimer))]
    [RequireComponent(typeof(StateManager))]
    public class Controller : Actor
    {
        public PlayerInput PlayerInput = new PlayerInput();
        public AttackTimer attackTimer;
        
        private StateManager stateManager;
        private const int bufferFrames = 1;

        void Start()
        {
            attackTimer = GetComponent<AttackTimer>();
            stateManager = GetComponent<StateManager>();
        }

        void HandleInput(PlayerInput.Action action)
        {
            if (stateManager.CanAttack)
            {
                stateManager.PlayerState.HandleInput(action);
            }
            else if (attackTimer.Timer >= (stateManager.SelectedMove.recoverFrame - bufferFrames).ToTime()
                     && attackTimer.Timer <= stateManager.SelectedMove.recoverFrame.ToTime())
            {   
                // Attack buffering
                attackTimer.QueueMove(action);
            }
        }

        // Manages basic horizontal movement
        void FixedUpdate()
        {
            if ((!stateManager.CanMove || attackTimer.IsTiming) && !stateManager.SelectedMove.movementAllowed)
                return;
            
            Run(PlayerInput.Horizontal);
            if (attackTimer.Timer <= float.Epsilon)
                stateManager.CheckDirection(PlayerInput.Horizontal);
        }

        // Manages single-input key presses
        void Update()
        {
            if      (PlayerInput.Jump)
                HandleInput(PlayerInput.Action.Jump);
            else if (PlayerInput.Melee)
                HandleInput(PlayerInput.Action.Melee);
            else if (PlayerInput.Magic)
                HandleInput(PlayerInput.Action.Magic);
            else if (PlayerInput.Utility)
                HandleInput(PlayerInput.Action.Utility);
        }
    }
}