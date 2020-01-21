using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public List<Room> rooms;
    public List<Vector2> grid;
    public List<Vector2> exits;
    public Vector2 spawnPoint;
    int width;
    int height;
    void Start()
    {
        //Initialize all variables by type
    }
}