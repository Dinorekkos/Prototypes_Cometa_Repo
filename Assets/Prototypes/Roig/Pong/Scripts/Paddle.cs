using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : NetworkBehaviour
{
    public bool isPlayer1;
    public float speed;
    public Rigidbody2D rb;

    [SerializeField] private PongControllers controllerMovement;

    PongControllers controls;
    PongControllers.MovementActions normalMovement;

    private Vector2 moveInput;

    void Awake()
    {
        controls = new PongControllers();
        normalMovement = controls.Movement;

        normalMovement.Move.performed += OnInput;
        normalMovement.Move.canceled += OnCancelled;
    }

    private void OnInput(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnCancelled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }


    void Update()
    {
        if (!IsOwner) return;
        rb.velocity = new Vector2(rb.velocity.x, moveInput.y * speed);
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }
}
