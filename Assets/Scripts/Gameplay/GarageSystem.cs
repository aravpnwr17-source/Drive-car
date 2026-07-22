using UnityEngine;

/// <summary>
/// Player garage - Vehicle selection and management
/// </summary>
public class GarageSystem : MonoBehaviour
{
    [System.Serializable]
    public class Vehicle
    {
        public int id;
        public string name;
        public float maxSpeed;
        public float acceleration;
        public float handling;
        public string prefabPath;
        public bool isOwned;
        public float price;
    }

    [SerializeField] private Vehicle[] availableVehicles = new Vehicle[10];
    private int selectedVehicleIndex = 0;

    private void Start()
    {
        InitializeVehicles();
    }

    /// <summary>
    /// Initialize available vehicles
    /// </summary>
    private void InitializeVehicles()
    {
        if (availableVehicles.Length > 0)
        {
            availableVehicles[0] = new Vehicle
            {
                id = 1,
                name = "Sports Car",
                maxSpeed = 250f,
                acceleration = 8f,
                handling = 8f,
                price = 50000f,
                isOwned = true
            };

            availableVehicles[1] = new Vehicle
            {
                id = 2,
                name = "Truck",
                maxSpeed = 150f,
                acceleration = 5f,
                handling = 4f,
                price = 30000f,
                isOwned = false
            };
        }
    }

    /// <summary>
    /// Select a vehicle
    /// </summary>
    public void SelectVehicle(int vehicleIndex)
    {
        if (vehicleIndex < 0 || vehicleIndex >= availableVehicles.Length) return;
        if (!availableVehicles[vehicleIndex].isOwned) return;

        selectedVehicleIndex = vehicleIndex;
        Debug.Log($"Selected: {availableVehicles[vehicleIndex].name}");
    }

    /// <summary>
    /// Purchase a vehicle
    /// </summary>
    public bool PurchaseVehicle(int vehicleIndex, int playerMoney)
    {
        if (vehicleIndex < 0 || vehicleIndex >= availableVehicles.Length) return false;
        if (availableVehicles[vehicleIndex].isOwned) return false;
        if (playerMoney < availableVehicles[vehicleIndex].price) return false;

        availableVehicles[vehicleIndex].isOwned = true;
        Debug.Log($"Purchased: {availableVehicles[vehicleIndex].name}");
        return true;
    }

    /// <summary>
    /// Get selected vehicle
    /// </summary>
    public Vehicle GetSelectedVehicle()
    {
        return availableVehicles[selectedVehicleIndex];
    }
}
