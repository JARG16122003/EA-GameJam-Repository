using UnityEngine;

public class CloudFloat : MonoBehaviour
{
    public float speed = 1f;
    public float amplitude = 0.2f;
    public float frequency = 1f;

    public float leftLimit = -12f;
    public float rightLimit = 12f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float x = transform.position.x + speed * Time.deltaTime;
        float y = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

        transform.position = new Vector3(x, y, startPos.z);

        if (transform.position.x > rightLimit)
        {
            transform.position = new Vector3(
                leftLimit,
                transform.position.y,
                transform.position.z
            );

            startPos = transform.position;
        }
    }
}