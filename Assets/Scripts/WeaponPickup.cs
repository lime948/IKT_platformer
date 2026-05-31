using UnityEngine;

/// <summary>
/// Attach to every weapon GameObject that exists in the world.
///
/// Setup checklist for each world weapon:
///   1. Add this script.
///   2. Add a Collider → set "Is Trigger" = true (acts as the pickup radius).
///   3. Add a Rigidbody → leave kinematic = false so it rests on the ground.
///   4. Weapon.cs enabled state is managed entirely by WeaponHolder — don't touch it here.
/// </summary>
public class WeaponPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    [Tooltip("How close the player must be to see the prompt and press E.")]
    public float pickupRadius = 2.5f;

    [Header("UI Prompt (optional)")]
    [Tooltip("A world-space Canvas/GameObject showing 'Press E to pick up'. Leave null to skip.")]
    public GameObject pickupPromptUI;

    // -----------------------------------------------------------------------
    // Private state
    // -----------------------------------------------------------------------

    private WeaponHolder playerHolder;
    private bool playerInRange;

    // -----------------------------------------------------------------------
    // Unity messages
    // -----------------------------------------------------------------------

    private void Awake()
    {
        // Do NOT touch Weapon.enabled here.
        // WeaponHolder is the only thing that should enable/disable the Weapon component.
        HidePrompt();
    }

    private void Update()
    {
        if (playerHolder == null) return;

        float dist = Vector3.Distance(transform.position, playerHolder.transform.position);
    
        if (dist <= pickupRadius)
        {
            ShowPrompt();
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerHolder.PickUp(gameObject);
            }
        }
        else
        {
            HidePrompt();
        }
    }

    // -----------------------------------------------------------------------
    // Triggers
    // -----------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        WeaponHolder holder = other.GetComponentInParent<WeaponHolder>();
        if (holder == null) return;

        playerHolder = holder; // just grab the reference, let Update handle the rest
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<WeaponHolder>() == null) return;
        playerHolder = null;
        HidePrompt();
    }
    // -----------------------------------------------------------------------
    // Helpers
    // -----------------------------------------------------------------------

    private void LeaveRange()
    {
        playerHolder = null;
        playerInRange = false;
        HidePrompt();
    }

    private void ShowPrompt()
    {
        if (pickupPromptUI != null) pickupPromptUI.SetActive(true);
    }

    private void HidePrompt()
    {
        if (pickupPromptUI != null) pickupPromptUI.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
