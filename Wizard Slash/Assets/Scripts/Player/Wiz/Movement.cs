using UnityEngine;

namespace Player.Wiz
{
    class Idle : WizState
    {
        public Idle(StateManager stateManager) : base(stateManager, "Idle") {}

        public override void DoStateBehaviour()
        {
            if (stateManager.isInCombat)
                stateManager.animator.Play(stateName + "Combat");
            else
                stateManager.animator.Play(stateName);
        }

        public override void Transitions()
        {
            if      (Run())  {}
            else if (Fall()) {}
        }
    }

    class Run : WizState
    {
        public Run(StateManager stateManager) : base(stateManager, "Run") {}

        public override void DoStateBehaviour()
        {
            if (stateManager.isInCombat)
                stateManager.animator.Play(stateName + "Combat");
            else
                stateManager.animator.Play(stateName);
        }

        public override void Transitions()
        {
            if      (RunBrake()) {}
            else if (Idle())     {}
            else if (Fall())     {}
        }
    }

    class RunBrake : WizState
    {
        public RunBrake(StateManager stateManager) : base(stateManager, "RunBrake") {}

        private float stateEndTime;

        public override void EnterState()
        {
            if (stateManager.isInCombat)
            {
                stateManager.animator.Play(stateName + "Combat");
                stateEndTime = Time.time + 0.300f;
            }
            else
            {
                stateManager.animator.Play(stateName);
                stateEndTime = Time.time + 0.278f;
            }
        }

        public override void Transitions()
        {
            if (Time.time >= stateEndTime)
            {
                Idle(); return;
            }

            if      (Run())  {}    
            else if (Jump()) {}
            else if (Fall()) {}
        }
    }

    class Jump : WizState
    {
        public Jump(StateManager stateManager) : base(stateManager, "Jump") {}

        public override void EnterState()
        {
            stateManager.animator.Play(stateName);
            stateManager.controller.rigidbody2d.velocity = new Vector2(Mathf.Clamp(stateManager.controller.rigidbody2d.velocity.x,
                                                                                  -stateManager.controller.moveSpeed, stateManager.controller.moveSpeed),
                                                                       stateManager.controller.jumpForce);
            stateManager.controller.attackTimer.ResetTimerAndValues();
        }

        public override void Transitions()
        {
            Fall();
        }
    }

    class Fall : WizState
    {
        public Fall(StateManager stateManager) : base(stateManager, "Fall") {}

        public override void DoStateBehaviour()
        {
            stateManager.animator.Play(stateName);
        }

        public override void Transitions()
        {
            Idle();
            Run();
        }
    }
}

// using UnityEngine;

// namespace Player
// {
//     namespace Wiz
//     {
//         class Movement : State
//         {
//             public Movement(StateManager stateManager) : base(stateManager) {}


//             protected void Land()
//             {

//             }

//             protected void Turn()
//             {

//             }

//             protected void Damaged()
//             {

//             }

//             protected void OutOfCombatTransition()
//             {

//             }
//         }

//         class Idle : Movement
//         {
//             public Idle(StateManager stateManager) : base(stateManager) {}

//             public override void EnterState()
//             {
//                 if (stateManager.inCombat)
//                     stateManager.animator.Play("Idle Combat");
//                 else
//                     stateManager.animator.Play("Idle");
//             }

//             public override void Transitions()
//             {
//                 Run();
//                 Jump();
//                 Fall();
//             }
//         }

//         class Run : Movement
//         {
//             public Run(StateManager stateManager) : base(stateManager) {}

//             public override void EnterState()
//             {
//                 if (stateManager.inCombat)
//                     stateManager.animator.Play("Run Combat");
//                 else    
//                     stateManager.animator.Play("Run");
//             }

//             public override void Transitions()
//             {
//                 RunBrake();
//                 Jump();
//                 Fall();
//             }
//         }

//         class RunBrake : Movement
//         {
//             public RunBrake(StateManager stateManager) : base(stateManager) {}

//             private float stateEndTime;

//             public override void EnterState()
//             {
//                 if (stateManager.inCombat)
//                     stateManager.animator.Play("RunBrake Combat");
//                 else
//                     stateManager.animator.Play("RunBrake");

//                 stateEndTime = Time.time + 0.333f;
//             }

//             public override void Transitions()
//             {
//                 if (Time.time >= stateEndTime)
//                     Idle();

//                 Run();    
//                 Jump();
//                 Fall();
//             }
//         }

//         class Jump : Movement
//         {
//             public Jump(StateManager stateManager) : base(stateManager) {}

//             public override void EnterState()
//             {
//                 stateManager.animator.Play("Jump");
//             }

//             public override void Transitions()
//             {
//                 Fall();
//             }
//         }

//         class Fall : Movement
//         {
//             public Fall(StateManager stateManager) : base(stateManager) {}

//             public override void EnterState()
//             {
//                 stateManager.animator.Play("Fall");
//             }

//             public override void Transitions()
//             {
//                 Idle();
//                 Run();
//             }
//         }
//         // TODO: Implement Land State
//         class Land : Movement
//         {
//             public Land(StateManager stateManager) : base(stateManager) {}

//             public override void EnterState()
//             {

//             }

//             public override void Transitions()
//             {

//             }
//         }
//         // TODO: Implement Turn State
//         class Turn : Movement
//         {
//             public Turn(StateManager stateManager) : base(stateManager) {}

//             public override void EnterState()
//             {

//             }

//             public override void Transitions()
//             {
                
//             }
//         }
//         // TODO: Implement Damaged State
//         class Damaged : Movement
//         {
//             public Damaged(StateManager stateManager) : base(stateManager) {}

//             public override void EnterState()
//             {

//             }

//             public override void Transitions()
//             {
                
//             }
//         }
//         // TODO: Implement OutOfCombatTransition State
//         class OutOfCombatTransition : Movement
//         {
//             public OutOfCombatTransition(StateManager stateManager) : base(stateManager) {}

//             public override void EnterState()
//             {

//             }

//             public override void Transitions()
//             {
                
//             }
//         }
//     }
// }