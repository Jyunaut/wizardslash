namespace Player.Wiz
{
    class FireFistLight1 : Attack
    {
        public FireFistLight1(StateManager stateManager) : base(stateManager, "FireFistLight1") {}
    }
    
    // TODO: Consider improving the game-feel of this rapid-punch
    class FireFistLight2 : Attack
    {
        private float rapidPunchTime = 0.333f;

        public FireFistLight2(StateManager stateManager) : base(stateManager, "FireFistLight2") {}

        public override void EnterState()
        {
            rapidPunchTime = 0.333f;
            stateManager.Animator.Play(stateName, 0, 0f);
            base.EnterState();
        }

        public override void DoStateBehaviour()
        {
            rapidPunchTime -= UnityEngine.Time.deltaTime;
        }

        public override void HandleInput(PlayerInput.Action action)
        {
            if (rapidPunchTime > 0f && action == PlayerInput.Action.Magic)
                Attacks(stateName);
            else
                base.HandleInput(action);
        }
    }

    class FireFistHeavy : Attack
    {
        public FireFistHeavy(StateManager stateManager) : base(stateManager, "FireFistHeavy") {}
    }

    class FireFistUppercut : Attack
    {
        public FireFistUppercut(StateManager stateManager) : base(stateManager, "FireFistUppercut") {}
    }

    class FireFistAirUppercut : Attack
    {
        public FireFistAirUppercut(StateManager stateManager) : base(stateManager, "FireFistAirUppercut") {}
    }
}
