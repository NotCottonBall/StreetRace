using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_pauseMenuPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_pauseMenuPanel.SetActive(false);       
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0.0f;
            m_pauseMenuPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // FUNCTONS //
    public void Resume()
    {
        Time.timeScale = 1.0f;
        m_pauseMenuPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
