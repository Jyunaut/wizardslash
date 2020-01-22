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

    void Update()
    {
        animator.SetFloat("Vertical Velocity", playerController.rigidbody2d.velocity.y);

        // Play "In Combat" neutral animations during battle
        if (inCombat && Time.fixedTime >= combatResetTime)
        {
            inCombat = false;
            SetState(currentState);
        }
        
        currentState.StateBehaviour();
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
}

// ==========================================
// State Machine
// ------------------------------------------

public abstract class State
{
    protected PlayerAnimator playerAnimator;

    public abstract void StateBehaviour();

    public virtual void EnterState() {}
    public virtual void ExitState() {}

    public State(PlayerAnimator playerAnimator)
    {
        this.playerAnimator = playerAnimator;
    }
}

class Idle : State
{
    public Idle(PlayerAnimator playerAnimator) : base(playerAnimator) {}

    public override void EnterState()
    {
        // Play Animation
        if (playerAnimator.inCombat)
            playerAnimator.animator.Play("Idle Combat");
        else
            playerAnimator.animator.Play("Idle");
    }

    public override void StateBehaviour()
    {
        if (!playerAnimator.playerController.onGround)
            return;

        // Transitions
        if (playerAnimator.playerInput.Horizontal != 0 && Mathf.Abs(playerAnimator.playerController.rigidbody2d.velocity.x) > 0)
        {
            playerAnimator.SetState(new Run(playerAnimator)); return;
        }

        if (playerAnimator.animator.HasState(0, Animator.StringToHash(playerAnimator.playerController.currentAction)) &&
           !playerAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName(playerAnimator.playerController.currentAction))
        {
            playerAnimator.SetState(new Attack(playerAnimator)); return;
        }

        if (playerAnimator.playerInput.Jump)
        {
            playerAnimator.SetState(new Jump(playerAnimator)); return;
        }

        if (!playerAnimator.playerController.onGround)
        {
            playerAnimator.SetState(new Fall(playerAnimator)); return;
        }
    }
}

class Run : State
{
    public Run(PlayerAnimator playerAnimator) : base(playerAnimator) {}

    public override void EnterState()
    {
        // Play Animation
        if (playerAnimator.inCombat)
            playerAnimator.animator.Play("Run Combat");
        else    
            playerAnimator.animator.Play("Run");
    }

    public override void StateBehaviour()
    {
        // Transitions
        if (playerAnimator.playerInput.Horizontal == 0)
        {
            playerAnimator.SetState(new Idle(playerAnimator)); return;
        }

        if (playerAnimator.animator.HasState(0, Animator.StringToHash(playerAnimator.playerController.currentAction)) &&
           !playerAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName(playerAnimator.playerController.currentAction))
        {
            playerAnimator.SetState(new Attack(playerAnimator)); return;
        }

        if (playerAnimator.playerInput.Jump)
        {
            playerAnimator.SetState(new Jump(playerAnimator)); return;
        }

        if (!playerAnimator.playerController.onGround)
        {
            playerAnimator.SetState(new Fall(playerAnimator)); return;
        }
    }
}

class Jump : State
{
    public Jump(PlayerAnimator playerAnimator) : base(playerAnimator) {}

    public override void EnterState()
    {
        // Play Animation
        playerAnimator.animator.Play("Jump");
    }

    public override void StateBehaviour()
    {
        // Transitions
        if (!playerAnimator.playerController.onGround && playerAnimator.playerController.rigidbody2d.velocity.y <= 0)
        {
            playerAnimator.SetState(new Fall(playerAnimator)); return;
        }
    }
}

class Fall : State
{
    public Fall(PlayerAnimator playerAnimator) : base(playerAnimator) {}

    public override void EnterState()
    {
        // Play Animation
        playerAnimator.animator.Play("Fall");
    }

    public override void StateBehaviour()
    {
        // Transitions
        if (playerAnimator.playerController.onGround)
        {
            if (playerAnimator.playerInput.Horizontal == 0)
            {
                playerAnimator.SetState(new Idle(playerAnimator)); return;
            }
            else
            {
                playerAnimator.SetState(new Run(playerAnimator)); return;
            }
        }
    }
}

class Attack : State
{
    public Attack(PlayerAnimator playerAnimator) : base(playerAnimator) {}

    public override void EnterState()
    {
        // Play Animation
        playerAnimator.animator.Play(playerAnimator.playerController.currentAction);
    }

    public override void StateBehaviour()
    {
        // Transitions
        if (!playerAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName(playerAnimator.playerController.currentAction))
        {
            if (playerAnimator.playerController.currentAction != "Neutral")
            {
                playerAnimator.SetState(new Attack(playerAnimator)); return;
            }
            else
            {
                playerAnimator.SetState(new Idle(playerAnimator)); return;
            }
        }
    }
}