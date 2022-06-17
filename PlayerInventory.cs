using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

///<summary>
/// Backend script that handles equipments, slots, switching equipment, not necessarily attached to camera
///</summary>
public class PlayerInventory : MonoBehaviour
{
    [SerializeField] GameObject [] inventory;
    int index;

    void Start()
    {
        if (inventory.Length == 0) //check if inventory has 0 slots
            throw new ArgumentException("Inventory has 0 slots!");
    }

    public void TryToEquip(GameObject go) //method linked with PlayerPickup to pick up the object and handle the logic related to it.
    {
        if (!AnyFreeSlots()) //if no free slots in inventory, we cannot equip ANYTHING. Therefore, print message and simply return (stop) code
        {
            Debug.Log("No free slots in inventory!");

            return;
        }

        for (int i = 0; i < inventory.Length; i++) // for loop running through objectsInInventory array
        {
            if (inventory[i] != null) //if the slot is equipped, move to check the next slot
                continue;

            if (go.GetComponent<Equipment>().GetEquipped()) //if object is already equipped, we of course can't equip it again
                break;
            
            AddItemToInventory(go, i); //if slot isn't equipped, equip item at that slot and break out of the loop so as to not equip the same thing in the next slots if free.
            break;
        }
    }

    void AddItemToInventory(GameObject go, int slot) //method to actually equip the item in the inventory array
    {
        inventory[slot] = go; //equip item at that slot
        
        Equipment equipment = go.GetComponent<Equipment>();

        equipment.Equip();
    }   

    void EnableAppropriateItem()
    {
        foreach (GameObject go in inventory)
            go.SetActive(false);

        inventory[index].SetActive(true);
    }

    public bool AnyFreeSlots()
    {
        for (int i = 0; i < inventory.Length; i++)
            if (inventory[i] == null)
                return true;

        return false;
    }

    public bool CheckIfSlotFree(int slot)
    {
        if (inventory[slot] == null)   
            return true;

        return false;
    }

    void NextItem()
    {
        if (index < inventory.Length - 1)
            index ++;

        else if (index == inventory.Length - 1)
            index = 0;

        EnableAppropriateItem();
    }

    void PreviousItem()
    {
        if (index < 0)
            index --;

        else if (index == 0)
            index = inventory.Length - 1;

        EnableAppropriateItem();
    }

}
