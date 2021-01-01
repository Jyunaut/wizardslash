using UnityEngine;

namespace Player
{
    public class StateManager : MonoBehaviour
    {
        [SerializeField] private Character character;
        [HideInInspector] public Controller controller;
        [HideInInspector] public Animator animator;
        public string playerStateName;
        public PlayerState playerState;
        public Moveset[] movesets;
        public Moveset selectedMoveset;
        public Move selectedMove;
        public Transform[] effectLocations;
        public bool isInCombat;
        public bool onGround;
        public bool facingRight;
        public bool canMove;
        public bool canAttack;
        public bool isAttacking;
        
        private enum Character
        {
            Wiz, Teacher, SomeOtherCharacter
        };
        private float combatResetTime;

        private StateManager()
        {
            onGround = false;
            facingRight = true;
            canMove = true;
            canAttack = true;
            isAttacking = false;
        }

        void Start()
        {
            controller = GetComponent<Controller>();
            animator = GetComponent<Animator>();

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

        public void SetState(PlayerState state)
        {
            if (playerState != null)
                playerState.ExitState();

            playerState = state;
            playerState.EnterState();
        }

        public void SetInCombat()
        {
            isInCombat = true;
            combatResetTime = Time.time + 5f;
        }

        public void AnimationEventSpawnEffect(AnimationEvent parameter)
        {
            EffectManager.SpawnEffect(parameter, effectLocations[parameter.intParameter], true);
        }

        // Select the next attack based on the current action and input (Melee / Magic)
        // Return the current action if there is no match or if there is no moveset
        // Note: The time complexity of this algorithm is O(n^4), but since the move size is typically less than 15 moves (<1 ms runtime), it should be okay
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
                        bool skip = false;
                        // Iterate through each equipped move's transitions to find one that matches the current action
                        foreach(string transition in move.canTransitionFrom)
                        {
                            // If a valid transition matches the current action, return the corresponding move
                            if (transition == playerStateName)
                            {
                                if (InCorrectPosition(move))
                                {
                                    selectedMoveset = moveset;
                                    selectedMove = move;
                                    return true;
                                }
                            }
                        }
                        if (move.canTransitionFrom[0] == "All")
                        {
                            // Exceptions to any transitions
                            foreach(string nonTransition in move.cannotTransitionFrom)
                            {
                                if (nonTransition == playerStateName)
                                {
                                    skip = true;
                                    break;
                                }
                            }
                            if (!skip && InCorrectPosition(move))
                            {   
                                selectedMoveset = moveset;
                                selectedMove = move;
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
                facingRight = true;
            else if (direction < 0)
                facingRight = false;

            Vector2 currentScale = transform.localScale;
            currentScale.x = (facingRight == true) ? 1 : -1;
            transform.localScale = currentScale;
        }

        bool InCorrectPosition(Move move)
        {
            if (move.position == Move.Position.Ground &&  onGround) return true;
            if (move.position == Move.Position.Air    && !onGround) return true;
            if (move.position == Move.Position.Both) return true;
            return false;
        }

        void Update()
        {
            animator.SetFloat("Vertical Velocity", controller.rigidbody2d.velocity.y);

            if (isInCombat && Time.time >= combatResetTime)
                isInCombat = false;

            playerStateName = playerState.GetType().Name;
            playerState.DoStateBehaviour();
            playerState.Transitions();
        }
    }
}