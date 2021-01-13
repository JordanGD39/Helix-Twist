using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 extraPos;
    [SerializeField] private Transform target;
    private Vector3 towerPos;

    // Start is called before the first frame update
    void Start()
    {
        towerPos = GameObject.FindGameObjectWithTag("Tower").transform.position;

        if (GameManager.instance.BgColor.a != 0)
        {
            GetComponent<Camera>().backgroundColor = GameManager.instance.BgColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        towerPos.y = target.position.y;
        Vector3 diff = target.position - towerPos;

        //Debug.DrawRay(target.position, diff, Color.green);

        Vector3 targetPos = target.position;
        targetPos += diff * extraPos.z;

        Vector3 adjustedPos = extraPos;
        adjustedPos.z = 0;

        transform.position = targetPos + adjustedPos;
        transform.LookAt(target.position);
    }
}
