using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int saveSlot;
    public int SaveSlot { get { return saveSlot; } set { saveSlot = value; } }
    [SerializeField] private GameObject player;
    public GameObject Player { get { return player; } set { player = value; } }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        player = GameObject.FindWithTag("Player"); // set player
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SceneManager.LoadScene("Level 2");
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SceneManager.LoadScene("Level 3");
        if (Input.GetKeyDown(KeyCode.O))
            File.Delete(Application.persistentDataPath + "slot" + saveSlot + ".save");
        if (player == null && GameObject.FindWithTag("Player") != null)
            player = GameObject.FindWithTag("Player");
    }

}
