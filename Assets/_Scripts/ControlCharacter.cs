using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 4.0f;
    public float sprintSpeed = 6.4f;
    public float crouchSpeed = 2.0f;
    public float gravity = -9.81f; // Añadimos gravedad para que detecte el suelo

    [Header("Estamina")]
    public float maxStamina = 100f;
    public float staminaDrain = 25f;
    public float staminaRegen = 15f;
    
    // CAMBIO APLICADO: Ahora es public para que la UI pueda leerla
    public float currentStamina; 
    
    private bool isFatigued = false;

    [Header("Cámara (Mouse Look)")]
    public Transform playerCamera;
    public float mouseSensitivity = 200f;
    private float xRotation = 0f;

    private CharacterController controller;
    private Vector3 velocity; // Almacena la velocidad de caída

    public enum MovementState { Idle, Walking, Running, Crouching }
    public MovementState currentState { get; private set; }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // --------------------------------------------------------
        // FIX DE PAUSA: Si el juego está pausado, salimos del Update
        // para que la cámara no se mueva ni se gaste estamina.
        // --------------------------------------------------------
        if (MenuPausa.JuegoPausado)
        {
            return;
        }

        HandleMouseLook();
        HandleMovement();
        HandleStamina();
    }

    void HandleMouseLook()
    {
        if (playerCamera == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        // 1. Aplicar fuerza para mantener al personaje pegado al suelo
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 2. Leer las teclas de dirección (WASD)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        float currentSpeed = walkSpeed;

        // 3. Determinar el Estado y la Velocidad (Con las teclas invertidas)
        if (Input.GetKey(KeyCode.LeftShift)) // SHIFT para Agacharse
        {
            currentSpeed = crouchSpeed;
            currentState = MovementState.Crouching;
            controller.height = 1.0f;
        }
        else if (Input.GetKey(KeyCode.LeftControl) && currentStamina > 0 && !isFatigued && move.magnitude > 0.1f) // CONTROL para Correr
        {
            currentSpeed = sprintSpeed;
            currentState = MovementState.Running;
            controller.height = 2.0f;
        }
        else if (move.magnitude > 0.1f) // Caminar por defecto
        {
            currentSpeed = isFatigued ? walkSpeed * 0.6f : walkSpeed;
            currentState = MovementState.Walking;
            controller.height = 2.0f;
        }
        else // Quieto
        {
            currentState = MovementState.Idle;
            controller.height = 2.0f; // Recupera la altura si se levanta estando quieto
        }

        // 4. Mover al personaje en los ejes X y Z
        controller.Move(move * currentSpeed * Time.deltaTime);

        // 5. Aplicar la gravedad en el eje Y
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleStamina()
    {
        if (currentState == MovementState.Running)
        {
            currentStamina -= staminaDrain * Time.deltaTime;
            if (currentStamina <= 0)
            {
                currentStamina = 0;
                StartCoroutine(FatigueDebuff());
            }
        }
        else
        {
            if (currentStamina < maxStamina && !isFatigued)
                currentStamina += staminaRegen * Time.deltaTime;
        }
    }

    System.Collections.IEnumerator FatigueDebuff()
    {
        isFatigued = true;
        yield return new WaitForSeconds(4.0f);
        isFatigued = false;
    }
}