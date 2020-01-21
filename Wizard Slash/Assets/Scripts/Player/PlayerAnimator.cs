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
    }
}