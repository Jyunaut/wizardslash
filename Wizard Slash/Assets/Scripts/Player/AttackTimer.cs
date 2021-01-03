using UnityEngine;
using ExtensionMethods;

namespace Player
{
    public class AttackTimer : MonoBehaviour
    {
        public const int FPS = 18;
        public float Timer { get; private set; } = 0f;
        public bool IsTiming { get; private set; }

        private Controller controller;
        private StateManager stateManager;
        private PlayerInput.Action action;
        private bool isQueued = false;

        void Start()
        {
            controller = GetComponent<Controller>();
            stateManager = GetComponent<StateManager>();
        }

        public void QueueMove(PlayerInput.Action action)
        {
            isQueued = true;
            this.action = action;
        }

        public void StartMove()
        {
            Timer = 0f;
            IsTiming = true;

            stateManager.DisableAttacks();
            stateManager.DisableMovement();
            stateManager.SetInCombat();
            stateManager.CheckDirection(controller.PlayerInput.Horizontal);
        }

        public void EndMove()
        {
            Timer = 0f;
            IsTiming = false;
            
            stateManager.EnableAttacks();
            stateManager.EnableMovement();
            controller.rigidbody2d.gravityScale = 1;
        }

        // Each player move goes through a timeline where windup -> hit -> recover
        // and allows the player to be pushed throughout a portion of the timeline
        void TimeAttack()
        {
            if (Timer <= stateManager.SelectedMove.hitFrame.ToTime())
            {
                // Windup
                stateManager.DisableAttacks();
                stateManager.DisableMovement();
            }
            else if (Timer >= stateManager.SelectedMove.hitFrame.ToTime()
                     && Timer <= stateManager.SelectedMove.recoverFrame.ToTime())
            {
                // Hit
            }
            else if (Timer >= stateManager.SelectedMove.recoverFrame.ToTime()
                     && Timer <= stateManager.SelectedMove.totalFrames.ToTime())
            {
                // Recover
                stateManager.EnableAttacks();
                stateManager.EnableMovement();
                
                // Execute next attack if there is a queued attack
                if (isQueued && Timer >= (stateManager.SelectedMove.recoverFrame + 1).ToTime())
                {
                    string previousAction = stateManager.PlayerStateName;
                    stateManager.PlayerState.HandleInput(action);
                    isQueued = false;
                }
            }
            else
            {
                // End combo
                EndMove();
            }
        }
        
        void Update()
        {
            if (!IsTiming)
                return;
                        
            TimeAttack();

            if (IsTiming)
                Timer += Time.deltaTime;
        }
    }
}