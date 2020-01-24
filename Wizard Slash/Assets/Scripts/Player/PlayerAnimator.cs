using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public Animator animator;

    public Transform[] effectLocation;

    public State currentState;
    public string stateInString;

    [HideInInspector] public bool inCombat;
    private float combatResetTime;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();

        SetState(new Idle(this));
    }

    public void SetState(State state)
    {
        if (currentState != null)
            currentState.ExitState();

        currentState = state;

        if (currentState != null)
            currentState.EnterState();
    }

    public void SetInCombat()
    {
        inCombat = true;
        combatResetTime = Time.time + 5f;
    }

    public void AnimationEventSpawnEffect(AnimationEvent parameter)
    {
        EffectManager.SpawnEffect(parameter, effectLocation[parameter.intParameter], true);
    }

    void Update()
    {
        animator.SetFloat("Vertical Velocity", playerController.rigidbody2d.velocity.y);

        // Play "In Combat" neutral animations during battle
        if (inCombat && Time.time >= combatResetTime)
        {
            inCombat = false;
            SetState(currentState);
        }
        currentState.Transitions();
        stateInString = currentState.ToString();
    }
}

// ==========================================
// State Machine
// ------------------------------------------

public abstract class State
{
    protected PlayerAnimator playerAnimator;

    public abstract void Transitions();
    public virtual void EnterState() {}
    public virtual void ExitState() {}

    public State(PlayerAnimator playerAnimator)
    {
        this.playerAnimator = playerAnimator;
    }

    #region "State Transitions"
    public void Idle()
    {
        if (playerAnimator.playerController.playerInput.Horizontal == 0
            && playerAnimator.playerController.onGround)
        {
            playerAnimator.SetState(new Idle(playerAnimator)); return;
        }
    }

    public void Run()
    {
        if (playerAnimator.playerController.playerInput.Horizontal != 0
            && Mathf.Abs(playerAnimator.playerController.rigidbody2d.velocity.x) > 0
            && playerAnimator.playerController.onGround)
        {
            playerAnimator.SetState(new Run(playerAnimator)); return;
        }
    }

    public void RunBrake()
    {
        if (playerAnimator.playerController.playerInput.Horizontal == 0
            && playerAnimator.playerController.onGround)
        {
            playerAnimator.SetState(new RunBrake(playerAnimator)); return;
        }
    }

    public void Jump()
    {
        if (playerAnimator.playerController.playerInput.Jump
            && playerAnimator.playerController.canMove
            && playerAnimator.playerController.onGround)
        {
            playerAnimator.SetState(new Jump(playerAnimator)); return;
        }
    }

    public void Fall()
    {
        if (playerAnimator.playerController.rigidbody2d.velocity.y <= 0
            && !playerAnimator.playerController.onGround)
        {
            playerAnimator.SetState(new Fall(playerAnimator)); return;
        }
    }

    public void Attack()
    {
        if (playerAnimator.animator.HasState(0, Animator.StringToHash(playerAnimator.playerController.currentAction))
            && !playerAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName(playerAnimator.playerController.currentAction))
        {
            playerAnimator.SetState(new Attack(playerAnimator)); return;
        }
    }

    public void Land()
    {

    }

    public void Turn()
    {

    }

    public void Damaged()
    {

    }

    public void NonCombatTransition()
    {

    }
    #endregion
}

class Idle : State
{
    public Idle(PlayerAnimator playerAnimator) : base(playerAnimator) {}

    public override void EnterState()
    {
        if (playerAnimator.inCombat)
            playerAnimator.animator.Play("Idle Combat");
        else
            playerAnimator.animator.Play("Idle");
    }

    public override void Transitions()
    {
        Run();
        Jump();
        Fall();
        Attack();
    }
}

class Run : State
{
    public Run(PlayerAnimator playerAnimator) : base(playerAnimator) {}

    public override void EnterState()
    {
        if (playerAnimator.inCombat)
            playerAnimator.animator.Play("Run Combat");
        else    
            playerAnimator.animator.Play("Run");
    }

    public override void Transitions()
    {
        RunBrake();
        Jump();
        Fall();
        Attack();
    }
}

class RunBrake : State
{
    public RunBrake(PlayerAnimator playerAnimator) : base(playerAnimator) {}

    private float stateEndTime;

    public override void EnterState()
    {
        if (playerAnimator.inCombat)
            playerAnimator.animator.Play("RunBrake");   // TODO: Add RunBrake Combat animation
        else
            playerAnimator.animator.Play("RunBrake");

        stateEndTime = Time.time + 0.278f;
    }

    public override void Transitions()
    {
        if (Time.time >= stateEndTime)
            Idle();

        Run();    
        Jump();
        Fall();
        Attack();
    }
}

class Jump : State
{
    public Jump(PlayerAnimator playerAnimator) : base(playerAnimator) {}

    public override void EnterState()
    {
        playerAnimator.animator.Play("Jump");
    }

    public override void Transitions()
    {
        Fall();
        Attack();
    }
}

class Fall : State
{
    public Fall(PlayerAnimator playerAnimator) : base(playerAnimator) {}

    public override void EnterState()
    {
        playerAnimator.animator.Play("Fall");
    }

    public override void Transitions()
    {
        Idle();
        Run();
        Attack();
    }
}
// TODO: Implement Land State
class Land : State
{
    public Land(PlayerAnimator playerAnimator) : base(playerAnimator) {}

    public override void EnterState()
    {

    }

    public override void Transitions()
    {

    }
}
// TODO: Implement Turn State
class Turn : State
{
    public Turn(PlayerAnimator playerAnimator) : base(playerAnimator) {}

    public override void EnterState()
    {

    }

    public override void Transitions()
    {
        
    }
}
// TODO: Implement Damaged State
class Damaged : State
{
    public Damaged(PlayerAnimator playerAnimator) : base(playerAnimator) {}

    public override void EnterState()
    {

    }

    public override void Transitions()
    {
        
    }
}
// TODO: Implement NonCombatTransition State
class NonCombatTransition : State
{
    public NonCombatTransition(PlayerAnimator playerAnimator) : base(playerAnimator) {}

    public override void EnterState()
    {

    }

    public override void Transitions()
    {
        
    }
}

class Attack : State
{
    public Attack(PlayerAnimator playerAnimator) : base(playerAnimator) {}

    public override void EnterState()
    {
        playerAnimator.animator.Play(playerAnimator.playerController.currentAction);
    }

    public override void Transitions()
    {
        Attack();
        Jump();
        if (!playerAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName(playerAnimator.playerController.currentAction))
        {
            switch (playerAnimator.playerController.currentAction)
            {
                case "Neutral":
                    Idle();
                    Run();
                    break;
                case "AirNeutral":
                    Fall();
                    break;
            }
        }
    }
}