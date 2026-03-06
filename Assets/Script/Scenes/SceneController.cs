using UnityEngine;
using UnityEngine.SceneManagement;


// Gères les changement de scéne depuis les différends menus
public class SceneController : MonoBehaviour
{
    public void Restart()
    {
        StopAudio();

        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainScene");
    }

    public void ToStartingScene()
    {
        StopAudio();

        Time.timeScale = 1.0f;
        SceneManager.LoadScene("StartScene");
    }

    public void Quit()
    {
       Application.Quit();
    }

    public void Unfreeze()
    {
        
        GameManager.Instance.IsPaused = false;
        Time.timeScale = 1f;
        GameManager.Instance.pauseMenuUI.gameObject.SetActive(false);
        AudioListener.pause = false;
    }

    // Stop tout les audio avant de changer de scéne
    private void StopAudio()
    {
        AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.Stop();
        }
    }

}
