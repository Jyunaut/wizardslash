using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Move
{
    public string Name;
    public string[] canTransitionFrom;

    [Header("Animation Frame Windows")]
    public float windupFrame;
    public float hitFrame;
    public float recoverFrame;
    public float totalFrames;
    public float pushFrames;

    [Header("Player Push")]
    public float pushX;
    public float pushY;
    public float decceleration;

    [Header("Target Knockback")]
    public float knockbackX;
    public float knockbackY;

    [Header("Screen Shake")]
    [Range(0, 1)] public float amplitude;
    [Range(0, 0.5f)] public float shakeLength;

    [Header("Particle Effects")]
    public GameObject spawnEffect;
    public GameObject hitEffect;

    public Move(float _windupFrame, float _hitFrame, float _recoverFrame, float _totalFrames, float _pushFrames,
                float _pushX,       float _pushY,    float _decceleration,
                float _knockbackX,  float _knockbackY,
                GameObject _spawnEffect, GameObject _hitEffect)
    {
        windupFrame  = _windupFrame;
        hitFrame     = _hitFrame;
        recoverFrame = _recoverFrame;
        totalFrames  = _totalFrames;
        pushFrames   = _pushFrames;

        pushX = _pushX;
        pushY = _pushY;
        decceleration = _decceleration;

        knockbackX = _knockbackX;
        knockbackY = _knockbackY;

        spawnEffect = _spawnEffect;
        hitEffect = _hitEffect;
    }
}