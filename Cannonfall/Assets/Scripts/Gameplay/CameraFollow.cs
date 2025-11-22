using UnityEngine;
using UnityEngine.SceneManagement;

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
        #region Camera Limits
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            
            // at start
            if (tempPos.y > - 14 && tempPos.y < -7 && tempPos.x < 15)
                tempPos.y = -7; // bottom limit 0
            if (tempPos.y < 10 && tempPos.y > 0 && tempPos.x > 9 && tempPos.x < 14)
                tempPos.x = 9; // right limit 6
            // first gap
            if (tempPos.y > -14 && tempPos.y < -6.5f && tempPos.x < -26)
                tempPos.y = -6.5f; // bottom limit 4
            // diagonal
            if (tempPos.x >= -46 && tempPos.x < -44 && tempPos.y > 14 && tempPos.y < 2 * tempPos.x + 106)
                tempPos.y = 2 * tempPos.x + 106; // bottom line y = 2x + 106
            if (tempPos.x >= -44 && tempPos.x < -36 && tempPos.y > 16 && tempPos.y < 0.5f * tempPos.x + 40)
                tempPos.y = 0.5f * tempPos.x + 40; // bottom line y = 0.5x + 40
            if (tempPos.x > -36 && tempPos.x < -32 && tempPos.y > 16 && tempPos.y < 22)
                tempPos.y = 22; // bottom limit 22
            if (tempPos.x >= -32 && tempPos.x < -26 && tempPos.y > 16 && tempPos.y < (-0.5f * tempPos.x) + 6)
                tempPos.y = (-0.5f * tempPos.x) + 6; // buttom line y = -0.5x + 6
            if (tempPos.x >= -26 && tempPos.x < 21 && tempPos.y > 16 && tempPos.y < 19)
                tempPos.y = 19; // bottom limit 19
            //at bottom 
            if (tempPos.y < -35 && tempPos.x < -22 && tempPos.x > -106) 
                tempPos.y = -35;
        }
        #endregion
        /*
        #region Colliders
        if (limit != null)
        {
            if (lastPos.x != tempPos.x && limit.GetComponent<BoxCollider2D>().bounds.Contains(tempPos))
                if (!limit.GetComponent<BoxCollider2D>().bounds.Contains(lastPos))
                    tempPos.x = lastPos.x;
            if (lastPos.y != tempPos.y && limit.GetComponent<BoxCollider2D>().bounds.Contains(tempPos))
                if (!limit.GetComponent<BoxCollider2D>().bounds.Contains(lastPos))
                    tempPos.y = lastPos.y;
        }
        tempPos.z = -10;
        lastPos = tempPos;
        transform.position += new Vector3(lastPos.x - tempPos.x, lastPos.y - tempPos.y, 0);
        #endregion
        */
        tempPos.z = -10;
        transform.position = tempPos;
    }
}
