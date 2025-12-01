using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using Unity.VisualScripting;
using Unity.Collections;

public class Player : MonoBehaviour
{
    #region attributes
    [SerializeField] private float moveForce; // horizontal speed
    [SerializeField] private float jumpForce; // force of jump
    private bool isGrounded = true; // if touching ground
    public bool groundBelow; // if that ground is below
    private bool doubleJump; // if has double jumped
    private bool tripleJump = false; // if has triple jump
    private bool isAlive = true; // if is alive
    private Rigidbody2D body; // reference for Rigidbody
    private string GROUND_TAG = "Ground"; // Ground tag
    private string HAZARD_TAG = "Hazard"; // Hazard tag
    private string ENEMY_TAG = "Enemy"; // Enemy tag
    [SerializeField] private GameObject cannonballObject; // reference to Cannonball 
    [SerializeField] private GameObject blackScreen; // reference to BlackScreen
    private Vector3 respawnPosition = new Vector3(0, 0, 0); // respawn position (accessed by Checkpoint)
    private GameObject activeCheckpoint; // holds active checkpoint reference
    private bool findCheckpoint = false; // if needs to get checkpoint after saving
    private float wallSliding = 0f; // x-coord when wallsliding
    [SerializeField] private int[] upgradeList = {0, 0, 0, 0, 0, 0}; // holds all upgrades (0 = inactive, 1 = active)
    [SerializeField] private GameObject upgradeMenu; // holds reference to upgrade menu
    private bool direction = true; // true = left, false = right
    private int reloadTimer; 
    [SerializeField] private int dashForce;
    [SerializeField] private bool dashing; // true while dashing
    [SerializeField] private GameObject scattershot;
    #endregion

    void Awake()
    {
        body = GetComponent<Rigidbody2D>(); // get the reference to the Rigidbody2D 
        GameManager.instance.Player = gameObject;
    }

