using UnityEngine;

/// <summary>
///  Frontend script that actual in-game through raycasting, positioning item, attached to the camera, passing it to PlayerInventory class
/// </summary>
[RequireComponent(typeof(Camera))]
public class PlayerPickup : MonoBehaviour
{
    [SerializeField, Range(.5f, 10f)] float pickupDistance;
    [SerializeField] LayerMask pickupMask;
    [SerializeField] KeyCode pickupKey;
    [SerializeField] KeyCode throwKey;

    private PlayerInventory inventory;
    [SerializeField] Transform itemHolder;

    void Start()
    {
        inventory = GetComponent<PlayerInventory>();
    }

    void OnValidate() //warning
    {
        Camera cam = GetComponent<Camera>();

        if (cam != null)
            return;

        Debug.LogWarning("It is recommended to place the player pickup script on the camera itself.");
    }

    void Update()
    {
        HandlePickup();
        HandleThrowing();
    }

    void HandlePickup()
    {
        Vector3 rayOrigin = transform.position;
        Vector3 rayDir = transform.forward;

        RaycastHit hit;

        if (!Physics.Raycast(rayOrigin, rayDir, out hit, pickupDistance, pickupMask))
            return;

        if (!Input.GetKeyDown(pickupKey))
            return;

        Transform objectHit = hit.collider.transform;

        inventory.AddItemToSlot(objectHit.gameObject);
        PositionItem(objectHit);
    }

    void HandleThrowing()
    {
        if (!Input.GetKeyDown(throwKey))
            return;

        if (!inventory.CheckIfItemEquipped())
            return;

        ThrowItem(inventory.GetActiveItem().transform);

    }

    void PositionItem(Transform item)
    {
        Rigidbody rb = item.GetComponent<Rigidbody>();
        Equipment equipment = item.GetComponent<Equipment>();

        //set parent of item to item holder
        item.SetParent(itemHolder);

        //reset location
        item.localPosition = Vector3.zero; 

        //set position of item to item holder
        item.localPosition = equipment.GetOffset();
        
        //set rotation of item to item holder
        item.forward = itemHolder.forward;

        //make rigidbody kinematic so it doesn't fall
        rb.isKinematic = true;
    }

    void ThrowItem(Transform item)
    {
        Rigidbody rb = item.GetComponent<Rigidbody>();

        //add some force to notion throwing item
        rb.AddForce(transform.forward * 20f); 

        //unparent item from item holder
        item.SetParent(null);

        //make rigidbody not kinematic 
        rb.isKinematic = false;

        //unequip item from inventory
        inventory.RemoveItemFromSlot();
    }
}
