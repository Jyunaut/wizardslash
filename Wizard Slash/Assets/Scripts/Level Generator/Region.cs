using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region
{
    public List<Level> levels;

    public Region()
    {
        Debug.Log("Creating preset levels");
    }

    public void CreateLevel(int floors, Level.LevelType type)
    {
        Level a = new Level();
        a.numFloors = floors;
        a.levelType = type;
        levels.Add(a);
    }
}