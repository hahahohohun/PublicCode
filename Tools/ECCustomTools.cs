using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ECCustomTools : MonoBehaviour
{
    [MenuItem("GameObject/IsolateActiveFalse %#q")]
    public static void IsolateActiveFalse()
    {
        var list = Selection.GetTransforms(SelectionMode.TopLevel);

        foreach (var item in list)
        {
            Transform traParent = item.parent;
            if (traParent != null)
            {
                for (int i = 0; i < traParent.childCount; i++)
                {
                    if (item == traParent.GetChild(i))
                        continue;

                    Undo.RegisterCompleteObjectUndo(traParent.GetChild(i).gameObject, "GameObject/IsolateActiveFalse");
                    traParent.GetChild(i).gameObject.SetActive(false);
                }
            }
            else
            {
                GameObject[] all_Objs = SceneManager.GetActiveScene().GetRootGameObjects();
                foreach (GameObject g in all_Objs)
                {
                    bool bSame = false;
                    foreach (var cur in list)
                    {
                        if (cur == g.transform)
                        {
                            bSame = true;
                            break;
                        }
                    }
                    if (!bSame)
                    {
                        Undo.RegisterCompleteObjectUndo(g, "GameObject/IsolateActiveFalse");
                        g.SetActive(false);
                    }
                }
            }
        }
    }
}
