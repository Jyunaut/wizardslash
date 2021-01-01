using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Wiz;

namespace Player
{
    public abstract class PlayerState : State
    {
        public StateManager stateManager;
        public string stateName;

        public PlayerState(StateManager stateManager, string stateName)
        {
            this.stateManager = stateManager;
            this.stateName = stateName;
        }

        public virtual void HandleInput(PlayerInput.Action action) {}
    }
}