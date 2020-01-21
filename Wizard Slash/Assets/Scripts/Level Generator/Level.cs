using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public enum LevelType
    {
        Forest, Tundra, Swamp, Desert, Pyramid, Arctic, Volcanic, Final
    };
    public List<Floor> floors;
    public LevelType levelType;
    public int numFloors;
}