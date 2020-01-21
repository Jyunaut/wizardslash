using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CanHit : MonoBehaviour
{
    PlayerController PlayerController;
    MoveSelector MoveSelector;

    public enum AttackingUnit { Player, Enemy, Boss, Projectile };
    public AttackingUnit attackingUnit;

    void Start()
    {
        if (attackingUnit == AttackingUnit.Player)
        {
            PlayerController = GetComponentInParent<PlayerController>();
            MoveSelector = GetComponentInParent<MoveSelector>();
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
                canBeHit.OnScreenShake.Invoke(MoveSelector.SelectedMove.amplitude, MoveSelector.SelectedMove.shakeLength);
                canBeHit.OnKnockback.Invoke(MoveSelector.SelectedMove.knockbackX, MoveSelector.SelectedMove.knockbackY, PlayerController.facingRight ? 1 : -1);
                EffectManager.SpawnEffect(MoveSelector.SelectedMove.hitEffect, transform, targetCollider, true);
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