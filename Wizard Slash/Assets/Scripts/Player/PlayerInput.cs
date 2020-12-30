using UnityEngine;

namespace Player
{
    public class PlayerInput
    {
        // Initialize player input variables
        public float Horizontal => Input.GetAxisRaw("Horizontal");
        public float Vertical => Input.GetAxisRaw("Vertical");
        public bool Jump  => Input.GetButtonDown("Jump");
        public bool Melee => Input.GetButtonDown("Melee"); 
        public bool Magic => Input.GetButtonDown("Magic"); 
        public bool Utility => Input.GetButtonDown("Dodge"); // TODO: Rename this to "Utility"
        public enum Action
        {
            Jump, Melee, Magic, Utility
        };
    }
}
