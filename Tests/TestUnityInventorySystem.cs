using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using unityInventorySystem;
using unityInventorySystem.Items;
using unityInventorySystem.Inventories;

using Laserbean.General;
using unityInventorySystem.Items.Components;

public class TestUnityInventorySystem
{

    [Test]
    public void TestCreateInventory()
    {
        Inventory inventory = new(size: 10);

        Assert.IsNotNull(inventory.Slots[0]);
        Assert.IsNotNull(inventory.Slots[9]);

    }


    [Test]
    public void TestAddToEmptySlot()
    {
        // Use the Assert class to test conditions
        Inventory inventory = new(size: 10);
        var slot = inventory.AddToNewSlot(new Item() { Name = "Fish", Id = 27 }, 1);

        Assert.IsNotNull(slot);
        Assert.IsTrue(slot.item.Name == "Fish");
    }


    [Test]
    public void TestAddToExistingSlot()
    {
        Inventory inventory = new(size: 10);
        inventory.AddToNewSlot(new Item() { Name = "Fish", Id = 27 }, 1);

        var success = inventory.TryAddToExistingSlot(new Item() { Name = "Fish", Id = 27 }, 3);

        Assert.IsTrue(success, "Did not succesfuly add");

        var slot = inventory.GetItemSlot(new Item() { Name = "Fish", Id = 27 });
        Assert.IsNotNull(slot, "Slot is null");
        Assert.IsTrue(slot.amount == 4, "Amount doesn't match added");
    }


    [Test]
    public void TestAddTillOverflow()
    {
        Inventory inventory = new(size: 2);
        Assert.IsNotNull(inventory.AddToNewSlot(new Item() { Name = "Fish", Id = 27 }, 1));
        Assert.IsNotNull(inventory.AddToNewSlot(new Item() { Name = "Fish", Id = 27 }, 1));
        Assert.IsNull(inventory.AddToNewSlot(new Item() { Name = "Fish", Id = 27 }, 1));
    }

    [Test]
    public void TestAddEquipmentItem()
    {
        // This should create an inventory with one slot which only allows Chest Itemtypes. 
        Inventory inventory = new(size: 1);
        var allowed = new ItemType[1];
        allowed[0] = ItemType.Chest;
        inventory.Slots[0] = new() { AllowedItems = allowed };

        // Try to add a non chest item;
        bool non_chest_attempt = inventory.TryAddToExistingSlot(new Item() { Name = "Fish", Id = 27 }, 1);
        Assert.IsFalse(non_chest_attempt);
        Assert.IsTrue(false, "I simply have not finished writing this test");
    }

    // [Test]
    // public void TestTryAddItems()
    // {
    //     Inventory inventory = new(size: 1);
    //     Assert.IsTrue(inventory.TryAddItem(new Item() { Name = "Fish", Id = 27 }, 1));
    //     Assert.IsFalse(inventory.TryAddItem(new Item() { Name = "Chicken", Id = 23 }, 1));
    // }


    [Test]
    public void SavingItemComponents()
    {
        var item1 = new Item() {
            Name = "Fish",
            Id = 27
        };
        item1.Components.Add(new TestItemComponent(42)); 
        var  buffitem = new BuffItemComponent(); 

        buffitem.buffs.Add(new ItemBuff(0, 4){
            attribute = unityInventorySystem.Attribute.AttributeType.Agility
        });
        item1.Components.Add(buffitem); 

        SaveAnything.SaveThing(item1, "/unity_projects/Debug", "itemtest1", "item"); 
       var item2 =  SaveAnything.LoadThing<Item>("/unity_projects/Debug", "itemtest1", "item"); 

        Assert.IsTrue(item2.Components[0] is TestItemComponent);
        Assert.IsTrue((item2.Components[0] as TestItemComponent).Fish == 42);

        Assert.IsTrue(item2.Components[1] is BuffItemComponent);
        Assert.IsTrue((item2.Components[1] as BuffItemComponent).buffs[0].attribute == unityInventorySystem.Attribute.AttributeType.Agility);


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
