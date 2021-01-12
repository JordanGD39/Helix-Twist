using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMaker : MonoBehaviour
{
    [SerializeField] private int partsToRemovePerSpiral = 3;
    [SerializeField] private int deadlyPartsPerSpiral = 1;
    [SerializeField] private Material deathMat;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform spiral = transform.GetChild(i);

            List<GameObject> availableParts = new List<GameObject>();

            for (int j = 0; j < spiral.childCount - 2; j++)
            {
                availableParts.Add(spiral.GetChild(j).gameObject);
            }

            for (int j = 0; j < partsToRemovePerSpiral; j++)
            {
                int rand = Random.Range(0, availableParts.Count);

                GameObject randPart = availableParts[rand];
                randPart.SetActive(false);
                availableParts.Remove(randPart);
            }

            for (int j = 0; j < deadlyPartsPerSpiral; j++)
            {
                int rand = Random.Range(0, availableParts.Count);

                Transform randPart = availableParts[rand].transform;
                
                ChangeMaterialToDeath(randPart.GetChild(0).GetComponentInChildren<MeshRenderer>());
                ChangeMaterialToDeath(randPart.GetChild(1).GetComponentInChildren<MeshRenderer>());
            }
        }
    }

    private void ChangeMaterialToDeath(MeshRenderer renderer)
    {
        renderer.material = deathMat;
        renderer.tag = "DeathPart";
    }
}
