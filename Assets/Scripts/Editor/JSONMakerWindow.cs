using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class JSONMakerWindow : EditorWindow
{
    private Dictionary<object, bool> foldouts = new Dictionary<object, bool>();
    private List<object> ClassInstances;
    private Type JSONDataClassType;
    private string JSONFilePath = "Assets/Data/JSONFiles/JSONData.txt";

    public static void Show(Type ClassType)
    {
        JSONMakerWindow Window = CreateInstance<JSONMakerWindow>();
        Window.JSONDataClassType = ClassType;
        Window.titleContent = new GUIContent(ClassType.Name + " JSON Maker");
        Window.ShowUtility();
    }

    private void OnEnable()
    {
        ClassInstances = new List<object>();
    }

    private void OnGUI()
    {
        JSONFilePath = EditorGUILayout.TextField("JSON file path", JSONFilePath);
        GUILayout.Space(5);

        if (ClassInstances.Count > 0)
        {
            int InstanceCount = 0;
            foreach(var item in ClassInstances)
            {
                if (!foldouts.ContainsKey(item))
                    foldouts[item] = true;

                foldouts[item] = EditorGUILayout.Foldout(foldouts[item], "Instance_"+InstanceCount.ToString());

                if (foldouts[item])
                {
                    EditorGUI.indentLevel++;

                    var fields = JSONDataClassType.GetFields(
                            BindingFlags.Public | BindingFlags.Instance);

                    foreach (var field in fields)
                    {
                        object value = field.GetValue(item);

                        // DRAW UI BASED ON FIELD TYPE
                        if (field.FieldType == typeof(int))
                            value = EditorGUILayout.IntField(field.Name, (int)value);

                        else if (field.FieldType == typeof(float))
                            value = EditorGUILayout.FloatField(field.Name, (float)value);

                        else if (field.FieldType == typeof(string))
                            value = EditorGUILayout.TextField(field.Name, (string)value);

                        else if (field.FieldType == typeof(bool))
                            value = EditorGUILayout.Toggle(field.Name, (bool)value);

                        else if (field.FieldType.IsEnum)
                            value = EditorGUILayout.EnumPopup(field.Name, (Enum)value);

                        // assign updated value
                        field.SetValue(item, value);
                    }
                    EditorGUI.indentLevel--;
                }

                InstanceCount++;
            }
        }

        GUILayout.BeginHorizontal();

        if(ClassInstances.Count <= 0)
        {
            if (GUILayout.Button("Create New File"))
            {
                ClassInstances.Add(Activator.CreateInstance(JSONDataClassType));
            }

            if (GUILayout.Button("Load Existing File"))
            {
                PopulateClassInstances();
            }
        }

        if (ClassInstances.Count > 0)
        {
            if(GUILayout.Button("Remove Instance"))
            {
                ClassInstances.RemoveAt(ClassInstances.Count - 1);
            }            

            if (GUILayout.Button("Add Instance"))
            {
                ClassInstances.Add(Activator.CreateInstance(JSONDataClassType));
            }
        }

                

        GUILayout.EndHorizontal();

        if (ClassInstances.Count > 0 && GUILayout.Button("Save JSON File"))
        {
            string JSONString = "$";

            foreach(var Instance in ClassInstances)
            {
                JSONString += JsonUtility.ToJson(Instance);
                JSONString += "$";
            }

            File.WriteAllText(JSONFilePath, JSONString);
            AssetDatabase.Refresh();
            Close();
        }
    }

    private void PopulateClassInstances()
    {
        if (!File.Exists(JSONFilePath))
            Debug.LogError("This file doesn't exist : " + JSONFilePath);

        string JSONString = File.ReadAllText(JSONFilePath);
        string[] JSONStrings = JSONString.Split("$");

        for (int i = 1; i < JSONStrings.Length - 1; i++)
        {
            object data = JsonUtility.FromJson(JSONStrings[i], JSONDataClassType);
            var typedInstance = Convert.ChangeType(data, JSONDataClassType);
            ClassInstances.Add(typedInstance);
        }
    }
}
