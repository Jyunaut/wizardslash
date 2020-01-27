using UnityEngine;

public class EffectManager : MonoBehaviour
{
    // Determine if the spawn reference location is within the bounds of the target collider
    // Otherwise, Move the spawn location on the boundary of the target collider closest to the reference location
    public static void SpawnEffect(GameObject hitEffect, Transform objectTransform, Collider2D targetCollider, bool sameDirection = true)
    {
        if (!hitEffect)
        {
            Debug.LogWarning("Hit Effect does not exist");
            return;
        }

        GameObject Effect;
        if (targetCollider.bounds.Contains(objectTransform.position))
        {
            Effect = Instantiate(hitEffect, objectTransform.position, Quaternion.identity);
        }
        else
        {
            Vector2 newHitEffectLocation = targetCollider.ClosestPoint(objectTransform.position);
            Effect = Instantiate(hitEffect, newHitEffectLocation, Quaternion.identity);
        }

        if (sameDirection)
        {
            int flipSprite = (objectTransform.lossyScale.x == 1) ? 0 : 1;
            Effect.GetComponent<ParticleSystemRenderer>().flip = new Vector2(flipSprite, 0);
        }
    }

    // Spawn effects using Animation Events
    public static void SpawnEffect(AnimationEvent parameter, Transform objectTransform, bool sameDirection = true)
    {
        GameObject Effect = Instantiate((GameObject)parameter.objectReferenceParameter, objectTransform.position, Quaternion.identity);

        if (sameDirection)
        {
            int flipSprite = (objectTransform.lossyScale.x == 1) ? 0 : 1;
            Effect.GetComponent<ParticleSystemRenderer>().flip = new Vector2(flipSprite, 0);
        }
    }
}