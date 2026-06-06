using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;


    private Camera mainCamera;

    private Vector3 originalScale;

    private bool isFacingRight = true;

    private void Awake()
    {
        mainCamera = Camera.main;
        originalScale = transform.localScale;
    }

    private void Update()
    {
        FlipCharacterBasedOnMouse();
    }

    private void FlipCharacterBasedOnMouse()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(
        Mouse.current.position.ReadValue()
    );

        isFacingRight = mouseWorld.x > transform.position.x;

        spriteRenderer.flipX = !isFacingRight;
    }
}
