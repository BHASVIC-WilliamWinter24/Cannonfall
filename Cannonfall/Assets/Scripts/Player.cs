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
    private bool isAlive = true;
    private Rigidbody2D body;
    private string GROUND_TAG = "Ground";
    private string HAZARD_TAG = "Hazard";
    [SerializeField]
    private GameObject cannonballObject;
    [SerializeField]
    private GameObject blackScreen;

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
        if (collision.gameObject.CompareTag(HAZARD_TAG) && isAlive)
        {
            isAlive = false;
            StartCoroutine(playerDeath());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(HAZARD_TAG) && isAlive)
        {
            StartCoroutine(playerDeath());
        }
    }

    private void Move() // horizontal movement
    {
        float movementX = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movementX, 0f) * Time.deltaTime * moveForce;
    }

    private void Jump()
    {
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

    private IEnumerator playerDeath()
    {
        StartCoroutine(blackScreen.GetComponent<FadeToBlack>().FadeBlackScreen(true));
        yield return new WaitForSeconds(1.5f);
        transform.position = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(blackScreen.GetComponent<FadeToBlack>().FadeBlackScreen(false));
        isAlive = true;
    }
    
}
