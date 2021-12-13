using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ECCustomTools : MonoBehaviour
{
    public static bool _bActive = true;
    public static List<GameObject> _undolist = new List<GameObject>();
    private void Awake()
    {
        _bActive = true;
        _undolist.Clear();
    }

    [MenuItem("GameObject/IsolateActive %#q")]
    public static void IsolateActive()
    {
        _bActive = !_bActive;
        var list = Selection.GetTransforms(SelectionMode.TopLevel);
        if (!_bActive)
        {
            foreach (var item in list)
            {
                Transform traParent = item.parent;
                if (traParent != null)
                {
                    for (int i = 0; i < traParent.childCount; i++)
                    {
                        SetActive(traParent.GetChild(i).gameObject, list);
                    }
                }
                else
                {
                    GameObject[] all_Objs = SceneManager.GetActiveScene().GetRootGameObjects();
                    foreach (GameObject g in all_Objs)
                    {
                        SetActive(g, list);
                    }
                }
            }
        }
        else
        {
            foreach (var item in _undolist)
            {
                item.SetActive(true);
            }
            _undolist.Clear();
        }
    }

    private static void SetUndo(GameObject obj)
    {
        Undo.RegisterCompleteObjectUndo(obj, "GameObject/IsolateActive");
        _undolist.Add(obj);
        obj.SetActive(false);
    }
    private static void SetActive(GameObject obj, Transform[] list)
    {
        bool bSame = false;
        foreach (var cur in list)
        {
            if (cur == obj.transform)
            {
                bSame = true;
                break;
            }
        }
        if (!bSame && obj.activeSelf)
        {
            SetUndo(obj);
        }
    }
}
