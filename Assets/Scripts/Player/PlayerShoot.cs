using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private InputActionReference shootAction;
    [SerializeField]
    private GameObject equippedWeapon;

    private IWeapon interfaceWeapon;

    private void Start()
    {
        if (equippedWeapon == null) return;

        interfaceWeapon = equippedWeapon.GetComponent<IWeapon>();
    }

    private void OnEnable()
    {
        if (shootAction == null) return;
        shootAction.action.started += ShootWeapon;
        shootAction.action.canceled += StopShootingWeapon;
    }


    private void ShootWeapon(InputAction.CallbackContext context)
    {
        if (interfaceWeapon == null) return;

        interfaceWeapon.Shoot();

    }

    private void StopShootingWeapon(InputAction.CallbackContext context)
    {
        if (interfaceWeapon == null) return;

        interfaceWeapon.StopShooting();

    }

}
