using UnityEngine;

public class Shot : MonoBehaviour
{
    public bool direction; // true = left, false = right
    [SerializeField] private float shotForce;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            float randomAngle = Random.Range(0f, 22.5f); // random from 0 to 45 degrees
            int randomSign = Random.Range(0, 2); // 50/50 chance of being 0 - 1
            if (randomSign == 0)
                randomSign = -1; // makes it so -1 or 1 (going diagonally up or down)
            int shoot; // which direction it goes horizontally
            if (direction)
                shoot = 1;
            else
                shoot = -1;
            float xForce = shoot * shotForce * Mathf.Cos(randomAngle * (Mathf.PI / 180)); // force * cos(theta) with radians
            float yForce = randomSign * shotForce * Mathf.Sin(randomAngle * (Mathf.PI / 180)); // force * sin(theta) with radians
            child.GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce, yForce), ForceMode2D.Impulse); // apply force
            child.SetParent(null);
        }
        Destroy(gameObject);
    }
}
