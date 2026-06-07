using UnityEngine;

public class camara: MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 3f;

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 destino =
            new Vector3(
                target.position.x,
                target.position.y,
                transform.position.z);

        transform.position =
            Vector3.Lerp(
                transform.position,
                destino,
                Time.deltaTime * smoothSpeed);
    }
}