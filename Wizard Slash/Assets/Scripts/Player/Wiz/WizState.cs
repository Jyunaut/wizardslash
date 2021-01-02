using System;
using System.Reflection;
using UnityEngine;

namespace Player.Wiz
{
    public class WizState : PlayerState
    {
        public WizState(StateManager stateManager, string stateName) : base(stateManager, stateName) {}

        public override void HandleInput(PlayerInput.Action action)
        {
            if (action == PlayerInput.Action.Jump)
                Jump();
            else
            {   // TODO: Rename "ChooseMove" to something better
                bool canAttack = stateManager.ChooseMove(action);
                if (canAttack)
                    Attacks(stateManager.selectedMove);
            }
        }

        protected void Attacks(Move move)
        {
            stateManager.controller.attackTimer.StartMove();
            Type.GetType(this.ToString()).InvokeMember(move.Name, BindingFlags.InvokeMethod, null, this, null);
        }

        protected void Attacks(string attackName)
        {
            stateManager.controller.attackTimer.StartMove();
            Type.GetType(this.ToString()).InvokeMember(attackName, BindingFlags.InvokeMethod, null, this, null);
        }

        #region Transition Conditions
        protected bool Idle()
        {
            if (stateManager.controller.playerInput.Horizontal == 0
                && stateManager.onGround)
            {
                stateManager.SetState(new Idle(stateManager)); return true;
            }
            return false;
        }

        protected bool Run()
        {
            if (stateManager.controller.playerInput.Horizontal != 0
                && Mathf.Abs(stateManager.controller.rigidbody2d.velocity.x) > 0
                && stateManager.onGround)
            {
                stateManager.SetState(new Run(stateManager)); return true;
            }
            return false;
        }

        protected bool RunBrake()
        {
            if (stateManager.controller.playerInput.Horizontal == 0
                && stateManager.onGround)
            {
                stateManager.SetState(new RunBrake(stateManager)); return true;
            }
            return false;
        }

        protected bool Jump()
        {
            if (stateManager.controller.playerInput.Jump
                && stateManager.canMove
                && stateManager.onGround)
            {
                stateManager.SetState(new Jump(stateManager)); return true;
            }
            return false;
        }

        protected bool Fall()
        {
            if (stateManager.controller.rigidbody2d.velocity.y <= 0
                && !stateManager.onGround)
            {
                stateManager.SetState(new Fall(stateManager)); return true;
            }
            return false;
        }
        #endregion

        #region Melee
        public void Basic1()      => stateManager.SetState(new Basic1(stateManager));
        public void Basic2()      => stateManager.SetState(new Basic2(stateManager));
        public void Basic3()      => stateManager.SetState(new Basic3(stateManager));
        public void AirBasic1()   => stateManager.SetState(new AirBasic1(stateManager));
        public void AirBasic2()   => stateManager.SetState(new AirBasic2(stateManager));
        public void AirBasic3()   => stateManager.SetState(new AirBasic3(stateManager));
        public void DashSlash()   => stateManager.SetState(new DashSlash(stateManager));
        public void RisingSlash() => stateManager.SetState(new RisingSlash(stateManager));
        #endregion

        #region Utility
        public void Dash()    => stateManager.SetState(new Dash(stateManager));
        public void AirDash() => stateManager.SetState(new AirDash(stateManager));
        #endregion

        #region Fire Fist
        public void FireFistLight1()      => stateManager.SetState(new FireFistLight1(stateManager));
        public void FireFistLight2()      => stateManager.SetState(new FireFistLight2(stateManager));
        public void FireFistHeavy()       => stateManager.SetState(new FireFistHeavy(stateManager));
        public void FireFistUppercut()    => stateManager.SetState(new FireFistUppercut(stateManager));
        public void FireFistAirUppercut() => stateManager.SetState(new FireFistAirUppercut(stateManager));
        #endregion
    }
}