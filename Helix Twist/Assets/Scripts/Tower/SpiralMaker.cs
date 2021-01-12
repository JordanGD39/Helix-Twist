using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralMaker : MonoBehaviour
{
    [SerializeField] private Transform tower;
    [SerializeField] private GameObject partPrefab;
    [SerializeField] private int objectsToSpawn = 25;
    [SerializeField] private float yOffset = 0.5f;
    [SerializeField] private float offset = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        CreateSpiral();
    }

    private void CreateSpiral()
    {
        Vector3 targetPos = tower.position;

        for (int i = 0; i < objectsToSpawn; i++)
        {
            GameObject part = Instantiate(partPrefab, tower, false);

            float angle = i * (2 * Mathf.PI / 10);

            float x = Mathf.Cos(angle) * offset;
            float z = Mathf.Sin(angle) * offset;

            targetPos.x += x;
            targetPos.y -= yOffset;
            targetPos.z += z;

            part.transform.position = targetPos;
        }
    }
}
