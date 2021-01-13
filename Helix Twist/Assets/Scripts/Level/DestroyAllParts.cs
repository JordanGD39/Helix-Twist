using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllParts : MonoBehaviour
{
    private DestructiblePart[] parts;

    // Start is called before the first frame update
    void Start()
    {
        parts = GetComponentsInChildren<DestructiblePart>();
    }

    public void DestroyAllPartsInSpiral()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].DestroyPart();
        }
    }
}
