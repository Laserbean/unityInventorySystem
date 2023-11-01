using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;
using System;
using Laserbean.General;

namespace unityInventorySystem.Inventories
{

    public enum InterfaceType
    {
        Inventory,
        Equipment,
        Chest
    }

    [CreateAssetMenu(fileName = "New Inventory", menuName = "unity Inventory System/Inventory")]
    public class InventoryObject : ScriptableObject
    {
        public string savePath;
        // public ItemDatabaseObject database;
        public InterfaceType type;
        public Inventory inventory;
        public InventorySlot[] GetSlots { get { return inventory.Slots; } }



        [ContextMenu("Save")]
        public void Save(string path = "")
        {
            //string saveData = JsonUtility.ToJson(this, true);
            //BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
            //bf.Serialize(file, saveData);
            //file.Close();

            IFormatter formatter = new BinaryFormatter();
            Stream stream;
            if (path == "") {
                stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
            }
            else {
                if (!path.EndsWith("/")) path += "/";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                stream = new FileStream(path + inventory.Name + ".fishfish", FileMode.Create, FileAccess.Write);
            }

            formatter.Serialize(stream, inventory);
            stream.Close();
        }


        [ContextMenu("Load")]
        public void Load(string path = "")
        {
            if (path != "") {
                if (!path.EndsWith("/")) path += "/";
            }

            // if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
            if (File.Exists(path + inventory.Name + ".fishfish")) {
                //BinaryFormatter bf = new BinaryFormatter();
                //FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
                //JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
                //file.Close();

                IFormatter formatter = new BinaryFormatter();
                Stream stream;

                if (path == "") {
                    stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
                }
                else {
                    stream = new FileStream(path + inventory.Name + ".fishfish", FileMode.Open, FileAccess.Read);
                }


                Inventory newContainer = (Inventory)formatter.Deserialize(stream);
                for (int i = 0; i < GetSlots.Length; i++) {
                    GetSlots[i].UpdateSlot(newContainer.Slots[i].item, newContainer.Slots[i].amount);
                }
                stream.Close();
            }
        }



        [ContextMenu("Clear")]
        public void Clear()
        {
            inventory.Clear();
        }
    }





}