using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [Header("Float Settings")]
    [Range(0f, 1f)]
    [SerializeField] 
    protected float amplitude = 0.25f; // Altura 
    [Range(0f, 10f)]
    [SerializeField] 
    protected float frequency = 1f;    // Velocidad

    private Vector3 startPosition;

    protected virtual void Start()
    {
        startPosition = transform.position;
    }

    
    protected virtual void Update()
    {
        HandleFloating();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
    }


    private void HandleFloating()
    {
        float offsetY = Mathf.Sin(Time.time * frequency) * amplitude;

        transform.position = startPosition + Vector3.up * offsetY;
    }

    protected virtual void ApplyLogicItem(GameObject player)
    {
        Destroy(gameObject, 0.2f);
    }

}
