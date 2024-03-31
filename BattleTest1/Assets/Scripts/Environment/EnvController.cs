using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvController : MonoBehaviour
{
    public float lineSize = 16f;
    private void Update()
    {
        Debug.DrawRay(transform.position, -transform.up * lineSize, Color.yellow);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, -transform.up, lineSize);

        for (int i = 0; i< hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            if (hit.collider.gameObject.name.Contains("Tile"))
            {
                hit.collider.transform.GetComponent<Tile>().tileState = Types.TileState.Block;
            }
        }
    }
}
