using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//Unity's JsonUtility cannot covert standalon lists to JSON strings. So this is a workaround for the issue.
//We convert each serializable object from the list into a JSON string and make a single list from all those string separating them with
//specail character '$' and save it in a text file. Now when it comes to get objects back from this JSON string,
//we split this string with '$' and convert those strings into separate objects and add them to a list and return this list.

public class JSONUtilityLibrary
{
    public static string ConvertObjectToJSONString(object ObjectToConvert)
    {
        return JsonUtility.ToJson(ObjectToConvert);
    }

    public static void SaveListASJSONString(List<object> ObjectListToConvert, string JSONFilePath)
    {
        string JSONString = "$";
        foreach (var Instance in ObjectListToConvert)
        {
            JSONString += ConvertObjectToJSONString(Instance);
            JSONString += "$";
        }

        File.WriteAllText(JSONFilePath, JSONString);
    }

    public static object ConvertJSONStringToObject(string JSONString, Type ObjectType)
    {
        object data = JsonUtility.FromJson(JSONString, ObjectType);
        var typedInstance = Convert.ChangeType(data, ObjectType);
        return typedInstance;
    }

    public static List<object> ConvertJSONStringToObjectsList(string JSONFilePath, Type ObjectType)
    {
        List<object> ObjectsList = new List<object>();

        if (!File.Exists(JSONFilePath))
        {
            Debug.LogError("This file doesn't exist : " + JSONFilePath);
            return ObjectsList;
        }            

        string JSONString = File.ReadAllText(JSONFilePath);
        string[] JSONStrings = JSONString.Split("$");

        for (int i = 1; i < JSONStrings.Length - 1; i++)
        {
            var typedInstance = JSONUtilityLibrary.ConvertJSONStringToObject(JSONStrings[i], ObjectType);
            ObjectsList.Add(typedInstance);
        }

        return ObjectsList;
    }
}
