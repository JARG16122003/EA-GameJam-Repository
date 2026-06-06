using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float movementeSpeed = 20.0f;
    [SerializeField]
    private InputActionReference moveAction;

    private Rigidbody2D playerRigidbody2D;
    private Vector2 moveDirection;
    private Animator animator;

    private void Awake()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementValue();
        updateAnimations();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void GetMovementValue()
    {
        if (moveAction == null) return;
        moveDirection = moveAction.action.ReadValue<Vector2>();
    }

    private void ApplyMovement()
    {
        playerRigidbody2D.linearVelocity = new Vector2(moveDirection.x * movementeSpeed,
                                                       moveDirection.y * movementeSpeed);
    }

    private void updateAnimations()
    {
        animator.SetBool("isMoving", moveDirection != Vector2.zero);

        animator.SetFloat("moveX", moveDirection.x);
        animator.SetFloat("moveY", moveDirection.y);
    }
}
