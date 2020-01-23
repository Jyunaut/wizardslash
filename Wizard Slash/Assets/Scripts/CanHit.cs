using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CanHit : MonoBehaviour
{
    PlayerController playerController;
    MoveSelector moveSelector;

    public enum AttackingUnit { Player, Enemy, Boss, Projectile };
    public AttackingUnit attackingUnit;

    void Start()
    {
        if (attackingUnit == AttackingUnit.Player)
        {
            playerController = GetComponentInParent<PlayerController>();
            moveSelector = GetComponentInParent<MoveSelector>();
        }
    }

    void OnTriggerEnter2D(Collider2D targetCollider)
    {
        CanBeHit canBeHit = targetCollider.GetComponent<CanBeHit>();

        if (!canBeHit)
            return;
        
        switch (attackingUnit)
        {
            case AttackingUnit.Player:
                canBeHit.OnScreenShake.Invoke(moveSelector.selectedMove.amplitude, moveSelector.selectedMove.shakeLength);
                canBeHit.OnKnockback.Invoke(moveSelector.selectedMove.knockbackX, moveSelector.selectedMove.knockbackY, playerController.facingRight ? 1 : -1);
                EffectManager.SpawnEffect(moveSelector.selectedMove.hitEffect, transform, targetCollider, true);
                break;
            case AttackingUnit.Enemy:
                break;
            case AttackingUnit.Boss:
                break;
            case AttackingUnit.Projectile:
                break;
        }
    }
}