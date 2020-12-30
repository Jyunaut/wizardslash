namespace Player.Wiz
{
    class Basic1 : Attack
    {
        public Basic1(StateManager stateManager) : base(stateManager, "Basic1") {}
    }

    class Basic2 : Attack
    {
        public Basic2(StateManager stateManager) : base(stateManager, "Basic2") {}
    }

    class Basic3 : Attack
    {
        public Basic3(StateManager stateManager) : base(stateManager, "Basic3") {}
    }

    class AirBasic1 : Attack
    {
        public AirBasic1(StateManager stateManager) : base(stateManager, "AirBasic1") {}
    }

    class AirBasic2 : Attack
    {
        public AirBasic2(StateManager stateManager) : base(stateManager, "AirBasic2") {}
    }

    class AirBasic3 : Attack
    {
        public AirBasic3(StateManager stateManager) : base(stateManager, "AirBasic3") {}
    }
}

// namespace Player
// {
//     namespace Wiz
//     {
//         class Basic1 : Attack
//         {
//             public Basic1(StateManager stateManager) : base(stateManager) {}

//             public override void EnterState()
//             {
//                 base.EnterState();
//                 UnityEngine.Debug.Log("Enter Basic 1");
//             }

//             public override void Transitions()
//             {
//                 base.Transitions();
//                 if (!stateManager.controller.playerInput.Melee) return;
//                 foreach (string transition in stateManager.selectedMove.canTransitionTo)
//                 {
//                     //Attack();
//                 }
//             }
//         }

//         class Basic2 : Attack
//         {
//             public Basic2(StateManager stateManager) : base(stateManager) {}

//             public override void EnterState()
//             {
//                 base.EnterState();
//                 UnityEngine.Debug.Log("Enter Basic 2");
//             }
            
//             public override void Transitions()
//             {
//                 base.Transitions();
//                 if (!stateManager.controller.playerInput.Melee) return;
//                 foreach (string transition in stateManager.selectedMove.canTransitionTo)
//                 {
//                     //Attack();
//                 }
//             }
//         }

//         class Basic3 : Attack
//         {
//             public Basic3(StateManager stateManager) : base(stateManager) {}

//             public override void EnterState()
//             {
//                 base.EnterState();
//                 UnityEngine.Debug.Log("Enter Basic 3");
//             }
            
//             public override void Transitions()
//             {
//                 base.Transitions();
//                 if (!stateManager.controller.playerInput.Melee) return;
//                 foreach (string transition in stateManager.selectedMove.canTransitionTo)
//                 {
//                     //Attack();
//                 }
//             }
//         }

//         class AirBasic1 : Attack
//         {
//             public AirBasic1(StateManager stateManager) : base(stateManager) {}

//             public override void EnterState()
//             {
//                 base.EnterState();
//                 UnityEngine.Debug.Log("Enter Air Basic 1");
//             }
//         }

//         class AirBasic2 : Attack
//         {
//             public AirBasic2(StateManager stateManager) : base(stateManager) {}

//             public override void EnterState()
//             {
//                 base.EnterState();
//             }
//         }

//         class AirBasic3 : Attack
//         {
//             public AirBasic3(StateManager stateManager) : base(stateManager) {}

//             public override void EnterState()
//             {
//                 base.EnterState();
//             }
//         }

//         class DashSlash : Attack
//         {
//             public DashSlash(StateManager stateManager) : base(stateManager) {}

//             public override void EnterState()
//             {
//                 base.EnterState();
//             }
//         }

//         class RisingSlash : Attack
//         {
//             public RisingSlash(StateManager stateManager) : base(stateManager) {}

//             public override void EnterState()
//             {
//                 base.EnterState();
//             }
//         }
//     }
// }
