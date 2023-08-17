//using UnityEngine;
//using UnityEditor;
//using Newtonsoft.Json;
//using System.Collections.Generic;

//public class SceneGraphExporter : EditorWindow
//{
//    [MenuItem("Window/Export Scene Graph")]
//    private static void ExportSceneGraph()
//    {
//        // Get the current scene
//        UnityEngine.SceneManagement.Scene scene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();

//        // Create a dictionary to store the scene graph
//        Dictionary<string, object> sceneGraph = new Dictionary<string, object>();
//        sceneGraph["name"] = scene.name;
//        sceneGraph["type"] = "Scene";
//        sceneGraph["children"] = GetChildren(scene.GetRootGameObjects());

//        // Convert the scene graph to JSON
//        string json = JsonConvert.SerializeObject(sceneGraph, Formatting.Indented);

//        // Save the JSON to a file
//        string outputPath = EditorUtility.SaveFilePanel("Export Scene Graph", "", "scene_graph.json", "json");
//        if (!string.IsNullOrEmpty(outputPath))
//        {
//            System.IO.File.WriteAllText(outputPath, json);
//            Debug.Log("Scene graph exported to: " + outputPath);
//        }
//    }

//    private static List<object> GetChildren(GameObject[] gameObjects)
//    {
//        List<object> children = new List<object>();

//        foreach (GameObject gameObject in gameObjects)
//        {
//            Dictionary<string, object> child = new Dictionary<string, object>();
//            child["name"] = gameObject.name;
//            child["type"] = "GameObject";
//            child["components"] = GetComponentData(gameObject);

//            // Get children recursively
//            Transform transform = gameObject.transform;
//            if (transform.childCount > 0)
//                child["children"] = GetChildren(GetChildGameObjects(transform));

//            children.Add(child);
//        }

//        return children;
//    }

//    private static List<object> GetComponentData(GameObject gameObject)
//    {
//        List<object> componentData = new List<object>();

//        Component[] components = gameObject.GetComponents<Component>();
//        foreach (Component component in components)
//        {
//            Dictionary<string, object> data = new Dictionary<string, object>();
//            data["type"] = component.GetType().ToString();

//            if (component is Transform)
//            {
//                Transform transform = (Transform)component;
//                data["position"] = GetVector3Data(transform.position);
//                data["rotation"] = GetVector3Data(transform.rotation.eulerAngles);
//                data["scale"] = GetVector3Data(transform.localScale);
//            }
//            else
//            {
//                // Get serialized data
//                SerializedObject serializedObject = new SerializedObject(component);
//                SerializedProperty serializedProperty = serializedObject.GetIterator();
//                while (serializedProperty.NextVisible(true))
//                {
//                    if (serializedProperty.name == "m_Script")
//                        continue;

//                    data[serializedProperty.name] = GetValue(serializedProperty);
//                }
//            }

//            componentData.Add(data);
//        }

//        return componentData;
//    }

//    private static GameObject[] GetChildGameObjects(Transform transform)
//    {
//        GameObject[] childGameObjects = new GameObject[transform.childCount];

//        for (int i = 0; i < transform.childCount; i++)
//        {
//            childGameObjects[i] = transform.GetChild(i).gameObject;
//        }

//        return childGameObjects;
//    }

//    private static object GetValue(SerializedProperty property)
//    {
//        switch (property.propertyType)
//        {
//            case SerializedPropertyType.Integer:
//                return property.intValue;
//            case SerializedPropertyType.Boolean:
//                return property.boolValue;
//            case SerializedPropertyType.Float:
//                return property.floatValue;
//            case SerializedPropertyType.String:
//                return property.stringValue;
//            case SerializedPropertyType.ObjectReference:
//                return property.objectReferenceValue != null ? property.objectReferenceValue.name : "null";
//            case SerializedPropertyType.Enum:
//                return property.enumNames[property.enumValueIndex];
//            default:
//                return null;
//        }
//    }
    
//    private static Dictionary<string, float> GetVector3Data(Vector3 vector)
//    {
//        Dictionary<string, float> data = new Dictionary<string, float>();
//        data["x"] = vector.x;
//        data["y"] = vector.y;
//        data["z"] = vector.z;
//        return data;
//    }
//}
