using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    private bool isMuted;

    [Space]
    [SerializeField] private AudioMixer m_audioMixer;
    [SerializeField] private Sprite m_muteIcon;
    [SerializeField] private Sprite m_unMuteIcon;
    [SerializeField] private Image m_muteButton;


    public void QuitGame()
    { Application.Quit(); }

    public void StartGame()
    { SceneManager.LoadScene("Main"); }

    public void ToggleMute()
    {
        isMuted = !isMuted;

        if(isMuted)
        {
            m_audioMixer.SetFloat("MasterVolume", 0.0f);
            m_muteButton.sprite = m_unMuteIcon;
        }
        else
        {
            m_audioMixer.SetFloat("MasterVolume", -80.0f);
            m_muteButton.sprite = m_muteIcon;
        }
    }
}
