using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimController : WeaponAim
{
    private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(
            Mouse.current.position.ReadValue()
        );

        AimWeaponAtDirection(mousePosition);

    }
}
