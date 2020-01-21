using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Moveset : ScriptableObject
{
    public enum MoveType
    {
        Melee, Magic, Utility
    };

    public MoveType moveType;
    public List<Move> moves = new List<Move>();
}