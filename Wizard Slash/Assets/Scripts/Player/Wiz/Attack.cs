using System;
using System.Reflection;
using UnityEngine;

namespace Player.Wiz
{
    public class Attack : WizState
    {
        public Attack(StateManager stateManager, string stateName) : base(stateManager, stateManager.selectedMove.Name) {}

        public override void EnterState()
        {
            stateManager.animator.Play(stateManager.selectedMove.Name);
        }

        public override void Transitions()
        {
            if (stateManager.controller.attackTimer.isTiming)
                return;

            if      (Idle()) {}
            else if (Run())  {}
            else if (Fall()) {}
        }
    }
}