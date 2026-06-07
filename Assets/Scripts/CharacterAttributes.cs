using UnityEngine;
using UnityEngine.UI;


public class CharacterAttributes : MonoBehaviour , IAttributes
{
    [SerializeField]
    protected Image healthBar;
    [SerializeField]
    protected float health = 100.0f;
    [SerializeField]
    protected bool shouldDestroyCharacter = true;

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

        if (healthBar != null && healthBar.enabled == false) healthBar.enabled = true;

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
        if(health == 0.0f)
        {
            Destroy(gameObject); // Logica provisional mientras se implementa animacion muerte
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null) return;
        healthBar.fillAmount = health / maxValueHealth;
    }

}
