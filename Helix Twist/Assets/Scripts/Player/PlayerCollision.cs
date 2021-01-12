using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision: MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float bounceForce = 100;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 0)
        {
            if (collision.gameObject.CompareTag("DeathPart"))
            {
                transform.position = startPos;
            }

            //rb.AddForce(collision.contacts[0].normal * bounceForce);
        }
    }
}
