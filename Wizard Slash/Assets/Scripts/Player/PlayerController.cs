using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(AttackTimer))]
[RequireComponent(typeof(MoveSelector))]
public class PlayerController : Actor
{
    PlayerInput PlayerInput = new PlayerInput();
    AttackTimer AttackTimer;
    MoveSelector MoveSelector;
    PlayerAnimator PlayerAnimator;

    void Start()
    {
        AttackTimer = GetComponent<AttackTimer>();
        MoveSelector = GetComponent<MoveSelector>();
        PlayerAnimator = GetComponent<PlayerAnimator>();
    }

    public void StartMove()
    {
        canAttack = false;
        canMove = false;
        isAttacking = true;
        AttackTimer.timer = 0;
        AttackTimer.isTiming = true;
        AttackTimer.isPushing = false;
        AttackTimer.initialPush = true;
        PlayerAnimator.SetInCombat();
        CheckDirection(PlayerInput.Horizontal);
    }

    // Manages basic horizontal movement
    void FixedUpdate()
    {
        if (!canMove || isAttacking)
            return;
        
        Run(PlayerInput.Horizontal);
        if (AttackTimer.timer == 0)
            CheckDirection(PlayerInput.Horizontal);
    }

    // Manages single-input key presses
    void Update()
    {
        if (PlayerInput.Jump)
        {
            if (canMove && onGround)
            {
                Invoke("Jump", 0.06f);
                currentAction = "AirNeutral";
            }
        }
        else if (PlayerInput.Melee)
        {
            if (canAttack)
            {
                string previousAction = currentAction;
                currentAction = MoveSelector.ChooseMove(Moveset.MoveType.Melee, currentAction);
                
                if (currentAction == previousAction)
                    return;
                StartMove();
            }
            else if (AttackTimer.timer >= (MoveSelector.SelectedMove.recoverFrame - 1f) / AttackTimer.FPS && AttackTimer.timer <= MoveSelector.SelectedMove.recoverFrame / AttackTimer.FPS)
            {   
                // Attack queueing (Melee)
                AttackTimer.isQueued = true;
                AttackTimer.moveType = Moveset.MoveType.Melee;
            }
        }
        else if (PlayerInput.Magic)
        {
            if (canAttack)
            {
                string previousAction = currentAction;
                currentAction = MoveSelector.ChooseMove(Moveset.MoveType.Magic, currentAction);

                if (currentAction == previousAction)
                    return;
                StartMove();
            }
            else if (AttackTimer.timer >= (MoveSelector.SelectedMove.recoverFrame - 1f) / AttackTimer.FPS && AttackTimer.timer < MoveSelector.SelectedMove.recoverFrame / AttackTimer.FPS)
            {
                // Attack queueing (Magic)
                AttackTimer.isQueued = true;
                AttackTimer.moveType = Moveset.MoveType.Magic;
            }
        }
        else if (PlayerInput.Dodge)
        {
            //TODO: Implement player dodge
        }
    }
}