using UnityEngine;

/// <summary>
/// Attach to the Player. Manages the currently equipped weapon.
/// Assign weaponHoldPoint to an empty child Transform where weapons will be parented
/// (e.g. a "WeaponHoldPoint" object positioned at the player's hands/camera).
/// </summary>
public class WeaponHolder : MonoBehaviour
{
    [Header("References")]
    public Transform weaponHoldPoint;   // Where the held weapon sits (child of camera or player)
    public Camera playerCamera;         // Passed to the Weapon script on pickup

    [Header("Starting Weapon")]
    public GameObject startingWeapon; // drag your held gun prefab/GO here in the Inspector

    private void Start()
    {
        if (startingWeapon != null)
            PickUp(startingWeapon);
    }

    [Header("Drop Settings")]
    public float dropForce = 5f;        // How hard the dropped weapon is tossed forward
    public float dropTorque = 2f;       // A little spin so it looks natural

    /// <summary>The weapon GameObject currently held by the player (null if unarmed).</summary>
    public GameObject CurrentWeapon { get; private set; }

    // -----------------------------------------------------------------------
    // Public API (called by WeaponPickup)
    // -----------------------------------------------------------------------

    /// <summary>
    /// Equip a new weapon. Drops the current one first if there is one.
    /// Returns the dropped weapon GameObject (or null if hands were empty).
    /// </summary>
    public GameObject PickUp(GameObject newWeaponGO)
    {
        GameObject dropped = null;

        if (CurrentWeapon != null)
            dropped = Drop();

        Equip(newWeaponGO);
        return dropped;
    }

    /// <summary>Drop the current weapon into the world. Returns the dropped GO.</summary>
    public GameObject Drop()
    {
        if (CurrentWeapon == null) return null;

        GameObject dropped = CurrentWeapon;
        Weapon weaponScript = dropped.GetComponent<Weapon>();

        // Detach from hold point
        dropped.transform.SetParent(null);

        // Re-enable physics
        Rigidbody rb = dropped.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(playerCamera.transform.forward * dropForce, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * dropTorque, ForceMode.Impulse);
        }

        // Re-enable the pickup collider so the player can grab it again
        Collider col = dropped.GetComponent<Collider>();
        if (col != null) col.enabled = true;

        // Let the weapon know it's no longer active
        if (weaponScript != null) weaponScript.enabled = false;

        // Re-enable the pickup component so the world weapon is interactive again
        WeaponPickup pickup = dropped.GetComponent<WeaponPickup>();
        if (pickup != null) pickup.enabled = true;

        CurrentWeapon = null;
        return dropped;
    }

    // -----------------------------------------------------------------------
    // Private helpers
    // -----------------------------------------------------------------------

    private void Equip(GameObject weaponGO)
    {
        CurrentWeapon = weaponGO;

        // Parent to hold point and zero local transform
        weaponGO.transform.SetParent(weaponHoldPoint);
        weaponGO.transform.localPosition = Vector3.zero;
        weaponGO.transform.localRotation = Quaternion.identity;

        // Disable physics while held
        Rigidbody rb = weaponGO.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // Disable pickup collider so we don't immediately re-trigger pickup
        Collider col = weaponGO.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // Wire up the camera reference and enable shooting
        Weapon weaponScript = weaponGO.GetComponent<Weapon>();
        if (weaponScript != null)
        {
            weaponScript.playerCamera = playerCamera;
            weaponScript.enabled = true;
        }

        // Disable the pickup component while held
        WeaponPickup pickup = weaponGO.GetComponent<WeaponPickup>();
        if (pickup != null) pickup.enabled = false;
    }
}
