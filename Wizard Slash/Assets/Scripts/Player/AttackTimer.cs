using UnityEngine;

public class AttackTimer : MonoBehaviour
{
    PlayerController playerController;
    MoveSelector moveSelector;

    public const float FPS = 18f;
    public float timer = 0f;
    public float timerInFrames = 0f;
    public bool isQueued = false;

    public bool isTiming;
    public bool isPushing;
    public bool initialPush;
    private float pushEndTime;
    [HideInInspector] public Moveset.MoveType moveType;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        moveSelector = GetComponent<MoveSelector>();
    }

    public void ResetTimerAndValues()
    {
        isTiming = false;
        timer = 0;
        timerInFrames = 0f;

        playerController.currentAction = (playerController.onGround) ? "Neutral" : "AirNeutral";
        
        playerController.canAttack = true;
        playerController.canMove = true;
        playerController.isAttacking = false;
    }

    // Each player move goes through a timeline where windup -> hit -> recover
    // and allows the player to be pushed throughout a portion of the timeline
    void TimeAttack()
    {
        if (timer <= moveSelector.selectedMove.hitFrame / FPS)
        {
            // Windup
            playerController.canAttack = false;
            playerController.canMove = false;
        }
        else if (timer >= moveSelector.selectedMove.hitFrame / FPS && timer <= moveSelector.selectedMove.recoverFrame / FPS)
        {
            // Hit
            isPushing = true;
        }
        else if (timer >= moveSelector.selectedMove.recoverFrame / FPS && timer <= moveSelector.selectedMove.totalFrames / FPS)
        {
            // Recover
            playerController.canAttack = true;
            playerController.canMove = true;
            isPushing = true;
            
            // Execute next attack if there is a queued attack
            if (isQueued && timer >= (moveSelector.selectedMove.recoverFrame + 1) / FPS)
            {
                string previousAction = playerController.currentAction;
                playerController.currentAction = moveSelector.ChooseMove(moveType, playerController.currentAction);
                isQueued = false;

                // Don't execute next attack if it was the same as the previous
                if (playerController.currentAction == previousAction)
                    return;
                playerController.StartMove();
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
        int direction = playerController.facingRight ? 1 : -1;

        if (initialPush)
        {
            // Initial push of the attack
            playerController.rigidbody2d.velocity = new Vector2(direction * moveSelector.selectedMove.pushX, moveSelector.selectedMove.pushY);
            pushEndTime = Time.time + moveSelector.selectedMove.pushFrames / FPS;
            initialPush = false;
        }
        else if (Time.time >= pushEndTime - moveSelector.selectedMove.pushFrames / FPS / 2f && Time.time <= pushEndTime)
        {
            // Deccelerate the player during the last half of the push event
            playerController.rigidbody2d.velocity = new Vector2(playerController.rigidbody2d.velocity.x * moveSelector.selectedMove.decceleration,
                                                                playerController.rigidbody2d.velocity.y);
        }
        else if (Time.time >= pushEndTime)
        {
            // Stop the push after the end time
            playerController.rigidbody2d.velocity = new Vector2(0, playerController.rigidbody2d.velocity.y);
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
