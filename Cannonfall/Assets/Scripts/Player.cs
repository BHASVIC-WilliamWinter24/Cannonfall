using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveForce; // horizontal speed
    [SerializeField] private float jumpForce; // force of jump
    private bool isGrounded = true; // if on ground
    private bool doubleJump; // if has double jumped
    private bool isAlive = true; // if is alive
    private Rigidbody2D body; // reference for Rigidbody
    private string GROUND_TAG = "Ground"; // Ground tag
    private string HAZARD_TAG = "Hazard"; // Hazard tag
    private string ENEMY_TAG = "Enemy"; // Enemy tag
    [SerializeField] private GameObject cannonballObject; // reference to Cannonball 
    [SerializeField] private GameObject blackScreen; // reference to BlackScreen
    private Vector3 respawnPosition = new Vector3(0, 0, 0); // respawn position (accessed by Checkpoint)

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
        if (collision.gameObject.CompareTag(HAZARD_TAG) && isAlive || collision.gameObject.CompareTag(ENEMY_TAG) && isAlive)
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
        transform.position = respawnPosition;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(blackScreen.GetComponent<FadeToBlack>().FadeBlackScreen(false));
        onDeath?.Invoke();
        isAlive = true;
    }

    public delegate void OnDeath();
    public static event OnDeath onDeath;

    public void setRespawnPosition(Vector3 newPosition)
    {
        respawnPosition = newPosition;
    }

    public Vector3 getRespawnPosition()
    {
        return respawnPosition;
    }
}
