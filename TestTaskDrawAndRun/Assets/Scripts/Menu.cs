using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;

   public void PauseOn()
    {
        Time.timeScale = 0;
        _pauseMenu.SetActive(true);
    }
    public void PauseOff()
    {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
