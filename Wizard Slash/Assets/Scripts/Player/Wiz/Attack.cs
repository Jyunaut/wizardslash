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

            Idle();
            IdleCombat();
            Run();
            RunCombat();
            Fall();
        }
    }
}

// namespace Player
// {
//     namespace Wiz
//     {
//         abstract class Attack : State
//         {
//             public Attack(StateManager stateManager) : base(stateManager) {}

//             public void Attacks()
//             {
//                 if (stateManager.animator.HasState(0, Animator.StringToHash(stateManager.controller.currentAction))
//                     && !stateManager.animator.GetCurrentAnimatorStateInfo(0).IsName(stateManager.controller.currentAction))
//                 {
//                     Type.GetType(this.ToString()).InvokeMember(stateManager.selectedMove.Name, BindingFlags.InvokeMethod, null, this, null);
//                 }
//             }

//             public override void EnterState()
//             {
//                 stateManager.animator.Play(stateManager.selectedMove.Name);
//             }
            
//             public override void Transitions()
//             {
//                 Jump();
//                 if (!stateManager.animator.GetCurrentAnimatorStateInfo(0).IsName(stateManager.selectedMove.Name))
//                 {
//                     switch (stateManager.controller.currentAction)
//                     {
//                         case "Neutral":
//                             Idle();
//                             Run();
//                             break;
//                         case "AirNeutral":
//                             Fall();
//                             break;
//                     }
//                 }
//             }
//         }
//     }
// }