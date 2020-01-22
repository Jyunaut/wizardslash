using UnityEngine;

public class CheckIfGrounded : MonoBehaviour
{   
    [SerializeField] private LayerMask layerGround;

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            GetComponentInParent<Actor>().onGround = true;

            if (!GetComponentInParent<Actor>().isAttacking)
                GetComponentInParent<Actor>().currentAction = "Neutral";
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            GetComponentInParent<Actor>().onGround = false;
            
            if (!GetComponentInParent<Actor>().isAttacking)
                GetComponentInParent<Actor>().currentAction = "AirNeutral";
        }
    }
}
