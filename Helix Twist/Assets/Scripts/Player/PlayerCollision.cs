using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision: MonoBehaviour
{
    private Rigidbody rb;
    private PlayerScore playerScore;
    private Transform partsParent;

    [SerializeField] private float bounceForce = 100;
    private Vector3 startPos;
    private bool destroying = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
        playerScore = GetComponent<PlayerScore>();
        partsParent = FindObjectOfType<LevelMaker>().transform;
    }

    public void CheckDestroyParts(Collider collider)
    {
        if (playerScore.ComboCounter > 1)
        {
            destroying = true;

            int partsToDestroy = playerScore.ComboCounter - 1;

            if (collider.transform.parent == null)
            {
                return;
            }

            GameObject part = collider.transform.parent.parent.gameObject;
            part.SetActive(false);
            int siblingIndex = part.transform.GetSiblingIndex();

            if (partsToDestroy == 2)
            {
                if (CheckInRange(siblingIndex + 1))
                {
                    partsParent.GetChild(siblingIndex + 1).gameObject.SetActive(false);
                }
                else if (CheckInRange(siblingIndex - 1))
                {
                    partsParent.GetChild(siblingIndex - 1).gameObject.SetActive(false);
                }
            }
            else if (partsToDestroy > 2)
            {
                for (int i = 0; i < partsToDestroy; i++)
                {
                    int index = 0 - (partsToDestroy - 2);
                    index += i;

                    if (CheckInRange(index))
                    {
                        partsParent.GetChild(index).gameObject.SetActive(false);
                    }
                }
            }
            else if (partsToDestroy >= 5)
            {
                part.transform.parent.gameObject.SetActive(false);
            }

            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * bounceForce);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 0)
        {
            if (!destroying && playerScore.ComboCounter < 2 && collision.gameObject.CompareTag("DeathPart"))
            {
                playerScore.ResetScore();
            }

            destroying = false;
            //rb.AddForce(collision.contacts[0].normal * bounceForce);
        }
    }

    private bool CheckInRange(int index)
    {
        if (index > 0 && index <= partsParent.childCount - 1)
        {
            if (partsParent.GetChild(index).CompareTag("Point"))
            {
                return false;
            }

            return true;
        }
        else
        {
            return false;
        }
    }
}
