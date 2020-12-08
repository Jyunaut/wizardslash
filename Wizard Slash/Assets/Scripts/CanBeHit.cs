using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CanBeHit : MonoBehaviour
{
    [Serializable]
    public class DamageEvent : UnityEvent<int> {}

    [Serializable]
    public class KnockbackEvent : UnityEvent<float, float, int> {}

    [Serializable]
    public class ScreenShakeEvent : UnityEvent<float, float> {}
    
    [Serializable]
    public class TimeStopEvent : UnityEvent<float, float> {}

    public DamageEvent OnTakeDamage;
    public KnockbackEvent OnKnockback;
    public ScreenShakeEvent OnScreenShake;
    public TimeStopEvent OnTimeStop;

    private Actor actor;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    private GameObject cinemachine;

    private bool isKnockedback;
    private float knockbackTime;
    private float curTime;
    private Vector2 velocity;

    void Start()
    {
        actor = GetComponent<Actor>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        cinemachine = GameObject.FindGameObjectWithTag("Cinemachine");
    }

    private void ResetMaterialShader()
    {
        spriteRenderer.material.color = Color.white;
    }

    public void TakeDamage(int inflictedDamage)
    {
        spriteRenderer.material.color = new Color(1, 0.5f, 0.5f, 1); // TODO: Eventually replace this with a less-blinding white
        Invoke("ResetMaterialShader", 0.02f);
    }

    public void GetKnockBacked(float knockbackForceX, float knockbackForceY, int knockbackDirection)
    {
        if (!rigidbody2d)
            return;

        isKnockedback = true;
        knockbackTime = Time.fixedTime + 0.5f;
        rigidbody2d.velocity = new Vector2(knockbackDirection * knockbackForceX * (1 - actor.knockbackResist), knockbackForceY * (1 - actor.knockbackResist));
    }

    public void ShakeScreen(float amplitude, float shakeLength)
    {
        cinemachine.GetComponent<CameraEffects>().ShakeScreen(amplitude, shakeLength);
    }

    public void TimeSlow(float percent, float length)
    {
        Time.timeScale = 1 - percent;
        StartCoroutine("ResetTimeScale", length);
    }

    IEnumerator ResetTimeScale(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        Time.timeScale = 1f;
    }

    void FixedUpdate()
    {
        if (isKnockedback && Time.fixedTime <= knockbackTime)
        {
            rigidbody2d.velocity = Vector2.SmoothDamp(rigidbody2d.velocity, new Vector2(0, rigidbody2d.velocity.y), ref velocity, 0.5f);
        }
        else if (isKnockedback && Time.fixedTime > knockbackTime)
        {
            isKnockedback = false;
            knockbackTime = 0;
            rigidbody2d.velocity = new Vector2(0, rigidbody2d.velocity.y);
        }
    }
}