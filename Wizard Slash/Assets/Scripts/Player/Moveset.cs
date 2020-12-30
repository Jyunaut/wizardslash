using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Moveset : ScriptableObject
{
    public Player.PlayerInput.Action moveType;
    public List<Move> moves = new List<Move>();
}