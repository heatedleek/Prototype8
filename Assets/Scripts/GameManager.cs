using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 3.0f;
    private ScreenShake screenShake;
    private bool inProgress;
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        // initialize screen utils
        AudioManager.Initialize(audioSource);
        ScreenUtils.Initialize();
    }
    // Start is called before the first frame update
    void Start()
    {
        inProgress = true;
        screenShake = Camera.main.GetComponent<ScreenShake>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForDebugInput();
    }

    void CheckForDebugInput()
    {
        // restart
        if (Input.GetKey(KeyCode.R))
        {
            RestartGame();
        }
        // restart level
        if (Input.GetKey(KeyCode.L))
        {
            RestartLevel();
        }
        // quit
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Level1");
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("Level2");
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("Level3");
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            SceneManager.LoadScene("Level4");
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            SceneManager.LoadScene("Level5");
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            SceneManager.LoadScene("Level6");
        }
    }

    public void Lose()
    {
        if(inProgress)
        {       
            screenShake.Shake();
            Invoke("RestartLevel", levelLoadDelay);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Level1");
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

}


