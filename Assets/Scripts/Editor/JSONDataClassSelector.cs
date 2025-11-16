using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

//This window will find all the classes with JSONConvertable attribute and will show as options to the user
//as for which class do they want to make JSON data file of.
public class JSONDataClassSelector : EditorWindow
{
    private List<Type> MarkedTypes = new List<Type>();
    private Vector2 Scroll;

    [MenuItem("Window/JSON Maker Widnow")]
    public static void ShowWindow()
    {
        GetWindow<JSONDataClassSelector>("JSON Maker Widnow");
    }

    private void OnEnable()
    {
        RefreshClassList();
    }

    private void RefreshClassList()
    {
        //Get all types in the whole code base
        var allTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes());

        //Find classes which have JSONConvertable attribute and are serializable
        MarkedTypes = allTypes
            .Where(t =>
                t.IsClass && t.IsSerializable &&
                t.GetCustomAttribute<JSONConvertableAttribute>() != null)
            .OrderBy(t => t.Name)
            .ToList();
    }

    private void OnGUI()
    {
        GUILayout.Label("Select Class", EditorStyles.boldLabel);
        GUILayout.Space(4);

        Scroll = GUILayout.BeginScrollView(Scroll);

        //Add buttons for each class type
        foreach (var type in MarkedTypes)
        {
            if (GUILayout.Button(type.Name))
            {
                //Pass the class type to the JSON maker window and open that editor window
                JSONMakerWindow.Show(type);
                Close();
            }
        }

        GUILayout.EndScrollView();
    }
}
