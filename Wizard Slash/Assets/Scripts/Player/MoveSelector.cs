using System.Collections.Generic;
using UnityEngine;

public class MoveSelector : MonoBehaviour
{
    PlayerController playerController;
    public List<Moveset> movesets = new List<Moveset>();
    [HideInInspector] public Move selectedMove;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    bool InCorrectPosition(Move move)
    {
        if (move.position == Move.Position.Ground &&  playerController.onGround) return true;
        if (move.position == Move.Position.Air    && !playerController.onGround) return true;
        if (move.position == Move.Position.Both) return true;
        return false;
    }

    // Select the next attack based on the current action and input (Melee / Magic)
    // Return the current action if there is no match or if there is no moveset
    // Note: The time complexity of this algorithm is O(n^4), but since the size is typically less than 15 moves, it should be okay
    public string ChooseMove(Moveset.MoveType moveType, string currentAction)
    {
        foreach(Moveset moveset in movesets)
        {
            if (moveset == null)
                continue;

            // Moveset: Melee, Magic or Utility?
            if (moveType == moveset.moveType)
            {
                foreach(Move move in moveset.moves)
                {
                    // Iterate through each equipped move's transitions to find one that matches the current action
                    foreach(string transition in move.canTransitionFrom)
                    {
                        // If a valid transition matches the current action, return the corresponding move
                        if (transition == currentAction)
                        {
                            if (InCorrectPosition(move))
                            {
                                selectedMove = move;
                                return selectedMove.Name;
                            }
                        }
                        else if (transition == "All")
                        {
                            // Exceptions to any transitions
                            foreach(string nonTransition in move.cannotTransitionFrom)
                            {
                                if (currentAction == nonTransition)
                                    continue;

                                if (InCorrectPosition(move))
                                {
                                    selectedMove = move;
                                    return selectedMove.Name;
                                }
                            }
                        }
                    }
                }
            }
        }
        return currentAction;
    }
}