using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int level;
    public int lives;
    private int score;

    public int LugiCounter;
    public bool LugiMode;

    private GameObject bgm;
    public AudioClip startBGM;
    public AudioClip lugi;

    private void Start()
    {
        bgm = GameObject.Find("MusicPlayer");
        DontDestroyOnLoad(gameObject);
        LugiMode = false;

        NewGame();
    }

    private void NewGame()
    {
        lives = 3;
        score = 0;

        LoadLevel(1);
    }

    private void LoadLevel(int index)
    {
        level = index;
        Physics2D.IgnoreLayerCollision(0, 0, false);

        Camera camera = Camera.main;

        if (camera != null) {
            camera.cullingMask = 0;
        }

        Invoke(nameof(LoadScene), 1f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(level);
        bgm.GetComponent<MusicController>().PlayMusic(0);
    }

    public void LevelComplete()
    {
        score += 1000;

        int nextLevel = level + 1;
        
        if (nextLevel < SceneManager.sceneCountInBuildSettings) {
            LoadLevel(nextLevel);
        } else {
            LoadLevel(1);
        }
    }

    public void LevelFailed()
    {
        lives--;

        if (lives <= 0) {
            NewGame();
        } else {
            LoadLevel(level);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            LugiCounter += 1;
            if (LugiCounter == 5) {
                bgm.GetComponent<MusicController>().PlayClip2D(lugi);
                LugiMode = true;
            }
        }
    }
}
