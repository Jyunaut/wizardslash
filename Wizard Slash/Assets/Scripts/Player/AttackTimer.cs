using UnityEngine;

public class AttackTimer : MonoBehaviour
{
    PlayerController PlayerController;
    MoveSelector MoveSelector;

    public const float FPS = 18f;
    public float timer = 0f;
    public float timerInFrames = 0f;
    public bool isQueued = false;

    public bool isTiming;
    public bool isPushing;
    public bool initialPush;
    private float pushEndTime;
    [HideInInspector]
    public Moveset.MoveType moveType;

    void Start()
    {
        PlayerController = GetComponent<PlayerController>();
        MoveSelector = GetComponent<MoveSelector>();
    }

    void ResetTimerAndValues()
    {
        isTiming = false;
        timer = 0;
        timerInFrames = 0f;

        PlayerController.currentAction = (PlayerController.onGround) ? "Neutral" : "AirNeutral";
        
        PlayerController.canAttack = true;
        PlayerController.canMove = true;
        PlayerController.isAttacking = false;
    }

    // Each player move goes through a timeline where windup -> hit -> recover
    // and allows the player to be pushed throughout a portion of the timeline
    void TimeAttack()
    {
        if (timer <= MoveSelector.SelectedMove.hitFrame / FPS)
        {
            // Windup
            PlayerController.canAttack = false;
            PlayerController.canMove = false;
        }
        else if (timer >= MoveSelector.SelectedMove.hitFrame / FPS && timer <= MoveSelector.SelectedMove.recoverFrame / FPS)
        {
            // Hit
            isPushing = true;
        }
        else if (timer >= MoveSelector.SelectedMove.recoverFrame / FPS && timer <= MoveSelector.SelectedMove.totalFrames / FPS)
        {
            // Recover
            PlayerController.canAttack = true;
            PlayerController.canMove = true;
            isPushing = true;
            
            // Execute next attack if there is a queued attack
            if (isQueued && timer >= (MoveSelector.SelectedMove.recoverFrame + 1) / FPS)
            {
                string previousAction = PlayerController.currentAction;
                PlayerController.currentAction = MoveSelector.ChooseMove(moveType, PlayerController.currentAction);
                isQueued = false;

                // Don't execute next attack if it was the same as the previous
                if (PlayerController.currentAction == previousAction)
                    return;
                PlayerController.StartMove();
            }
        }
        else
        {
            // End combo
            ResetTimerAndValues();
        }
    }

    // Push player in a specified direction depending on the executed move
    void PushPlayer()
    {
        int direction = PlayerController.facingRight ? 1 : -1;

        if (initialPush)
        {
            // Initial push of the attack
            PlayerController.rigidbody2d.velocity = new Vector2(direction * MoveSelector.SelectedMove.pushX, MoveSelector.SelectedMove.pushY);
            pushEndTime = Time.time + MoveSelector.SelectedMove.pushFrames / FPS;
            initialPush = false;
        }
        else if (Time.time >= pushEndTime - MoveSelector.SelectedMove.pushFrames / FPS / 2f && Time.time <= pushEndTime)
        {
            // Deccelerate the player during the last half of the push event
            PlayerController.rigidbody2d.velocity = new Vector2(PlayerController.rigidbody2d.velocity.x * MoveSelector.SelectedMove.decceleration,
                                                                PlayerController.rigidbody2d.velocity.y);
        }
        else if (Time.time >= pushEndTime)
        {
            // Stop the push after the end time
            PlayerController.rigidbody2d.velocity = new Vector2(0, PlayerController.rigidbody2d.velocity.y);
            isPushing = false;
        }
    }

    void Update()
    {
        if (!isTiming)
            return;
        
        if (isPushing)
            PushPlayer();
        
        TimeAttack();
        if (isTiming)
        {
            timer += Time.deltaTime;
            timerInFrames = timer * FPS;
        }
    }
}
