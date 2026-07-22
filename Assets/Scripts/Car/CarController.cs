using UnityEngine;

/// <summary>
/// Main car controller script handling physics and input
/// </summary>
public class CarController : MonoBehaviour
{
    [Header("Car Stats")]
    [SerializeField] private float maxSpeed = 200f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float brakePower = 100f;
    [SerializeField] private float steeringSpeed = 5f;
    [SerializeField] private float maxSteeringAngle = 45f;
    [SerializeField] private float driftFactor = 1.5f;

    [Header("Physics")]
    [SerializeField] private float mass = 1500f;
    [SerializeField] private float dragCoefficient = 0.3f;
    [SerializeField] private float engineTorque = 5000f;
    [SerializeField] private float fuelConsumption = 0.5f;
    [SerializeField] private float maxFuel = 60f;

    [Header("Wheels")]
    [SerializeField] private WheelCollider[] wheelColliders = new WheelCollider[4];
    [SerializeField] private Transform[] wheelMeshes = new Transform[4];

    [Header("Engine")]
    [SerializeField] private AudioSource engineSound;
    [SerializeField] private AudioClip idleSound;
    [SerializeField] private AudioClip driveSound;

    private Rigidbody carRigidbody;
    private float currentSpeed = 0f;
    private float currentSteeringAngle = 0f;
    private float currentFuel;
    private bool isHandBraking = false;
    private float damageLevel = 0f;
    private float speedMultiplier = 1f;

    private void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        currentFuel = maxFuel;
        SetupWheels();
    }

    private void Update()
    {
        HandleInput();
        UpdateSpeedometer();
        UpdateFuel();
    }

    private void FixedUpdate()
    {
        ApplyPhysics();
        UpdateWheels();
    }

    /// <summary>
    /// Handle player input for acceleration, braking, steering
    /// </summary>
    private void HandleInput()
    {
        // Forward/Backward
        float accelerationInput = 0f;
        if (Input.GetKey(KeyCode.W))
            accelerationInput = 1f;
        else if (Input.GetKey(KeyCode.S))
            accelerationInput = -1f;

        // Steering
        float steeringInput = 0f;
        if (Input.GetKey(KeyCode.A))
            steeringInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            steeringInput = 1f;

        // Handbrake
        if (Input.GetKey(KeyCode.Space))
            isHandBraking = true;
        else
            isHandBraking = false;

        // Apply steering
        currentSteeringAngle = steeringInput * maxSteeringAngle;
        currentSteeringAngle = Mathf.Lerp(currentSteeringAngle, steeringInput * maxSteeringAngle, Time.deltaTime * steeringSpeed);

        // Apply acceleration/braking
        if (accelerationInput > 0 && currentFuel > 0)
        {
            currentSpeed = Mathf.Min(currentSpeed + acceleration * accelerationInput * Time.deltaTime, maxSpeed * speedMultiplier);
        }
        else if (accelerationInput < 0)
        {
            currentSpeed = Mathf.Max(currentSpeed + brakePower * accelerationInput * Time.deltaTime, 0f);
        }
        else if (!isHandBraking)
        {
            // Natural deceleration
            currentSpeed = Mathf.Max(currentSpeed - (currentSpeed * 0.02f), 0f);
        }

        // Handbrake deceleration
        if (isHandBraking && currentSpeed > 0)
        {
            currentSpeed = Mathf.Max(currentSpeed - brakePower * 0.5f * Time.deltaTime, 0f);
        }
    }

    /// <summary>
    /// Apply physics forces to the car
    /// </summary>
    private void ApplyPhysics()
    {
        // Calculate drag
        float dragForce = currentSpeed * currentSpeed * dragCoefficient;
        
        // Apply forward force
        Vector3 forceDirection = transform.forward;
        float appliedForce = currentSpeed - dragForce;

        carRigidbody.velocity = forceDirection * (currentSpeed / 3.6f); // Convert to m/s

        // Apply steering rotation
        if (currentSpeed > 0.1f)
        {
            float rotationAmount = currentSteeringAngle * (currentSpeed / maxSpeed) * Time.deltaTime;
            carRigidbody.angularVelocity = new Vector3(0, rotationAmount * 10f, 0);
        }
    }

    /// <summary>
    /// Update wheel collider and mesh positions/rotations
    /// </summary>
    private void UpdateWheels()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            if (wheelColliders[i] == null) continue;

            // Set steering for front wheels
            if (i < 2)
                wheelColliders[i].steerAngle = currentSteeringAngle;

            // Apply motor torque to rear wheels
            if (i > 1)
            {
                wheelColliders[i].motorTorque = engineTorque * (currentSpeed / maxSpeed);
            }

            // Apply brakes
            if (isHandBraking)
                wheelColliders[i].brakeTorque = brakePower;
            else
                wheelColliders[i].brakeTorque = 0;

            // Update wheel mesh position and rotation
            WheelHit wheelHit;
            wheelColliders[i].GetGroundHit(out wheelHit);

            if (wheelMeshes[i] != null)
            {
                wheelMeshes[i].position = wheelHit.point;
                wheelMeshes[i].Rotate(Vector3.right, wheelColliders[i].rpm / 60f * 360f * Time.fixedDeltaTime, Space.Self);
            }
        }
    }

    /// <summary>
    /// Manage fuel consumption
    /// </summary>
    private void UpdateFuel()
    {
        if (currentSpeed > 0)
            currentFuel = Mathf.Max(currentFuel - (fuelConsumption * Time.deltaTime), 0f);

        if (currentFuel <= 0)
            currentSpeed = 0f;
    }

    /// <summary>
    /// Setup wheel colliders initial properties
    /// </summary>
    private void SetupWheels()
    {
        foreach (WheelCollider wheel in wheelColliders)
        {
            if (wheel == null) continue;

            wheel.mass = mass / 4f;
            wheel.suspensionDistance = 0.3f;
            
            WheelFrictionCurve friction = wheel.forwardFriction;
            friction.extremumValue = 1f;
            friction.asymptoteValue = 0.5f;
            wheel.forwardFriction = friction;

            WheelFrictionCurve sideFriction = wheel.sidewaysFriction;
            sideFriction.extremumValue = 0.8f;
            sideFriction.asymptoteValue = 0.6f;
            wheel.sidewaysFriction = sideFriction;
        }
    }

    /// <summary>
    /// Update speedometer and display
    /// </summary>
    private void UpdateSpeedometer()
    {
        // Can be used to update UI
        Debug.Log($"Speed: {currentSpeed:F0} km/h | Fuel: {currentFuel:F1}L | Damage: {damageLevel:F0}%");
    }

    /// <summary>
    /// Apply damage to the car
    /// </summary>
    public void ApplyDamage(float damageAmount)
    {
        damageLevel = Mathf.Min(damageLevel + damageAmount, 100f);
        speedMultiplier = 1f - (damageLevel / 100f * 0.5f);

        if (damageLevel >= 100f)
            currentSpeed = 0f;
    }

    /// <summary>
    /// Repair the car
    /// </summary>
    public void Repair()
    {
        damageLevel = 0f;
        speedMultiplier = 1f;
    }

    /// <summary>
    /// Refuel the car
    /// </summary>
    public void Refuel(float amount)
    {
        currentFuel = Mathf.Min(currentFuel + amount, maxFuel);
    }

    // Getters
    public float GetCurrentSpeed() => currentSpeed;
    public float GetMaxSpeed() => maxSpeed;
    public float GetFuel() => currentFuel;
    public float GetMaxFuel() => maxFuel;
    public float GetDamage() => damageLevel;
    public bool IsHandBraking() => isHandBraking;
}
