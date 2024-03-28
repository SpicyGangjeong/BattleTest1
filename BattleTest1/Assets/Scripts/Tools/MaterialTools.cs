
using System.Collections.Generic;
using UnityEngine;

public class MaterialTools
{

    // 대상 오브젝트 투명화 // 퍼온거임
    public static void SetTransparency(Transform obj, bool visible)
    {
        // Use DFS to recursivly set the transparency of given object and its childrens
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(obj);
        Transform current;
        while (queue.Count > 0)
        {
            current = queue.Dequeue();
            foreach (Renderer renderer in current.GetComponents<Renderer>())
            {
                foreach (Material material in renderer.materials)
                {
                    if (visible)
                    {
                        material.SetOverrideTag("RenderType", "");
                        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                        material.SetInt("_ZWrite", 1);
                        material.DisableKeyword("_ALPHATEST_ON");
                        material.DisableKeyword("_ALPHABLEND_ON");
                        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        material.renderQueue = -1;
                    }
                    else
                    {
                        material.SetOverrideTag("RenderType", "Transparent");
                        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        material.SetInt("_ZWrite", 0);
                        material.DisableKeyword("_ALPHATEST_ON");
                        material.EnableKeyword("_ALPHABLEND_ON");
                        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    }
                }
            }
            foreach (Transform child in current) queue.Enqueue(child);
        }
    }
}