    void Start()
    {
        upgradeList = new int[6]; // ensures that list has a length of 5
        cannonballObject.GetComponent<Cannonball>().upgradeActive = false;
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
        if (Time.timeScale != 0) // as long as game is not paused
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
            if (Input.GetKeyDown(KeyCode.Q) && reloadTimer <= 0 && isGrounded && upgradeList[5] == 1)
            {
                if (direction && dashForce > 0) // if moving left and applying right
                    dashForce *= -1; // flip force
                else if (!direction && dashForce < 0) // if moving right and applying left
                    dashForce *= -1; // flip force
                StartCoroutine(Dash()); // dash
            }
            if (reloadTimer > 0) 
                reloadTimer -= 1;
        }
        /* for testing
        if (Input.GetKeyDown(KeyCode.K))
        {
            upgradeList[0] = 1;
            upgradeList[1] = 1;
            //upgradeList[2] = 1;
            upgradeList[3] = 1; cannonballObject.GetComponent<Cannonball>().upgradeActive = true;
            //upgradeList[4] = 1;
        }
        */
    }

    void FixedUpdate()
    {
        if (dashing)
            body.linearVelocity = new(dashForce, 0);
    }

    #region collisions
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
            if (upgradeList[2] == 1) // if triple jump upgrade active
                tripleJump = true;
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
        if (collision.CompareTag("Checkpoint") && findCheckpoint == true)
        {
            activeCheckpoint = collision.gameObject; // set touching checkpoint as the active one
            activeCheckpoint.GetComponent<Checkpoint>().activateCheckpoint();
            findCheckpoint = false; // no longer need to find a checkpoint
        }
        if (collision.CompareTag("Upgrade"))
        {
            int upgradeIndex = collision.GetComponent<UpgradeToken>().getUpgrade(); // get index 
            upgradeList[upgradeIndex] = 1; // set index of list to 'collected'
            upgradeMenu.GetComponent<UpgradeMenu>().Activate(upgradeIndex); // activate menu
            Destroy(collision.gameObject); // destroy upgrade token
            if (upgradeIndex == 3) // if explosive upgrade
                cannonballObject.GetComponent<Cannonball>().upgradeActive = true; // set explosive cannonballs
        }
    }
    #endregion

    private void Move() // horizontal movement
    {
        float movementX = Input.GetAxis("Horizontal");
        // flip
        if (movementX < 0 && transform.localScale.x > 0) // if moving left and not flipped 
            transform.localScale *= new Vector2(-1, 1); // flip on y
        else if (movementX > 0 && transform.localScale.x < 0) // if moving right and flipped
            transform.localScale *= new Vector2(-1, 1); // flip on y
        // direction
        if (movementX < 0) // if moving left
            direction = true;
        else if (movementX > 0) // if moving right
            direction = false;
        // move
        float move = moveForce + (upgradeList[0] * 0.1f * moveForce); // change moveForce depending on upgrade
        body.linearVelocity = new Vector2(movementX * move, body.linearVelocityY);
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
        float jump = jumpForce + (upgradeList[1] * 0.05f * jumpForce);  // change jumpforce depending on upgrades
        if (Input.GetButtonDown("Jump") && !doubleJump && !isGrounded && tripleJump) // if jump after double and has triple
        {
            body.linearVelocity = new Vector3(body.linearVelocityX, 0f, 0f); // continues with x velocity, resets y to 0
            tripleJump = false;
            body.AddForce(new Vector2(0, jump+2), ForceMode2D.Impulse);
            GameObject cannonball1 = Instantiate(cannonballObject); // create a cannonball
            cannonball1.transform.position = body.transform.position; // sets it position to that of the player
            if(upgradeList[4] == 1)
            {
                GameObject cannonball2 = Instantiate(cannonballObject);
                cannonball2.transform.position = body.transform.position + new Vector3(0.4f, 0, 0);
                cannonball1.transform.position = body.transform.position - new Vector3(0.4f, 0, 0);
            }
        }
        if (Input.GetButtonDown("Jump") && doubleJump && !isGrounded) // if not on ground but has a double jump charge
        {
            body.linearVelocity = new Vector3(body.linearVelocityX, 0f, 0f); // continues with x velocity, resets y to 0
            doubleJump = false;
            body.AddForce(new Vector2(0, jump+2), ForceMode2D.Impulse);
            GameObject cannonball3 = Instantiate(cannonballObject); // create a cannonball
            cannonball3.transform.position = body.transform.position; // sets it position to that of the player
            if (upgradeList[4] == 1)
            {
                GameObject cannonball4 = Instantiate(cannonballObject);
                cannonball4.transform.position = body.transform.position + new Vector3(0.3f, 0, 0);
                cannonball3.transform.position = body.transform.position - new Vector3(0.3f, 0, 0);
            }
        }
        if (Input.GetButtonDown("Jump") && isGrounded) // if jump button and on ground
        {
            isGrounded = false;
            body.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
        }
    }


    private IEnumerator Dash()
    {
        reloadTimer = 60;
        dashing = true;
        scattershot.GetComponent<Shot>().direction = direction;
        GameObject shot = Instantiate(scattershot);
        shot.transform.position = transform.position;
        yield return new WaitForSeconds(0.25f);
        dashing = false;
    }

    private IEnumerator playerDeath()
    {
        body.linearVelocity = new Vector2(0, 0); // stop velocity
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
        if (checkpoint != null)
            respawnPosition = checkpoint.GetComponent<Checkpoint>().getPosition(); // get respawn position
        else
            respawnPosition = new Vector3(0, 0, 0);
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
        data.upgrades = upgradeList; // save upgrade list
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
        upgradeList = data.upgrades;
    }

    #endregion
}

[System.Serializable]
public struct PlayerSaveData
{
    public Vector3 checkpointPosition;
    public string time;
    public string currentScene;
    public int[] upgrades;
}