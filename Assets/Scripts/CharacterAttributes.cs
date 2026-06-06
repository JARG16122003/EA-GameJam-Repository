using UnityEngine;
using UnityEngine.UI;


public class CharacterAttributes : MonoBehaviour , IAttributes
{
    [SerializeField]
    protected Image healthBar;

    protected float health = 100.0f;

    private float maxValueHealth = 100.0f;
    
    void Start()
    {
        if (healthBar != null) return;
        healthBar = GetComponentInChildren<Image>();
    }

    public virtual void ReceiveDamage(float amountDamage)
    {
        health = Mathf.Clamp(health - amountDamage, 0.0f, maxValueHealth);

        healthBar.enabled = true;
        healthBar.fillAmount = health / maxValueHealth;

        VerifyHealth();
    }

    public virtual void VerifyHealth()
    {
        if(health == 0.0f)
        {
            Destroy(gameObject); // Logica provisional mientras se implementa animacion muerte
        }
    }
}
