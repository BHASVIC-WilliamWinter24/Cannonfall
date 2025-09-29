using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    private Vector3 tempPos;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform; // gets reference for player transform

    }
    void LateUpdate() // executes before update every frame
    {
        tempPos = player.position;
        if (tempPos.y < 0)
            tempPos.y = 0;
        tempPos.z = -10;
        transform.position = tempPos;
    }
}
