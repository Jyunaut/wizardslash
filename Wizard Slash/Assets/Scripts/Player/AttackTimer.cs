using UnityEngine;

namespace Player
{
    public class AttackTimer : MonoBehaviour
    {
        public const float FPS = 18f;
        public float timer = 0f;
        public float timerInFrames = 0f;
        public bool isQueued = false;
        public bool isTiming;
        public bool isPushing;
        public bool initialPush;
        public PlayerInput.Action action;

        private Controller controller;
        private StateManager stateManager;

        public void StartMove()
        {
            timer = 0;
            isTiming = true;
            isPushing = false;
            initialPush = true;
            stateManager.canAttack = false;
            stateManager.canMove = false;
            stateManager.isAttacking = true;
            stateManager.SetInCombat();
            stateManager.CheckDirection(controller.playerInput.Horizontal);
        }

        void ResetTimerAndValues()
        {
            isTiming = false;
            timer = 0f;
            timerInFrames = 0f;
            
            stateManager.canAttack = true;
            stateManager.canMove = true;
            stateManager.isAttacking = false;
            isPushing = false;
            controller.rigidbody2d.gravityScale = 1;
        }

        // Each player move goes through a timeline where windup -> hit -> recover
        // and allows the player to be pushed throughout a portion of the timeline
        void TimeAttack()
        {
            if (timer <= stateManager.selectedMove.hitFrame / FPS)
            {
                // Windup
                stateManager.canAttack = false;
                stateManager.canMove = false;
            }
            else if (timer >= stateManager.selectedMove.hitFrame / FPS
                && timer <= stateManager.selectedMove.recoverFrame / FPS)
            {
                // Hit
                isPushing = true;
            }
            else if (timer >= stateManager.selectedMove.recoverFrame / FPS
                && timer <= stateManager.selectedMove.totalFrames / FPS)
            {
                // Recover
                stateManager.canAttack = true;
                stateManager.canMove = true;
                isPushing = true;
                
                // Execute next attack if there is a queued attack
                if (isQueued && timer >= (stateManager.selectedMove.recoverFrame + 1) / FPS)
                {
                    string previousAction = stateManager.playerStateName;
                    stateManager.playerState.HandleInput(action);
                    isQueued = false;
                }
            }
            else
            {
                // End combo
                ResetTimerAndValues();
            }
        }
        
        // Push player in a specified direction depending on the executed move
        private float pushEndTime;
        void PushPlayer()
        {
            int direction = stateManager.facingRight ? 1 : -1;

            if (initialPush)
            {
                // Initial push of the attack
                controller.rigidbody2d.gravityScale = 0;
                controller.rigidbody2d.velocity = new Vector2(direction * stateManager.selectedMove.pushX, stateManager.selectedMove.pushY);
                pushEndTime = Time.time + stateManager.selectedMove.pushFrames / FPS;
                initialPush = false;
            }
            else if (Time.time >= pushEndTime - stateManager.selectedMove.pushFrames / FPS / 2f && Time.time <= pushEndTime)
            {
                // Deccelerate the player during the last half of the push event
                controller.rigidbody2d.velocity = new Vector2(controller.rigidbody2d.velocity.x * stateManager.selectedMove.decceleration,
                                                                    controller.rigidbody2d.velocity.y);
            }
            else if (Time.time >= pushEndTime)
            {
                // Stop the push after the end time
                controller.rigidbody2d.gravityScale = 1;
                controller.rigidbody2d.velocity = new Vector2(0, controller.rigidbody2d.velocity.y);
                isPushing = false;
            }
        }

        void Start()
        {
            controller = GetComponent<Controller>();
            stateManager = GetComponent<StateManager>();
        }

        void Update()
        {
            if (!isTiming)
                return;
            
            if (isPushing && stateManager.selectedMove.pushPlayer)
                PushPlayer();
            
            TimeAttack();
            if (isTiming)
            {
                timer += Time.deltaTime;
                timerInFrames = timer * FPS;
            }
        }
    }
}