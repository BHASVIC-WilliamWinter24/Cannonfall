using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Timeline;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveForce; // horizontal speed
    [SerializeField] private float jumpForce; // force of jump
    private bool isGrounded = true; // if touching ground
    public bool groundBelow; // if that ground is below
    private bool doubleJump; // if has double jumped
    private bool isAlive = true; // if is alive
    private Rigidbody2D body; // reference for Rigidbody
    private string GROUND_TAG = "Ground"; // Ground tag
    private string HAZARD_TAG = "Hazard"; // Hazard tag
    private string ENEMY_TAG = "Enemy"; // Enemy tag
    [SerializeField] private GameObject cannonballObject; // reference to Cannonball 
    [SerializeField] private GameObject blackScreen; // reference to BlackScreen
    private Vector3 respawnPosition = new Vector3(0, 0, 0); // respawn position (accessed by Checkpoint)
    private GameObject activeCheckpoint; // holds active checkpoint reference
    private bool findCheckpoint = false;
    private float wallSliding = 0f;
    

    void Awake()
    {
        body = GetComponent<Rigidbody2D>(); // get the reference to the Rigidbody2D 
        GameManager.instance.Player = gameObject;
    }

    void Start()
    {
        if (!File.Exists(Application.persistentDataPath + "slot" + GameManager.instance.SaveSlot + ".save")) // if no save yet
        {
            SaveSystem.Save();
        }
        if (SaveSystem.readFileScene(GameManager.instance.SaveSlot) == SceneManager.GetActiveScene().name) // if loading to this scene
        {
            SaveSystem.Load();
        }
    }

    void Update()
    {
        if (Time.timeScale != 0 || !isAlive) // as long as game is not paused and is alive
        {
            Move();
            Jump();
            if (!isGrounded)
                body.linearVelocityX *= 0.9f; // slows jumps through air slightly
            if (wallSliding != 0 && wallSliding != transform.position.x) // if was frictionless but has now moved from the wall 
            {
                wallSliding = 0;
                GetComponent<CapsuleCollider2D>().sharedMaterial = null;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(HAZARD_TAG) && isAlive || collision.gameObject.CompareTag(ENEMY_TAG) && isAlive)
        {
            isAlive = false;
            StartCoroutine(playerDeath());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GROUND_TAG) && groundBelow)
        {
            isGrounded = true;
            doubleJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GROUND_TAG))
            isGrounded = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(HAZARD_TAG) && isAlive)
        {
            StartCoroutine(playerDeath());
        }
        if (collision.name == "Checkpoint" && findCheckpoint == true)
        {
            activeCheckpoint = collision.gameObject; // set touching checkpoint as the active one
            activeCheckpoint.GetComponent<Checkpoint>().activateCheckpoint();
            findCheckpoint = false; // no longer need to find a checkpoint
        }
    }

    private void Move() // horizontal movement
    {
        float movementX = Input.GetAxis("Horizontal");
        body.linearVelocity = new Vector2(movementX * moveForce, body.linearVelocityY);
        // forcibly slide down walls
        if (body.linearVelocityX != 0 && !groundBelow && body.linearVelocityY == 0) 
        {
            PhysicsMaterial2D frictionless = new PhysicsMaterial2D();
            frictionless.friction = 0;
            GetComponent<CapsuleCollider2D>().sharedMaterial = frictionless;
            wallSliding = transform.position.x;
        }


        /*
        body.AddForce(new Vector2(movementX * moveForce, 0f), ForceMode2D.Impulse);
        if (body.linearVelocityX > 10)
            body.linearVelocityX = 10;
        else if (body.linearVelocityX < -10)
            body.linearVelocityX = -10;
        */
        //transform.position += new Vector3(movementX, 0f) * Time.deltaTime * moveForce;
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && doubleJump && !isGrounded) // if not on ground but has a double jump charge
        {
            body.linearVelocity = new Vector3(body.linearVelocityX, 0f, 0f); // continues with x velocity, resets y to 0
            doubleJump = false;
            body.AddForce(new Vector2(0, jumpForce+2), ForceMode2D.Impulse);
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
        blackScreen.GetComponent<FadeUI>().fadeIn = true;
        yield return new WaitForSeconds(1.5f);
        transform.position = respawnPosition;
        yield return new WaitForSeconds(0.5f);
        blackScreen.GetComponent<FadeUI>().fadeIn = false;
        onDeath?.Invoke();
        isAlive = true;
    }

    public delegate void OnDeath();
    public static event OnDeath onDeath;

    public void setActiveCheckpoint(GameObject checkpoint)
    {
        activeCheckpoint = checkpoint; // get object 
        respawnPosition = checkpoint.GetComponent<Checkpoint>().getPosition(); // get respawn position
    }

    public Vector3 getRespawnPosition()
    {
        return respawnPosition;
    }

    #region saveAndLoad

    public void Save(ref PlayerSaveData data)
    {
        data.checkpointPosition = respawnPosition; // save the value of the activeCheckpoint
        data.time = System.DateTime.Now.ToString("HH:mm dd/MM/yy"); // save the current value of the time
        data.currentScene = SceneManager.GetActiveScene().name; // save the name of the current scene
    }

    public void Load(PlayerSaveData data)
    {
        respawnPosition = data.checkpointPosition; // set the respawnPosition to the one in the file
        if (respawnPosition == null || respawnPosition == new Vector3(0, 0, 0)) // if null or no checkpoint
        {
            respawnPosition = new Vector3(0, 0, 0);
            transform.position = respawnPosition;
        }
        else
        {
            transform.position = respawnPosition;
            findCheckpoint = true;
        }
    }

    #endregion
}

[System.Serializable]
public struct PlayerSaveData
{
    public Vector3 checkpointPosition;
    public string time;
    public string currentScene;
}