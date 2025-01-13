using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RacePosition : MonoBehaviour
{
    public List<GameObject> Racers;

    [Space]
    [SerializeField] private TextMeshProUGUI m_playerPositionUI;

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
        m_playerPositionUI.text = $"{playerPosition.ToString()} / {Racers.Count}";
    }
}