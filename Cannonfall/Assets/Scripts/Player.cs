using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Packages.Rider.Editor.UnitTesting;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    [SerializeField] // show and edit in Inspector
    private float moveForce;
    [SerializeField]
    private float jumpForce;
    private bool isGrounded = true;
    private bool doubleJump;
    private Rigidbody2D body;
    private string GROUND_TAG = "Ground";
    [SerializeField]
    private GameObject cannonballObject;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>(); // get the reference to the Rigidbody2D  
    }

    // called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GROUND_TAG))
        {
            isGrounded = true;
            doubleJump = true;
        }
    }

    private void Move() // horizontal movement
    {
        float movementX = Input.GetAxis("Horizontal");
        if (movementX < 0)
            Debug.Log("Left");
        if (movementX > 0)
            Debug.Log("Right");
        transform.position += new Vector3(movementX, 0f) * Time.deltaTime * moveForce;
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
            Debug.Log("Jump");
        if (Input.GetButtonDown("Jump") && doubleJump && !isGrounded) // if not on ground but has a double jump charge
            {
                body.linearVelocity = new Vector3(body.linearVelocityX, 0f, 0f); // continues with x velocity, resets y to 0
                doubleJump = false;
                body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                GameObject cannonball = Instantiate(cannonballObject); // create a cannonball
                cannonball.transform.position = body.transform.position; // sets it position to that of the player
            }
        if (Input.GetButtonDown("Jump") && isGrounded) // if jump button and on ground
        {
            isGrounded = false;
            body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        
    }

    
}
