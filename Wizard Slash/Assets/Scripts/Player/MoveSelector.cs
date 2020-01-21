using System.Collections.Generic;
using UnityEngine;

public class MoveSelector : MonoBehaviour
{
    public List<Moveset> movesets = new List<Moveset>();
    [HideInInspector] public Move SelectedMove;

    // Select the next attack based on the current action and input (Melee / Magic)
    // Return the current action if there is no match or if there is no moveset
    public string ChooseMove(Moveset.MoveType moveType, string currentAction)
    {
        foreach(Moveset moveset in movesets)
        {
            if (moveset == null)
                return currentAction;

            // Moveset: Melee or Magic?
            if (moveType == moveset.moveType)
            {
                // Iterate through each equipped move's transitions to find one that matches the current action
                foreach(Move move in moveset.moves)
                {
                    foreach(string transition in move.canTransitionFrom)
                    {
                        // If a valid transition matches the current action, return the corresponding move
                        if (transition == currentAction)
                        {
                            SelectedMove = move;
                            return SelectedMove.Name;
                        }
                    }
                }
            }
        }
        return currentAction;
    }
}