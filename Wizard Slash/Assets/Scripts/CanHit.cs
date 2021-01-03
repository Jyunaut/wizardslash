using UnityEngine;
using Player;

[RequireComponent(typeof(BoxCollider2D))]
public class CanHit : MonoBehaviour
{
    public enum AttackingUnit { Player, Enemy, Boss, Projectile };
    public AttackingUnit attackingUnit;
    
    private Controller controller;
    private StateManager stateManager;

    void Start()
    {
        if (attackingUnit == AttackingUnit.Player)
        {
            controller = GetComponentInParent<Controller>();
            stateManager = GetComponentInParent<StateManager>();
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
                canBeHit.OnScreenShake.Invoke(stateManager.SelectedMove.amplitude, stateManager.SelectedMove.shakeLength);
                canBeHit.OnKnockback.Invoke(stateManager.SelectedMove.knockbackX, stateManager.SelectedMove.knockbackY, stateManager.IsFacingRight ? 1 : -1);
                canBeHit.OnTimeStop.Invoke(stateManager.SelectedMove.timeSlowPercent, stateManager.SelectedMove.timeSlowLength);
                foreach(GameObject effect in stateManager.SelectedMove.hitEffect)
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