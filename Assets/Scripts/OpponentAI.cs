using UnityEngine;

public class OpponentAI : MonoBehaviour
{
    public float OpponentSpeed = 1000.0f;
    public float OpponentLookSpeed = 100.0f;
    
    [Space]
    [SerializeField] private Transform[] m_checkpoints;
    private int m_currentCheckpointIndex = 0;

    [Space]
    private Vector3 m_targetPosition;
    [SerializeField] private float m_swayAmplitude = 3.0f;


    void Start()
    {
        SetNewCheckpoint();
    }

    void Update()
    {
        MoveTowardsPosition();
    }


    // Fucntions //
    void SetNewCheckpoint()
    {
        Transform checkpoint = m_checkpoints[m_currentCheckpointIndex];

        float swayOffset = Random.Range(-m_swayAmplitude, m_swayAmplitude);
        m_targetPosition =
            checkpoint.position + (checkpoint.right * swayOffset);
    }

    void MoveTowardsPosition()
    {
        transform.position = Vector3.MoveTowards(
            transform.position, m_targetPosition,
            OpponentSpeed * Time.deltaTime
        );

        Vector3 direction = (m_targetPosition - transform.position).normalized;
        Quaternion lookAt = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation, lookAt, OpponentLookSpeed * Time.deltaTime
        );

        if(Vector3.Distance(transform.position, m_targetPosition) < 1f)
        {
            m_currentCheckpointIndex++;
            if(m_currentCheckpointIndex >= m_checkpoints.Length)
            { m_currentCheckpointIndex = 0; }
            SetNewCheckpoint();
        }
    }
}
