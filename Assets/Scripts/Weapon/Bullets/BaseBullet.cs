using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    [SerializeField]
    protected float BaseDamage = 20.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IAttributes iAttributesCharacter = collision.GetComponent<IAttributes>();

        if (iAttributesCharacter == null) return;
        
        iAttributesCharacter.ReceiveDamage(BaseDamage);

        Destroy(gameObject);
    }
}
