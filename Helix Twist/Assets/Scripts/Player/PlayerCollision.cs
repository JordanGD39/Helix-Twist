using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision: MonoBehaviour
{
    private Rigidbody rb;
    private PlayerScore playerScore;
    private GroundCheck groundCheck;
    private Transform partsParent;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float bounceForce = 100;
    //[SerializeField] private float angleToSurviveDeath = 15;
    private Vector3 startPos;

    private Vector3 checkPos;
    private float checkRadius;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
        playerScore = GetComponent<PlayerScore>();
        groundCheck = GetComponent<GroundCheck>();
        partsParent = FindObjectOfType<LevelMaker>().transform;
    }

    public void CheckDestroyParts(Collider collider)
    {
        if (playerScore.ComboCounter > 1)
        {
            if (collider.transform.parent == null)
            {
                return;
            }

            int combo = playerScore.ComboCounter - 1;

            /*Transform part = collider.transform.parent.parent.parent;
            part.GetChild(0).GetComponent<DestructiblePart>().DestroyPart();
            part.GetChild(1).GetComponent<DestructiblePart>().DestroyPart();
            int siblingIndex = part.GetSiblingIndex();
            int spiral = part.parent.GetSiblingIndex();*/

            //combo = 5;

            if (combo < 5)
            {
                Vector3 destroyPos = transform.position;
                destroyPos.y = collider.transform.position.y;

                checkPos = destroyPos;
                checkRadius = (float)combo / 10 + 0.3f;
            }
            else
            {
                checkRadius = 1;
                Vector3 towerPos = partsParent.parent.position;
                towerPos.y = collider.transform.position.y;
                checkPos = towerPos;
            }

            Collider[] parts = Physics.OverlapSphere(checkPos, checkRadius, layerMask);

            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].GetComponentInParent<DestructiblePart>().DestroyPart();
            }

            //partsToDestroy = 5;

            /*if (partsToDestroy == 2)
            {
                if (CheckInRange(siblingIndex + 1, spiral))
                {
                    Transform selectedPart = partsParent.GetChild(spiral).GetChild(siblingIndex + 1);
                    Debug.Log("Part is: " + selectedPart);
                    selectedPart.GetChild(0).GetComponent<DestructiblePart>().DestroyPart();
                    selectedPart.GetChild(1).GetComponent<DestructiblePart>().DestroyPart();
                }
                else if (CheckInRange(siblingIndex - 1, spiral))
                {
                    Transform selectedPart = partsParent.GetChild(spiral).GetChild(siblingIndex - 1);
                    selectedPart.GetChild(0).GetComponent<DestructiblePart>().DestroyPart();
                    selectedPart.GetChild(1).GetComponent<DestructiblePart>().DestroyPart();
                }
            }
            else if (partsToDestroy > 2)
            {
                for (int i = 0; i < partsToDestroy; i++)
                {
                    int index = 0 - (partsToDestroy - 2);
                    index += i;

                    if (CheckInRange(siblingIndex + index, spiral))
                    {
                        Transform selectedPart = partsParent.GetChild(spiral).GetChild(siblingIndex + index);
                        selectedPart.GetChild(0).GetComponent<DestructiblePart>().DestroyPart();
                        selectedPart.GetChild(1).GetComponent<DestructiblePart>().DestroyPart();
                    }
                }
            }
            else if (partsToDestroy >= 5)
            {
                part.GetComponentInParent<DestroyAllParts>().DestroyAllPartsInSpiral();
            }*/

            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * bounceForce);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 0)
        {
            if (collision.gameObject.CompareTag("DeathPart"))
            {
                Vector3 dirToPlayer = transform.position - collision.transform.position;
                dirToPlayer.Normalize();

                Debug.Log("Direction to player: " + dirToPlayer);

                if (playerScore.ComboCounter > 1 && dirToPlayer.y > 0)
                {
                    return;
                }
                
                playerScore.ResetLevel();
            }

            //rb.AddForce(collision.contacts[0].normal * bounceForce);
        }
        else if (collision.gameObject.CompareTag("End"))
        {
            GameManager.instance.NextLevel(playerScore.Score);
        }
    }

    private bool CheckInRange(int index, int spiral)
    {
        if (index > 0 && index <= partsParent.GetChild(spiral).childCount - 2)
        {
            if (partsParent.GetChild(spiral).GetChild(index).CompareTag("Point"))
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(checkPos, checkRadius);
    }
}
