using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblePart : MonoBehaviour
{
    [SerializeField] private Transform part;

    public void ChangeMaterial(Material mat)
    {
        for (int i = 1; i < part.childCount; i++)
        {
            GameObject cell = part.GetChild(i).gameObject;

            cell.GetComponent<MeshRenderer>().material = mat;
        }
    }

    public void DestroyPart()
    {
        for (int i = 0; i < part.childCount; i++)
        {
            if (i == 0)
            {
                part.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                GameObject cell = part.GetChild(i).gameObject;

                cell.SetActive(true);
                Destroy(cell, 2);
            }
        }
    }
}
