using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAttributes : MonoBehaviour, IAttributes
{
    public event Action onCharacterDead;

    [SerializeField]
    private Animator characterAnimator;
    [SerializeField]
    protected Image healthBar;
    [SerializeField]
    protected float health = 100.0f;
    [SerializeField]
    private string deadAnimName;
    [SerializeField]
    private float timeToDestroy;
    [SerializeField]
    private AudioClip deathSound;

    private bool isDead = false;
    private float maxValueHealth = 100.0f;

    void Start()
    {
        maxValueHealth = health;
        if (healthBar != null) return;
        healthBar = GetComponentInChildren<Image>();
    }

    public virtual void ReceiveDamage(float amountDamage)
    {
        health = Mathf.Clamp(health - amountDamage, 0.0f, maxValueHealth);

        if (healthBar != null && healthBar.enabled == false)
            healthBar.enabled = true;

        UpdateHealthBar();

        VerifyHealth();
    }

    public virtual void CureHealth(float amountHealth)
    {
        health = Mathf.Clamp(health + amountHealth, 0.0f, maxValueHealth);

        UpdateHealthBar();
    }

    protected virtual void VerifyHealth()
    {
        if (health == 0.0f && !isDead)
        {
            isDead = true;
            onCharacterDead?.Invoke();

            if (deathSound != null)
            {
                AudioSource.PlayClipAtPoint(deathSound, transform.position);
            }

            StartAnimation(deadAnimName, characterAnimator);
            Destroy(gameObject, timeToDestroy);
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null) return;
        healthBar.fillAmount = health / maxValueHealth;
    }

    private void StartAnimation(string nameAnimation, Animator animator)
    {
        if (animator != null)
        {
            animator.CrossFade(nameAnimation, 0.2f);
        }
    }
}