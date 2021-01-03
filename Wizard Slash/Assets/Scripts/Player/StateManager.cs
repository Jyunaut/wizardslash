using UnityEngine;

namespace Player
{
    public class StateManager : MonoBehaviour
    {
        [HideInInspector] public Controller Controller { get; private set; }
        [HideInInspector] public Animator Animator { get; private set; }
        [SerializeField] private Character character;
        public Move SelectedMove { get; private set; }
        public PlayerState PlayerState { get; private set; }
        public string PlayerStateName  { get; private set; }

        // State Conditions
        public bool IsFacingRight { get; private set; } = true;
        public bool CanAttack     { get; private set; } = true;
        public bool CanMove       { get; private set; } = true;
        public bool IsInCombat    { get; private set; } = false;
        public bool IsOnGround    { get; private set; } = false;
        
        [SerializeField] private Moveset[] movesets;
        [SerializeField] private Transform[] effectLocations;
        private float combatResetTime;

        private enum Character
        {
            Wiz, Teacher, SomeOtherCharacter
        };

        void Start()
        {
            Controller = GetComponent<Controller>();
            Animator = GetComponent<Animator>();
    
            // Select starting state
            switch (character)
            {
                case Character.Wiz:
                    SetState(new Wiz.Idle(this));
                    break;
                case Character.Teacher:
                    //SetState(new Teacher.Idle(this));
                    //break;
                default:
                    Debug.Log("Uh oh stinky");
                    break;
            }
        }

        public void SetGroundState(bool state)
        {
            IsOnGround = state;
        }

        public void EnableMovement()
        {
            CanMove = true;
        }

        public void DisableMovement()
        {
            CanMove = false;
        }

        public void EnableAttacks()
        {
            CanAttack = true;
        }

        public void DisableAttacks()
        {
            CanAttack = false;
        }

        public void SetState(PlayerState state)
        {
            if (PlayerState != null)
                PlayerState.ExitState();

            PlayerState = state;
            PlayerState.EnterState();
        }

        public void SetInCombat()
        {
            IsInCombat = true;
            combatResetTime = Time.time + 5f;
        }

        public void AnimationEventSpawnEffect(AnimationEvent parameter)
        {
            EffectManager.SpawnEffect(parameter, effectLocations[parameter.intParameter], true);
        }

        /// <summary>
        /// Select the next attack based on the current action and input (Melee / Magic).
        /// Return the current action if there is no match or if there is no moveset
        /// </summary>
        // Note: The time complexity of this algorithm is O(n^4), but since the move size is typically less than 15 moves (~1 ms runtime), it should be okay.
        public bool ChooseMove(PlayerInput.Action moveType)
        {
            foreach(Moveset moveset in movesets)
            {
                if (moveset == null)
                    continue;

                // Moveset: Melee, Magic or Utility?
                if (moveType == moveset.moveType)
                {
                    foreach(Move move in moveset.moves)
                    {
                        // Iterate through each equipped move's transitions to find one that matches the current action
                        foreach(string transition in move.canTransitionFrom)
                        {
                            // If a valid transition matches the current action, return the corresponding move
                            if (transition == PlayerStateName)
                            {
                                if (InCorrectPosition(move))
                                {
                                    SelectedMove = move;
                                    return true;
                                }
                            }
                        } // TODO: Fix magic string "All"
                        if (move.canTransitionFrom[0] == "All")
                        {
                            bool skip = false;
                            // Exceptions to any transitions
                            foreach(string nonTransition in move.cannotTransitionFrom)
                            {
                                if (nonTransition == PlayerStateName)
                                {
                                    skip = true;
                                    break;
                                }
                            }
                            if (!skip && InCorrectPosition(move))
                            {   
                                SelectedMove = move;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        // Check which way player is facing depending on player input and flip
        // if player wants to turn around
        public void CheckDirection(float direction)
        {
            if (direction > 0)
                IsFacingRight = true;
            else if (direction < 0)
                IsFacingRight = false;

            Vector2 currentScale = transform.localScale;
            currentScale.x = (IsFacingRight == true) ? 1 : -1;
            transform.localScale = currentScale;
        }

        bool InCorrectPosition(Move move)
        {
            if (move.position == Move.Position.Ground &&  IsOnGround) return true;
            if (move.position == Move.Position.Air    && !IsOnGround) return true;
            if (move.position == Move.Position.Both) return true;
            return false;
        }

        void Update()
        {
            Animator.SetFloat("Vertical Velocity", Controller.rigidbody2d.velocity.y);

            if (IsInCombat && Time.time >= combatResetTime)
                IsInCombat = false;

            PlayerStateName = PlayerState.GetType().Name;
            PlayerState.DoStateBehaviour();
            PlayerState.Transitions();
        }
    }
}