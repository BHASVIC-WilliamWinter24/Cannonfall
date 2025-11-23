using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float scaleSpeed;

    void Update()
    {
        Vector3 scale = transform.localScale; // get current
        Vector3 desiredScale = new Vector3(1, 1, 1); // set desired
        scale.x = Mathf.MoveTowards(scale.x, desiredScale.x, scaleSpeed * Time.deltaTime); // scale x
        scale.y = Mathf.MoveTowards(scale.y, desiredScale.y, scaleSpeed * Time.deltaTime); // scale y
        transform.localScale = scale; // change scale to match
        if (transform.localScale.x == 1)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            collision.GetComponent<Enemy>().EnemyDeath();
    }
}
