using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RacePosition : MonoBehaviour
{
    public List<Transform> Racers;
    public List<Transform> Checkpoints;
    private Dictionary<Transform, int> m_racerCheckpointIndex =
        new Dictionary<Transform, int>();
    private Dictionary<Transform, float> m_racerProgress =
        new Dictionary<Transform, float>();

    [SerializeField] private TextMeshProUGUI m_positionText;

    void Start()
    {
        foreach (Transform racer in Racers)
        {
            m_racerCheckpointIndex[racer] = 0;
            m_racerProgress[racer] = 0f;
        }
    }

    void Update()
    {
        CalculateProgress();

        var sortedRacers = Racers.OrderByDescending(
            racer => m_racerProgress[racer]
        ).ToList();

        int playerPosition = sortedRacers.IndexOf(Racers[0]) + 1;
        m_positionText.text = $"{playerPosition}/{Racers.Count}";
    }

    void CalculateProgress()
    {
        foreach (Transform racer in Racers)
        {
            int checkpointIndex = m_racerCheckpointIndex[racer];
            Transform currentCheckpoint = Checkpoints[checkpointIndex];
            Transform nextCheckpoint =
                Checkpoints[(checkpointIndex + 1) % Checkpoints.Count];

            float distanceToNext =
                Vector3.Distance(racer.position, nextCheckpoint.position);
            float checkpointDistance =
                Vector3.Distance(
                    currentCheckpoint.position, nextCheckpoint.position
                );

            float progressBetweenCheckpoints =
                1 - (distanceToNext / checkpointDistance);

            m_racerProgress[racer] =
                checkpointIndex + progressBetweenCheckpoints;
        }
    }

    public void UpdateCheckpoint(Transform racer)
    {
        int nextCheckpoint =
            (m_racerCheckpointIndex[racer] + 1) % Checkpoints.Count;
        m_racerCheckpointIndex[racer] = nextCheckpoint;
    }
}
