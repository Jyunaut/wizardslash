using System;
using UnityEngine;
using UnityEngine.Events;

public class CanBeHit : MonoBehaviour
{
    [Serializable]
    public class DamageEvent : UnityEvent<int> {}

    [Serializable]
    public class KnockbackEvent : UnityEvent<float, float, int> {}

    [Serializable]
    public class ScreenShakeEvent : UnityEvent<float, float> {}

    public DamageEvent OnTakeDamage;
    public KnockbackEvent OnKnockback;
    public ScreenShakeEvent OnScreenShake;

    Rigidbody2D rigidbody2d;
    private bool isKnockedback;
    private float knockbackTime;
    private Vector2 velocity;
    private SpriteRenderer spriteRenderer;

    GameObject cinemachine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        cinemachine = GameObject.FindGameObjectWithTag("Cinemachine");
    }

    private void ResetMaterialShader()
    {
        spriteRenderer.material.shader = Shader.Find("Sprites/Default");
    }

    public void TakeDamage(int inflictedDamage)
    {
        spriteRenderer.material.shader = Shader.Find("GUI/Text Shader"); // TODO: Eventually replace this with a less-blinding white
        Invoke("ResetMaterialShader", 0.02f);
    }

    public void GetKnockBacked(float knockbackForceX, float knockbackForceY, int knockbackDirection)
    {
        if (!rigidbody2d)
            return;

        isKnockedback = true;
        knockbackTime = Time.fixedTime + 0.5f;
        rigidbody2d.velocity = new Vector2(knockbackDirection * knockbackForceX, knockbackForceY);
    }

    public void ShakeScreen(float amplitude, float shakeLength)
    {
        cinemachine.GetComponent<CameraEffects>().ShakeScreen(amplitude, shakeLength);
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