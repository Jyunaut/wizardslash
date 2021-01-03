using UnityEngine;

namespace Player.Wiz
{
    class Idle : WizState
    {
        public Idle(StateManager stateManager) : base(stateManager, "Idle") {}

        public override void DoStateBehaviour()
        {
            if (stateManager.IsInCombat)
                stateManager.Animator.Play(stateName + "Combat");
            else
                stateManager.Animator.Play(stateName);
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
            if (stateManager.IsInCombat)
                stateManager.Animator.Play(stateName + "Combat");
            else
                stateManager.Animator.Play(stateName);
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
            if (stateManager.IsInCombat)
            {
                stateManager.Animator.Play(stateName + "Combat");
                stateEndTime = Time.time + 0.300f;
            }
            else
            {
                stateManager.Animator.Play(stateName);
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
            stateManager.Animator.Play(stateName);
            stateManager.Controller.rigidbody2d.velocity = new Vector2(Mathf.Clamp(stateManager.Controller.rigidbody2d.velocity.x,
                                                                                  -stateManager.Controller.moveSpeed, stateManager.Controller.moveSpeed),
                                                                       stateManager.Controller.jumpForce);
            stateManager.Controller.attackTimer.EndMove();
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
            stateManager.Animator.Play(stateName);
        }

        public override void Transitions()
        {
            Idle();
            Run();
        }
    }
}