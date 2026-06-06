using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private InputActionReference shootAction;
    [SerializeField]
    private GameObject equippedWeapon;

    private IWeapon interfaceWeapon;

    [SerializeField]
    private float AmountAmmo = 10;

    private void Start()
    {
        if (equippedWeapon == null) return;

        interfaceWeapon = equippedWeapon.GetComponent<IWeapon>();

        if(interfaceWeapon == null) return;

        interfaceWeapon.onWeaponShot += DecreaseAmmo;
        
    }

    private void OnEnable()
    {
        if (shootAction == null) return;
        shootAction.action.started += ShootWeapon;
        shootAction.action.canceled += StopShootingWeapon;
    }


    private void ShootWeapon(InputAction.CallbackContext context)
    {
        if (interfaceWeapon == null || AmountAmmo == 0) return;

        interfaceWeapon.Shoot();

    }

    private void StopShootingWeapon(InputAction.CallbackContext context)
    {
        if (interfaceWeapon == null) return;

        interfaceWeapon.StopShooting();

    }

    private void DecreaseAmmo()
    {
        AmountAmmo = Mathf.Clamp(AmountAmmo - 1, 0, 100);

        if (AmountAmmo == 0) interfaceWeapon.StopShooting();
    }

}
