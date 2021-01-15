using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBall : MonoBehaviour
{
    private Vector3 startPos;
    private GameObject trail;
    private Rigidbody rb;
    [SerializeField] private float teleportY = -20;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        trail = GetComponentInChildren<TrailRenderer>().gameObject;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < teleportY)
        {
            trail.SetActive(false);
            transform.position = startPos;
            rb.velocity = Vector3.zero;
            Invoke("TurnOnTrailRenderer", 1);
        }
    }

    private void TurnOnTrailRenderer()
    {
        trail.SetActive(true);
    }
}
