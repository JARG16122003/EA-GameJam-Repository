using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponAim : MonoBehaviour
{


    protected void AimWeaponAtDirection(Vector3 position)
    {
        Vector2 direction = position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (angle > 90 || angle < -90)
        {
            transform.localScale = new Vector3(1,-1,1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
