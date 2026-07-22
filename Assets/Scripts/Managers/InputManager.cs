using UnityEngine;

/// <summary>
/// Input manager - Centralized input handling
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [Header("Input Sensitivity")]
    [SerializeField] private float steeringSensitivity = 1f;
    [SerializeField] private float cameraSensitivity = 2f;

    private Vector2 movementInput;
    private Vector2 cameraInput;
    private bool isHandBraking;
    private bool isPausing;
    private bool isInteracting;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        HandleMovementInput();
        HandleCameraInput();
        HandleActionInput();
    }

    /// <summary>
    /// Get movement input (WASD)
    /// </summary>
    private void HandleMovementInput()
    {
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");
    }

    /// <summary>
    /// Get camera input (Mouse)
    /// </summary>
    private void HandleCameraInput()
    {
        cameraInput.x = Input.GetAxis("Mouse X") * cameraSensitivity;
        cameraInput.y = Input.GetAxis("Mouse Y") * cameraSensitivity;
    }

    /// <summary>
    /// Get action input (Space, E, ESC)
    /// </summary>
    private void HandleActionInput()
    {
        isHandBraking = Input.GetKey(KeyCode.Space);
        isPausing = Input.GetKeyDown(KeyCode.Escape);
        isInteracting = Input.GetKeyDown(KeyCode.E);
    }

    // Getters
    public Vector2 GetMovementInput() => movementInput;
    public Vector2 GetCameraInput() => cameraInput;
    public bool IsHandBraking() => isHandBraking;
    public bool IsPausing() => isPausing;
    public bool IsInteracting() => isInteracting;
}
