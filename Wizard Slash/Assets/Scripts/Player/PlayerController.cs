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

    // Manages basic horizontal movement
    void FixedUpdate()
    {
        if (!canMove || isAttacking)
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
            if (canAttack)
            {
                string previousAction = currentAction;
                currentAction = moveSelector.ChooseMove(Moveset.MoveType.Melee, currentAction);
                
                if (currentAction == previousAction)
                    return;
                StartMove();
            }
            else if (attackTimer.timer >= (moveSelector.selectedMove.recoverFrame - 1f) / AttackTimer.FPS
                     && attackTimer.timer <= moveSelector.selectedMove.recoverFrame / AttackTimer.FPS)
            {   
                // Attack queueing (Melee)
                attackTimer.isQueued = true;
                attackTimer.moveType = Moveset.MoveType.Melee;
            }
        }
        else if (playerInput.Magic)
        {
            if (canAttack)
            {
                string previousAction = currentAction;
                currentAction = moveSelector.ChooseMove(Moveset.MoveType.Magic, currentAction);

                if (currentAction == previousAction)
                    return;
                StartMove();
            }
            else if (attackTimer.timer >= (moveSelector.selectedMove.recoverFrame - 1f) / AttackTimer.FPS
                     && attackTimer.timer < moveSelector.selectedMove.recoverFrame / AttackTimer.FPS)
            {
                // Attack queueing (Magic)
                attackTimer.isQueued = true;
                attackTimer.moveType = Moveset.MoveType.Magic;
            }
        }
        else if (playerInput.Dodge)
        {
            //TODO: Implement player dodge
        }
    }
}