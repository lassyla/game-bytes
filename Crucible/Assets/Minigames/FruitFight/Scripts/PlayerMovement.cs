using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int player_num;
    public Tongue tongue;
    public Animator anim; 
    private bool grounded = true;
    private float horizontalSpeed = .2f;
   // private float rotationSpeed = .2f;
    private float jumpPower = 1000.0f;
    private Rigidbody rb;
    private Vector3 tonguePos;
    private Vector3 tongueDestination;
    private float tongueDistance = 8.5f;
    private Vector3 facingDirection; 


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float moveHorizontal = MinigameInputHelper.GetHorizontalAxis(player_num);
        float moveVertical = MinigameInputHelper.GetVerticalAxis(player_num);

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        anim.SetFloat("Speed", 0);

        //when tongue is not moving, you can run and jump freely 
       // if (!tongue.moving)
        {
            tonguePos = transform.position;

            if (movement.magnitude > 0.1)
            {
                rb.MovePosition(transform.position + movement * horizontalSpeed);

                Quaternion q = Quaternion.LookRotation(-1 * movement, Vector3.up);
                facingDirection = Vector3.Normalize(movement);
                rb.MoveRotation(q);
                facingDirection = Vector3.Normalize(movement);
                anim.SetFloat("Speed", moveHorizontal * moveHorizontal + moveVertical * moveVertical);

            }
            if (grounded && MinigameInputHelper.IsButton1Held(player_num))
            {
                anim.SetBool("Grounded",false);
                anim.SetTrigger("Jump");

                grounded = false;
                rb.AddForce(Vector3.up * jumpPower);
            }
            if (!grounded && rb.transform.position.y < .01)
            {
                grounded = true;
                anim.SetBool("Grounded", true);

            }
            if (MinigameInputHelper.IsButton2Held(player_num))
            {
                anim.SetTrigger("Tongue");
                UnityEngine.Debug.Log(player_num + "tongue out");
                tongue.setDestination(transform.position + facingDirection * tongueDistance);
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "fruit")
        {
            Destroy(other.gameObject);
            MinigameController.Instance.AddScore(player_num, 1);
            UnityEngine.Debug.Log("added point");
        }

    }
}
