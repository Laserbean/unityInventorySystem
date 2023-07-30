using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

using System.Linq;

using System.IO;

#if UNITY_EDITOR
using UnityEditorInternal;
#endif

using Laserbean.General; 

namespace unityInventorySystem {



[System.Serializable]
public class RestrictedList 
{
    public List<string> list = new List<string>();

    // [System.NonSerialized]
    [HideInInspector]
    public string ID = ""; 
}

#if UNITY_EDITOR


[CustomPropertyDrawer(typeof(RestrictedList))]
public class CustomListDrawer : PropertyDrawer
{
    private ReorderableList list;
    private Dictionary<string, ReorderableList> listDict = new Dictionary<string, ReorderableList>(); 

    private ConfigObject configObject; 

    bool hasInit = false; 

    private int ExtractIndexFromPropertyPath(string propertyPath)
    {
        // The property path has the format: myArray.Array.data[Index]
        int startIndex = propertyPath.LastIndexOf('[') + 1;
        int endIndex = propertyPath.LastIndexOf(']');

        if (endIndex == -1) { return -1;}
        string indexString = propertyPath.Substring(startIndex, endIndex - startIndex);
        int index;
        if (int.TryParse(indexString, out index))
        {
            return index;
        }

        // Return a default value (e.g., -1) if the index cannot be parsed
        return -1;
    }

    private void Init(SerializedProperty property)
    {
        // if (hasInit) return; 
        // string debugstring = hasInit ? "yes".DebugColor(Color.green) : "no".DebugColor(Color.red); 
        // Debug.Log(debugstring);

        SerializedProperty curID = property.FindPropertyRelative("ID");
        int index = ExtractIndexFromPropertyPath(property.propertyPath);

        if (!curID.stringValue.EndsWith("" + index)) {
            listDict.Remove(curID.stringValue);
            curID.stringValue = ""; 
        }

        if (string.IsNullOrEmpty(curID.stringValue)) {
            curID.stringValue = RandomStatic.GenerateRandomString(20) + index.ToString(); 
            Debug.Log(curID.stringValue.DebugColor(Color.green)); 

        }


        if (!listDict.TryGetValue(curID.stringValue, out ReorderableList list)) {
            list = new ReorderableList(property.serializedObject, property.FindPropertyRelative("list"), true, true, true, true);
            list.drawHeaderCallback += rect => EditorGUI.LabelField(rect, "Tags: " + curID.stringValue);
            list.onAddDropdownCallback += OnAddDropdown;
            list.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var elementProperty = list.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.LabelField(rect, elementProperty.stringValue);
            };

            listDict.Add(curID.stringValue, list);
        }


        hasInit = true; 

    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        EditorGUI.BeginProperty(position, label, property);
        Init(property);

        position.height = EditorGUIUtility.singleLineHeight;
        EditorGUI.LabelField(position, label);
        position.y += EditorGUIUtility.singleLineHeight;


        SerializedProperty curID = property.FindPropertyRelative("ID");

        // EditorGUI.LabelField(position, curID.stringValue);
        // position.y += EditorGUIUtility.singleLineHeight;

        
        if (string.IsNullOrEmpty(curID.stringValue)) {
            curID.stringValue = RandomStatic.GenerateRandomString(20); 
        }

        listDict[curID.stringValue].DoList(position);

        EditorGUI.EndProperty();
    }


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        Init(property);

        SerializedProperty curID = property.FindPropertyRelative("ID");
        if (string.IsNullOrEmpty(curID.stringValue)) {
            // curID.stringValue = RandomStatic.GenerateRandomString(20); 
            Debug.LogError("this shouldn't be happening"); 
            return 50; 
        }
        if (property == null || property.serializedObject == null ||  property.serializedObject.targetObject == null) return 30;
        return listDict[curID.stringValue].GetHeight() + EditorGUIUtility.singleLineHeight;
    }


    private void OnAddClickHandler(object tag, ReorderableList list) // Receive the ReorderableList instance as an argument
    {
        list.serializedProperty.arraySize++;
        var newIndex = list.serializedProperty.arraySize - 1;
        var element = list.serializedProperty.GetArrayElementAtIndex(newIndex);
        element.stringValue = (string)tag;
        list.index = newIndex;
        list.serializedProperty.serializedObject.ApplyModifiedProperties();
    }


    private static ConfigObject fakeConfig; 
    private void OnAddDropdown(Rect buttonRect, ReorderableList list)
    {
        GenericMenu menu = new GenericMenu();
        string[] tags = { "Head", "Body", "Hand", "Feet" };
        configObject = ConfigStatic.GetConfigObject(); 

        tags = configObject.tags.ToArray(); 

        
        for (int i = 0; i < tags.Length; i++)
        {
            var label = new GUIContent(tags[i]);
            if (!PropertyContainsString(list.serializedProperty, tags[i]))
            {
                // menu.AddItem(label, false, OnAddClickHandler, tags[i]);
                // menu.AddItem(label, false, () => OnAddClickHandler(tags[i], list));
                menu.AddItem(label, false, (object data) => OnAddClickHandler(data, list), tags[i]);


            }
            else
            {
                menu.AddDisabledItem(label);
            }
        }

        menu.ShowAsContext();
    }

    private bool PropertyContainsString(SerializedProperty property, string value)
    {
        if (property.isArray)
        {
            for (int i = 0; i < property.arraySize; i++)
            {
                if (property.GetArrayElementAtIndex(i).stringValue == value)
                    return true;
            }
        }
        else
        {
            return property.stringValue == value;
        }

        return false;
    }
}

#endif

}
