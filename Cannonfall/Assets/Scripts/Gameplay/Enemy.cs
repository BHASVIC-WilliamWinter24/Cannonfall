using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float chaseDistance;
    private float moveCapLeft;
    private float moveCapRight;
    private GameObject player;
    private Rigidbody2D body;
    public Vector3 spawnpoint;
    public bool jumping;
    public bool ledgeBelowLeft;
    public bool ledgeBelowRight;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        moveCapLeft = -99999;
        moveCapRight = 99999;
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (!(transform.position.x - moveCapLeft < 0.5 ) && !(moveCapRight - transform.position.x < 0.5))
        {
            if (body.linearVelocityX < 0 && transform.localScale.x > 0) // if moving left and not flipped 
            {
                transform.localScale *= new Vector2(-1, 1); // flip on y
                // flip the ledge detectors so that they cap the correct way
                gameObject.transform.GetChild(2).transform.localScale *= new Vector2(-1, 1);
                gameObject.transform.GetChild(3).transform.localScale *= new Vector2(-1, 1);
            }
            else if (body.linearVelocityX > 0 && transform.localScale.x < 0) // if moving right and flipped
            {
                transform.localScale *= new Vector2(-1, 1); // flip on y
                // flip the ledge detectors so that they cap the correct way
                gameObject.transform.GetChild(2).transform.localScale *= new Vector2(-1, 1);
                gameObject.transform.GetChild(3).transform.localScale *= new Vector2(-1, 1);
            }
        }
        if (distance < chaseDistance)
        {
            if (transform.position.x < moveCapLeft) // if beyond cap
            {
                setMoveSpeedSign(true); // make moveSpeed positive
                body.linearVelocity = new Vector3(moveSpeed, body.linearVelocity.y, 0); // return to within cap
            }
            if (transform.position.x > moveCapRight) // if beyond cap
            {
                setMoveSpeedSign(false); // make moveSpeed negative
                body.linearVelocity = new Vector3(moveSpeed, body.linearVelocity.y, 0); // return to within cap
            }
            if (player.transform.position.x < transform.position.x && transform.position.x > moveCapLeft) // if player is left of enemy & right of cap
            {
                setMoveSpeedSign(false); // make moveSpeed negative
                body.linearVelocity = new Vector3(moveSpeed, body.linearVelocity.y, 0);
            }
            else if (player.transform.position.x > transform.position.x && transform.position.x < moveCapRight) // if player is right of enemy & left of cap
            {
                setMoveSpeedSign(true); // make moveSpeed positive
                body.linearVelocity = new Vector3(moveSpeed, body.linearVelocity.y, 0);
            }
        }
        else // player is out of range
        {
            if (transform.position.x < spawnpoint.x - 3) // if left of spawnpoint
            {
                setMoveSpeedSign(true);
                body.linearVelocity = new Vector3(moveSpeed, body.linearVelocity.y, 0);
            }
            else if (transform.position.x > spawnpoint.x + 3) // if right of spanpoint
            {
                setMoveSpeedSign(false);
                body.linearVelocity = new Vector3(moveSpeed, body.linearVelocity.y, 0);
            }
            else // around spawnpoint
            {
                body.linearVelocity = new Vector3(0, body.linearVelocity.y, 0); // stamd still
            }
        }
    }

    void setMoveSpeedSign(bool positive)
    {
        if (positive)
        {
            if (moveSpeed < 0)
                moveSpeed = -moveSpeed;
        }
        else
        {
            if (moveSpeed > 0)
                moveSpeed = -moveSpeed;
        }
    }

    public void reachedEdge()
    {
        if (!jumping) /// if not jumping and no ledge below
        {
            if (moveSpeed < 0 && !ledgeBelowLeft) // if moving left & no ledge on left
            {
                moveCapLeft = transform.position.x + 0.5f; // set cap to here (little offset so doesn't hang so much)
                body.linearVelocityX = 0; // stop moving left
            }
            else if (moveSpeed > 0 && !ledgeBelowRight) // if moving right & no ledge on right
            {
                moveCapRight = transform.position.x - 0.5f; // set cap to here (little offset so doesn't hang so much)
                body.linearVelocityX = 0; // stop moving right
            }
        }
    }

    public void jumpLedge()
    {
        jumping = true;
        body.AddForce(new Vector2(0, 3), ForceMode2D.Impulse); // push upwards slightly
    }

    public void EnemyDeath()
    {
        Destroy(gameObject); // destroy self
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
            Destroy(gameObject);
    }
}
