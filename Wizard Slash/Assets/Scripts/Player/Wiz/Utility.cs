namespace Player.Wiz
{
    class Dash : Attack
    {
        public Dash(StateManager stateManager) : base(stateManager, "Dash") {}

        public override void EnterState()
        {
            UnityEngine.Debug.Log("wa");
            stateManager.animator.Play(stateName, 0, 0f);
            base.EnterState();
        }
    }

    class AirDash : Attack
    {
        public AirDash(StateManager stateManager) : base(stateManager, "AirDash") {}
    }
}
