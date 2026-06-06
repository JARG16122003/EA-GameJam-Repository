using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour , IInventory
{
    [SerializeField]
    private InputActionReference shootAction;
    [SerializeField]
    private GameObject equippedWeapon;
    [SerializeField]
    private TextMeshProUGUI contadorBalas;

    private IWeapon interfaceWeapon;

    [SerializeField]
    private float AmountAmmo = 10;


    void Start()
    {
        UpdateUI();
        if (equippedWeapon == null) return;

        interfaceWeapon = equippedWeapon.GetComponent<IWeapon>();

        if(interfaceWeapon == null) return;

        interfaceWeapon.onWeaponShot += DecreaseAmmo;
        
    }

    public void IncreaseAmmo(float ammountAmmo)
    {
        AmountAmmo = Mathf.Clamp(AmountAmmo + ammountAmmo, 0, 100);
        UpdateUI();
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
        UpdateUI();

        if (AmountAmmo == 0) interfaceWeapon.StopShooting();
    }

    private void UpdateUI()
    {
        if (contadorBalas == null) return;

        contadorBalas.text = (AmountAmmo).ToString();
    }

}
