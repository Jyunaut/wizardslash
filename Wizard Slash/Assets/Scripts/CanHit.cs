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
                canBeHit.OnTakeDamage.Invoke(1); //TODO: Add damage numbers
                canBeHit.OnScreenShake.Invoke(moveSelector.selectedMove.amplitude, moveSelector.selectedMove.shakeLength);
                canBeHit.OnKnockback.Invoke(moveSelector.selectedMove.knockbackX, moveSelector.selectedMove.knockbackY, playerController.facingRight ? 1 : -1);
                foreach(GameObject effect in moveSelector.selectedMove.hitEffect)
                    EffectManager.SpawnEffect(effect, transform, targetCollider, true);
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