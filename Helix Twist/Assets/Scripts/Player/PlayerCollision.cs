using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision: MonoBehaviour
{
    private Rigidbody rb;
    private PlayerScore playerScore;
    private PlayerSoundEffects playerSound;
    private GroundCheck groundCheck;
    private Transform partsParent;
    private ParticleSystem ps;
    private MeshRenderer meshRenderer;
    private TrailRenderer trail;
    private HUD ui;
    private CameraShake camShake;

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
        ui = FindObjectOfType<HUD>();
        camShake = FindObjectOfType<CameraShake>();
        playerScore = GetComponent<PlayerScore>();
        playerSound = GetComponent<PlayerSoundEffects>();
        groundCheck = GetComponent<GroundCheck>();
        partsParent = FindObjectOfType<LevelMaker>().transform;
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        ps = GetComponentInChildren<ParticleSystem>();
        ParticleSystem.MainModule main = ps.main;

        Color playerColor = meshRenderer.material.color;

        main.startColor = playerColor;
        trail = GetComponentInChildren<TrailRenderer>();
        trail.startColor = playerColor;
        playerColor.a = 0;
        trail.endColor = playerColor;
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

                if (combo <= 2)
                {
                    playerSound.PlayBreakingSound(0);
                    StartCoroutine(camShake.Shake(0.1f, 0.05f));
                }
                else
                {
                    playerSound.PlayBreakingSound(1);
                    StartCoroutine(camShake.Shake(0.1f, 0.1f));
                }                
            }
            else
            {
                checkRadius = 1;
                Vector3 towerPos = partsParent.parent.position;
                towerPos.y = collider.transform.position.y;
                checkPos = towerPos;
                playerSound.PlayBreakingSound(2);
                StartCoroutine(camShake.Shake(0.1f, 0.2f));
            }

            Collider[] parts = Physics.OverlapSphere(checkPos, checkRadius, layerMask);

            for (int i = 0; i < parts.Length; i++)
            {
                if (!parts[i].isTrigger)
                {
                    DestructiblePart destructiblePart = parts[i].GetComponentInParent<DestructiblePart>();
                    destructiblePart.DestroyPart();

                    GameObject twoPart = destructiblePart.transform.parent.gameObject;

                    if (!playerScore.PointParts.Contains(twoPart))
                    {
                        playerScore.PointParts.Add(twoPart);
                    }
                }
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
            playerScore.ResetCombo();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 0)
        {
            if (playerScore.ComboCounter > 1)
            {
                CheckDestroyParts(collision.collider);
                return;
            }

            if (collision.gameObject.CompareTag("DeathPart"))
            {
                //Debug.Log("Direction to player: " + dirToPlayer);                

                meshRenderer.enabled = false;
                trail.gameObject.SetActive(false);
                rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
                rb.isKinematic = true;
                ps.Play();
                Invoke("Die", 2);
                playerSound.PlayDeathSound();
                return;
            }

            playerSound.PlayLandingSound();
            //rb.AddForce(collision.contacts[0].normal * bounceForce);
        }
        else if (collision.gameObject.CompareTag("End"))
        {
            ui.ShowLevelCompleteText(GameManager.instance.Level + 1);
            playerScore.ResetCombo();
        }
    }

    private void Die()
    {
        ui.StartFade(true);
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
