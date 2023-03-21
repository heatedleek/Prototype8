using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [Header("Particle Effects")]
    [SerializeField] ParticleSystem winParticles;

    [Header("Misc")]
    [SerializeField] float levelDelay = 3.0f;

    private bool haveWon = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if(player.isActiveAndEnabled)
            {
                player.Win();
                Win();
            }
        }
    }

    private void Win()
    {
        if(!haveWon)
        {
            haveWon = true;
            winParticles.Play();
            AudioManager.Play(AudioClipName.Goal);
            Invoke("LoadNextLevel", levelDelay);
        }
    }

    void LoadNextLevel()
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
