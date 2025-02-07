using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;

public class RacePosition : MonoBehaviour
{
    public List<GameObject> Racers;
    [Description("no of checkpoints per lap.")]
    [SerializeField] private int m_oneLap = 66;
    [SerializeField] private int m_winLap = 2;

    [Space]
    [SerializeField] private TextMeshProUGUI m_playerPositionUI;
    [SerializeField] private TextMeshProUGUI m_playerLapText;
    [SerializeField] private GameObject m_winPanel;
    [SerializeField] private TextMeshProUGUI m_winText;

    void Start()
    {
        m_winPanel.SetActive(false);
    }

    void Update()
    {
        Racers = Racers
            .OrderByDescending(racer =>
            {
                var Player = racer.GetComponent<PlayerController>();
                var Opponent = racer.GetComponent<OpponentAI>();

                return (Player != null) ? Player.PlayerPositionScore
                        : (Opponent != null) ? Opponent.PositionScore
                        : 0;
            }).ToList();

        int playerPosition =
            Racers.IndexOf(gameObject) + 1;
        m_playerPositionUI.text =
            $"{playerPosition.ToString()} / {Racers.Count}";

        m_playerLapText.text = "LAP: " +
            (gameObject.GetComponent<PlayerController>()
            .PlayerPositionScore / m_oneLap).ToString();

        foreach (GameObject racer in Racers)
        {
            if (racer.GetComponent<OpponentAI>() != null)
            {
                if (racer.GetComponent<OpponentAI>()
                    .PositionScore > (m_oneLap * m_winLap)
                )
                {
                    m_winPanel.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    m_winText.text = "THE OPPONENT WON!\n YOU LOOSER!";
                    Time.timeScale = 0.0f;
                }
            }
            else if (racer.GetComponent<PlayerController>() != null)
            {
                if (racer.GetComponent<PlayerController>()
                    .PlayerPositionScore > (m_oneLap * m_winLap)
                )
                {
                    m_winPanel.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    m_winText.text = "DAMN! YOU WON!!!";
                    Time.timeScale = 0.0f;
                }
            }
        }
    }
}