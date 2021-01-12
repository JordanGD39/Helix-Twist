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

            List<Transform> availableParts = new List<Transform>();

            for (int j = 0; j < spiral.childCount - 2; j++)
            {
                availableParts.Add(spiral.GetChild(j));
            }

            for (int j = 0; j < partsToRemovePerSpiral; j++)
            {
                int rand = Random.Range(0, availableParts.Count);

                Transform randPart = availableParts[rand];
                ChangePartToPoint(randPart.GetChild(0).GetComponentInChildren<MeshCollider>());
                ChangePartToPoint(randPart.GetChild(1).GetComponentInChildren<MeshCollider>());
                randPart.tag = "Point";
                availableParts.Remove(randPart);
            }

            for (int j = 0; j < deadlyPartsPerSpiral; j++)
            {
                int rand = Random.Range(0, availableParts.Count);

                Transform randPart = availableParts[rand];
                
                ChangeMaterialToDeath(randPart.GetChild(0).GetComponentInChildren<MeshRenderer>());
                ChangeMaterialToDeath(randPart.GetChild(1).GetComponentInChildren<MeshRenderer>());
            }
        }

        FindObjectOfType<PlayerScore>().AddPoints();
    }

    private void ChangePartToPoint(MeshCollider collider)
    {
        collider.isTrigger = true;
        collider.GetComponent<MeshRenderer>().enabled = false;
    }

    private void ChangeMaterialToDeath(MeshRenderer renderer)
    {
        renderer.material = deathMat;
        renderer.tag = "DeathPart";
    }
}
