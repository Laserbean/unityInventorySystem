using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using unityInventorySystem;

public class TestUnityInventorySystem
{

    [Test]
    public void TestCreateInventory() {
        Inventory inventory = new (size:10); 

        Assert.IsNotNull(inventory.Slots[0]);
        Assert.IsNotNull(inventory.Slots[9]);

    }


    [Test]
    public void TestAddToEmptySlot()
    {
        // Use the Assert class to test conditions
        Inventory inventory = new (size:10); 
        var slot = inventory.AddToNewSlot(new Item(){Name = "Fish", Id = 27}, 1);

        Assert.IsNotNull(slot);
    }


    [Test]
    public void TestAddToExistingSlot() {
        Inventory inventory = new (size:10); 
        inventory.AddToNewSlot(new Item(){Name = "Fish", Id = 27}, 1);

        var success = inventory.TryAddToExistingSlot(new Item() {Name = "Fish",Id = 27}, 3);

        Assert.IsTrue(success, "Did not succesfuly add"); 

        var slot = inventory.GetItemSlot(new Item() {Name = "Fish",Id = 27});
        Assert.IsNotNull(slot, "Slot is null");       
        Assert.IsTrue(slot.amount == 4, "Amount doesn't match added");          
    }


    [Test]
    public void TestAddTillOverflow() {
        Inventory inventory = new (size:2); 
        Assert.IsNotNull(inventory.AddToNewSlot(new Item(){Name = "Fish", Id = 27}, 1));
        Assert.IsNotNull(inventory.AddToNewSlot(new Item(){Name = "Fish", Id = 27}, 1));
        Assert.IsNull(inventory.AddToNewSlot(new Item(){Name = "Fish", Id = 27}, 1));
    }

    [Test]
    public void TestAddEquipmentItem() {
        // This should create an inventory with one slot which only allows Chest Itemtypes. 
        Inventory inventory = new (size:1); 
        var allowed = new ItemType[1]; 
        allowed[0] = ItemType.Chest;
        inventory.Slots[0] = new () {AllowedItems = allowed};

        // Try to add a non chest item;
        bool non_chest_attempt =  inventory.TryAddToExistingSlot(new Item(){Name = "Fish", Id = 27}, 1);
        Assert.IsFalse(non_chest_attempt); 
        Assert.IsTrue(false, "I simply have not finished writing this test");
    }

    [Test]
    public void TestTryAddItems() {
        Inventory inventory = new (size:1); 
        Assert.IsTrue(inventory.TryAddItem(new Item(){Name = "Fish", Id = 27}, 1));
        Assert.IsFalse(inventory.TryAddItem(new Item(){Name = "Chicken", Id = 23}, 1));



    }

    // // // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // // // `yield return null;` to skip a frame.
    // // [UnityTest]
    // // public IEnumerator TestUnityInventorySystemWithEnumeratorPasses()
    // // {
    // //     // Use the Assert class to test conditions.
    // //     // Use yield to skip a frame.
    // //     yield return null;
    // // }
}
