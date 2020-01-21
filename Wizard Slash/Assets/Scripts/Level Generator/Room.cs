using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum RoomType 
    { 
        spawn, exit, encounter, treasure, empty
    };
    
    public List<Vector2> exits;
    public RoomType roomType;
}