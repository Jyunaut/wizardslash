using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    PlayerInput PlayerInput = new PlayerInput();
    PlayerController PlayerController;
    Animator Animator;

    public Transform[] effectLocation;

    private bool inCombat = false;
    private float combatResetTime;

    void Start()
    {
        PlayerController = GetComponent<PlayerController>();
        Animator = GetComponent<Animator>();
    }

    public void PlayJumpSquatAnimation()
    {
        Animator.Play("Jump Squat");
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
        Animator.SetFloat("Horizontal Input", Mathf.Abs(PlayerInput.Horizontal));
        Animator.SetFloat("Absolute Horizontal Velocity", Mathf.Abs(PlayerController.rigidbody2d.velocity.x));
        Animator.SetFloat("Vertical Velocity", PlayerController.rigidbody2d.velocity.y);

        // Play animation based on player state
        if (Animator.HasState(0, Animator.StringToHash(PlayerController.currentAction)) && !Animator.GetCurrentAnimatorStateInfo(0).IsName(PlayerController.currentAction))
        {
            Animator.Play(PlayerController.currentAction);
        }

        // Play "In Combat" default animations during battle
        if (inCombat && Time.fixedTime >= combatResetTime)
        {
            inCombat = false;
        }

        if (PlayerController.currentAction == "Neutral" && !Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle (Combat)") && inCombat && PlayerController.onGround && PlayerInput.Horizontal == 0)
        {
            Animator.Play("Idle (Combat)");
        }
    }
}