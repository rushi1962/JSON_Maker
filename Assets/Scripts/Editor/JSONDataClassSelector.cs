using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class JSONDataClassSelector : EditorWindow
{
    private List<Type> markedTypes = new List<Type>();
    private Vector2 scroll;

    [MenuItem("Window/JSON Maker Widnow")]

    public static void ShowWindow()
    {
        GetWindow<JSONDataClassSelector>("Select Class");
    }

    private void OnEnable()
    {
        RefreshClassList();
    }

    private void RefreshClassList()
    {
        var allTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes());

        markedTypes = allTypes
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

        scroll = GUILayout.BeginScrollView(scroll);

        foreach (var type in markedTypes)
        {
            if (GUILayout.Button(type.Name))
            {
                JSONMakerWindow.Show(type);
                Close();
            }
        }

        GUILayout.EndScrollView();
    }
}
