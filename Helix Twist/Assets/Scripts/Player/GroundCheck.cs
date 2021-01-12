using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private bool grounded = false;
    [SerializeField] private float raycastLength = 0.2f;
    [SerializeField] private LayerMask layerMask;
    public bool Grounded { get { return grounded; } }

    private PlayerSwipe playerSwipe;
    private PlayerCollision playerCol;
    private PlayerScore playerScore;

    // Start is called before the first frame update
    void Start()
    {
        playerSwipe = GetComponent<PlayerSwipe>();
        playerScore = GetComponent<PlayerScore>();
        playerCol = GetComponent<PlayerCollision>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastLength, layerMask))
        {
            if (!hit.collider.isTrigger)
            {
                if (!grounded)
                {
                    playerCol.CheckDestroyParts(hit.collider);
                }

                grounded = true;
                playerSwipe.ResetSwipe();                
                playerScore.ResetCombo();
            }
        }
        else
        {
            grounded = false;
        }
    }
}
