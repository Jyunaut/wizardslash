using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(AttackTimer))]
[RequireComponent(typeof(MoveSelector))]
public class PlayerController : Actor
{
    public PlayerInput playerInput = new PlayerInput();
    AttackTimer attackTimer;
    MoveSelector moveSelector;
    PlayerAnimator playerAnimator;

    void Start()
    {
        attackTimer = GetComponent<AttackTimer>();
        moveSelector = GetComponent<MoveSelector>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    public void StartMove()
    {
        canAttack = false;
        canMove = false;
        isAttacking = true;
        attackTimer.timer = 0;
        attackTimer.isTiming = true;
        attackTimer.isPushing = false;
        attackTimer.initialPush = true;
        playerAnimator.SetInCombat();
        CheckDirection(playerInput.Horizontal);
    }

    void HandleMoveInput(Moveset.MoveType moveType)
    {
        if (canAttack)
        {
            string previousAction = currentAction;
            currentAction = moveSelector.ChooseMove(moveType, currentAction);
            
            if (currentAction == previousAction)
                return;
            StartMove();
        }
        else if (attackTimer.timer >= (moveSelector.selectedMove.recoverFrame - 1f) / AttackTimer.FPS
                    && attackTimer.timer <= moveSelector.selectedMove.recoverFrame / AttackTimer.FPS)
        {   
            // Attack buffering
            attackTimer.isQueued = true;
            attackTimer.moveType = moveType;
        }
    }

    // Manages basic horizontal movement
    void FixedUpdate()
    {
        if ((!canMove || isAttacking) && !moveSelector.selectedMove.movementAllowed)
            return;
        
        Run(playerInput.Horizontal);
        if (attackTimer.timer == 0)
            CheckDirection(playerInput.Horizontal);
    }

    // Manages single-input key presses
    void Update()
    {
        if (playerInput.Jump)
        {
            if (canMove && onGround)
            {
                Jump();
                attackTimer.ResetTimerAndValues();
                currentAction = "AirNeutral";
            }
        }
        else if (playerInput.Melee)
        {
            HandleMoveInput(Moveset.MoveType.Melee);
        }
        else if (playerInput.Magic)
        {
            HandleMoveInput(Moveset.MoveType.Magic);
        }
        else if (playerInput.Dodge)
        {
            HandleMoveInput(Moveset.MoveType.Utility);
        }
    }
}