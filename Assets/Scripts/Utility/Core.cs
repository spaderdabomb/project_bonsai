using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public static class Core
{

    public static GameObject FindGameObjectByNameAndTag(string name, string tag)
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag(tag);
        GameObject returnObject = null;
        foreach (GameObject taggedOne in go)
        {
            if (taggedOne.name == name)
            {
                returnObject = taggedOne;
            }
        }

        return returnObject;
    }

    public static GameObject FindHiddenGameObjectByNameAndTag(string name, string tag)
    {
        List<GameObject> go = GetAllObjectsInScene();
        GameObject returnObject = null;
        foreach (GameObject taggedOne in go)
        {
            if (taggedOne.name == name)
            {
                returnObject = taggedOne;
            }
        }

        return returnObject;
    }

    public static List<GameObject> GetAllObjectsInScene()
    {
        List<GameObject> objectsInScene = new List<GameObject>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (go.hideFlags != HideFlags.None)
                continue;
            if (PrefabUtility.GetPrefabAssetType(go) == PrefabAssetType.Regular || PrefabUtility.GetPrefabAssetType(go) == PrefabAssetType.Regular)
                continue;
            objectsInScene.Add(go);
        }
        return objectsInScene;
    }

    public static GameObject FindChildGameObjectWithTag(GameObject parent, string tag)
    {
        Transform t = parent.transform;
        for (int i = 0; i < t.childCount; i++)
        {
            if (t.GetChild(i).gameObject.CompareTag(tag))
            {
                return t.GetChild(i).gameObject;
            }
        }
        return null;
    }

    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.CompareTag(tag))
            {
                return tr.GetComponent<T>();
            }
        }
        return null;
    }

    public static T[] FindComponentsInChildrenWithTag<T>(this GameObject parent, string tag, bool forceActive = false) where T : Component
    {
        if (parent == null) { throw new System.ArgumentNullException(); }
        if (string.IsNullOrEmpty(tag) == true) { throw new System.ArgumentNullException(); }
        List<T> list = new List<T>(parent.GetComponentsInChildren<T>(forceActive));
        if (list.Count == 0) { return null; }

        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].CompareTag(tag) == false)
            {
                list.RemoveAt(i);
            }
        }
        return list.ToArray();
    }

    public static bool ContainsAny(this string haystack, params string[] needles)
    {
        foreach (string needle in needles)
        {
            if (haystack.Contains(needle))
                return true;
        }

        return false;
    }

    public static bool ContainsAll(string[] haystacks, string[] needles)
    {
        bool foundAll = new bool();
        foundAll = false;

        foreach (string needle in needles)
        {
            foundAll = false;
            foreach (string haystack in haystacks)
            {
                MonoBehaviour.print(haystack);
                if (haystack == needle)
                {
                    foundAll = true;
                }
            }
            if (!foundAll)
            {
                foundAll = false;
            }
        }

        return foundAll;
    }

    public static GameObject GetFirstParentWithTag(GameObject gameObject, string parentTag) 
    {
        int debugIndex = 0;
        GameObject currentGO = gameObject;
        while (currentGO.tag != parentTag)
        {
            Console.WriteLine(currentGO.tag);
            currentGO = currentGO.transform.parent.gameObject;

            debugIndex += 1;
            if (debugIndex > 50)
            {
                Debug.LogWarning("Could not find parent with tag " + parentTag + " after 50 loops");
                return null;
            }
        }

        return currentGO;
    }

    public static bool Contains(Array a, object val)
    {
        return Array.IndexOf(a, val) != -1;
    }

    public static int GetIndex(Array a, object val)
    {
        return Array.IndexOf(a, val);
    }

    public static void DestroyAllChildren(GameObject gameObject)
    {
        foreach (Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public static Color ConvertHexColorToRGBA(string hexColor)
    {
        Color newCol;
        if (ColorUtility.TryParseHtmlString(hexColor, out newCol))
        {
            return newCol;
        }

        Debug.LogError("Failed to convert hex string to RGBA > Core.ConvertHexColorToRGBA");
        return new Color(0, 0, 0, 255);
    }

    public static void SetCanvasGroupState(CanvasGroup canvasGroup, bool state)
    {
        if (state)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}