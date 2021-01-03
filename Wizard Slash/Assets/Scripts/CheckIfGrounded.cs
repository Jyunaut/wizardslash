using UnityEngine;

public class CheckIfGrounded : MonoBehaviour
{   
    [SerializeField] private LayerMask layerGround;

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            GetComponentInParent<Player.StateManager>().SetGroundState(true);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            GetComponentInParent<Player.StateManager>().SetGroundState(false);
        }
    }
}
