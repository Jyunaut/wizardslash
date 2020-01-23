using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public PlayerInput playerInput = new PlayerInput();
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public Animator animator;

    public Transform[] effectLocation;

    public State currentState;

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
        combatResetTime = Time.fixedTime + 5f;
    }

    public void AnimationEventSpawnEffect(AnimationEvent parameter)
    {
        EffectManager.SpawnEffect(parameter, effectLocation[parameter.intParameter], true);
    }

    void Update()
    {
        animator.SetFloat("Vertical Velocity", playerController.rigidbody2d.velocity.y);

        // Play "In Combat" neutral animations during battle
        if (inCombat && Time.fixedTime >= combatResetTime)
        {
            inCombat = false;
            SetState(currentState);
        }
        Debug.Log(currentState);
        currentState.Transitions();
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
        if (playerAnimator.playerInput.Horizontal == 0
            && playerAnimator.playerController.onGround)
        {
            playerAnimator.SetState(new Idle(playerAnimator)); return;
        }
    }

    public void Run()
    {
        if (playerAnimator.playerInput.Horizontal != 0
            && Mathf.Abs(playerAnimator.playerController.rigidbody2d.velocity.x) > 0
            && playerAnimator.playerController.onGround)
        {
            playerAnimator.SetState(new Run(playerAnimator)); return;
        }
    }

    public void Jump()
    {
        if (playerAnimator.playerInput.Jump)
        {
            playerAnimator.SetState(new Jump(playerAnimator)); return;
        }
    }

    public void Fall()
    {
        if (!playerAnimator.playerController.onGround
            && playerAnimator.playerController.rigidbody2d.velocity.y <= 0)
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
        Idle();
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