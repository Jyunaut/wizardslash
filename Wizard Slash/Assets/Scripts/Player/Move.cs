using UnityEngine;

[System.Serializable]
public class Move
{
    public string Name;

    public enum Position { Ground, Air, Both };
    [Header("Conditions")]
    public Position position;
    public string[] canTransitionFrom;
    public string[] cannotTransitionFrom;

    [Header("Animation Frame Windows")]
    public int windupFrame;
    public int hitFrame;
    public int recoverFrame;
    public int totalFrames;
    public int pushFrames;

    [Header("Player Push")]
    public bool movementAllowed;
    public bool pushPlayer;
    public float pushX;
    public float pushY;
    public float decceleration;

    [Header("Target Knockback")]
    public float knockbackX;
    public float knockbackY;

    [Header("Screen Shake")]
    [Range(0,1f)] public float amplitude;
    [Range(0,0.5f)] public float shakeLength;

    [Header("Time Slow")]
    [Range(0,1f)] public float timeSlowPercent;
    [Range(0,2f)] public float timeSlowLength;

    [Header("Particle Effects")]
    public GameObject spawnEffect;
    public GameObject[] hitEffect;
}