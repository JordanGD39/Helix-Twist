using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwipe : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float swipeForce = 5;
    private bool firstTouch = true;
    private bool canSwipe = true;
    private Vector2 previousTouchPos;
    private Vector3 diff;

    private Camera cam;
    private HUD ui;
   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ui = FindObjectOfType<HUD>();

        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckSwipe();

        /*if (diff != Vector3.zero)
        {
            Debug.DrawRay(transform.position, diff, Color.cyan);
        }*/
    }

    public void ResetSwipe()
    {
        canSwipe = true;
    }

    private void CheckSwipe()
    {
        if (canSwipe && Input.GetMouseButton(0) /*Input.touchCount > 0*/)
        {
            Vector2 mousePos = Input.mousePosition;/*Input.GetTouch(0).position*/

            if (firstTouch)
            {
                firstTouch = false;
                previousTouchPos = mousePos;
                return;
            }            

            diff = mousePos - previousTouchPos;
            diff.Normalize();

            //Debug.Log(diff);

            if (diff.magnitude > 0.3f)
            {
                if (ui.LevelPanel.activeSelf)
                {
                    ui.RemoveStartPanel();                    
                }

                float y = diff.y;

                diff = cam.transform.TransformVector(diff);
                diff.y = y;

                rb.velocity = diff * swipeForce;
                canSwipe = false;
            }

            previousTouchPos = mousePos;
        }
        else
        {
            firstTouch = true;
        }
    }
}
