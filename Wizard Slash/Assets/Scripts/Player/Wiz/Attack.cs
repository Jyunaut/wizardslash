using UnityEngine;
using ExtensionMethods;

namespace Player.Wiz
{
    public class Attack : WizState
    {
        private bool isPushing = false;
        private bool initialPush = true;
        
        public Attack(StateManager stateManager, string stateName) : base(stateManager, stateManager.SelectedMove.Name) {}

        public override void EnterState()
        {
            stateManager.Animator.Play(stateManager.SelectedMove.Name);
            isPushing = true;
            initialPush = true;
            Debug.Log(stateManager.SelectedMove.Name);
        }

        public override void DoStateBehaviour()
        {
            if (isPushing && stateManager.SelectedMove.pushPlayer)
                PushPlayer();           
        }

        public override void Transitions()
        {
            if (stateManager.Controller.attackTimer.IsTiming)
                return;

            if      (Idle()) {}
            else if (Run())  {}
            else if (Fall()) {}
        }

        void PushPlayer()
        {
            int direction = stateManager.IsFacingRight ? 1 : -1;
            float pushEndTime = Time.time + stateManager.SelectedMove.pushFrames.ToTime();

            if (initialPush)
            {
                // Initial push of the attack
                stateManager.Controller.rigidbody2d.gravityScale = 0;
                stateManager.Controller.rigidbody2d.velocity = new Vector2(direction * stateManager.SelectedMove.pushX, stateManager.SelectedMove.pushY);
                initialPush = false;
            }
            else if (Time.time >= pushEndTime - stateManager.SelectedMove.pushFrames.ToTime() / 2f && Time.time <= pushEndTime)
            {
                // Deccelerate the player during the last half of the push event
                stateManager.Controller.rigidbody2d.velocity = new Vector2(stateManager.Controller.rigidbody2d.velocity.x * stateManager.SelectedMove.decceleration,
                                                                           stateManager.Controller.rigidbody2d.velocity.y);
            }
            else if (Time.time >= pushEndTime)
            {
                // Stop the push after the end time
                stateManager.Controller.rigidbody2d.gravityScale = 1;
                stateManager.Controller.rigidbody2d.velocity = new Vector2(0, stateManager.Controller.rigidbody2d.velocity.y);
                isPushing = false;
            }
        }
    }
}