using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class JSONMakerWindow : EditorWindow
{
    private Dictionary<object, bool> Foldouts = new Dictionary<object, bool>();
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
        //Give a text field for JSON file path
        JSONFilePath = EditorGUILayout.TextField("JSON file path", JSONFilePath);
        GUILayout.Space(5);

        if (ClassInstances.Count > 0)
        {
            int InstanceCount = 0;
            foreach(var item in ClassInstances)
            {
                //Foldouts for class instances
                if (!Foldouts.ContainsKey(item))
                    Foldouts[item] = true;

                Foldouts[item] = EditorGUILayout.Foldout(Foldouts[item], "Instance_"+InstanceCount.ToString());

                if (Foldouts[item])
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

        //Add required buttons according to the number of class instances and if user wants to create a new file or load an existing one.
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
            JSONUtilityLibrary.SaveListASJSONString(ClassInstances, JSONFilePath);
            AssetDatabase.Refresh();
            Close();
        }
    }

    //Populates instances list with JSON string from existing JSON file.
    private void PopulateClassInstances()
    {
       ClassInstances.Clear();
       JSONUtilityLibrary.ConvertJSONStringToObjectsList(JSONFilePath, JSONDataClassType, ClassInstances);
    }
}
