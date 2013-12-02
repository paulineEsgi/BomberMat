using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

class SelectSame : EditorWindow
{
    [MenuItem("Custom/SelectSameTag #t")]
    static void selectSameTag()
    {
        Selection.objects = GameObject.FindGameObjectsWithTag( Selection.activeGameObject.tag);
    }

    [MenuItem("Custom/SelectSameName #n")]
    static void selectSameName()
    {
        List<GameObject> sceneObjects;

        sceneObjects = new List<GameObject>();
        foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if(obj.name == Selection.activeGameObject.name)
                sceneObjects.Add(obj);
        }

        Selection.objects = sceneObjects.ToArray();
    }


}
