using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class OpponentAI : MonoBehaviour
{
    [SerializeField] private Animator m_animator;

    [Space]
    public float OpponentSpeed = 10.0f;
    private float OpponentMaxSpeed;
    public float OpponentLookSpeed = 100.0f;
    public float OpponentMinSpeed = 2.0f;

    [Space]
    [SerializeField] UnityEngine.UI.Slider HealthBarSlider;
    public int OpponentHealth = 100;
    private int OpponentMaxHealth;
    public float HealthIncreaseAfter = 2.5f;
    public int HealthDecreaseAmount = 30;
    private bool increasingHealth = false;
    
    [Space]
    [SerializeField] private Transform[] m_checkpoints;
    private int m_currentCheckpointIndex = 0;
    public int PositionScore;
    public RacePosition m_raceManager;

    [Space]
    private Vector3 m_targetPosition;
    [SerializeField] private float m_swayAmplitude = 3.0f;


    void Start()
    {
        SetNewCheckpoint();
        OpponentMaxSpeed = OpponentSpeed;
        OpponentMaxHealth = OpponentHealth;
        SetHealthBar();
    }

    void Update()
    {
        MoveTowardsPosition();

        if(OpponentHealth <= 0)
        {
            OpponentSpeed = OpponentMinSpeed;
            if(!increasingHealth)
            { StartCoroutine(IncreaseHealthAfter(HealthIncreaseAfter)); }
        }
        else
        {
            OpponentSpeed = OpponentMaxSpeed;
            OpponentSpeed = Mathf.Min(OpponentSpeed, OpponentMaxSpeed);
        }

        m_animator.SetBool("isRunning", (OpponentSpeed >= 0.0f));
        m_animator.SetFloat(
            "SpeedMultiplier", (OpponentSpeed / OpponentMaxSpeed) - 2
        );
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Checkpoint"))
        { PositionScore++; }
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
            PositionScore++;
            if(m_currentCheckpointIndex >= m_checkpoints.Length)
            { m_currentCheckpointIndex = 0; }
            SetNewCheckpoint();
        }
    }

    IEnumerator IncreaseHealthAfter(float time)
    {
        increasingHealth = true;
        yield return new WaitForSeconds(time);
        OpponentHealth += OpponentMaxHealth;
        SetHealthBar();
        increasingHealth = false;
    }

    void SetHealthBar()
    {
        HealthBarSlider.gameObject.SetActive(
            OpponentHealth < OpponentMaxHealth
        );

        HealthBarSlider.value =
            (float)OpponentHealth / (float)OpponentMaxHealth;
    }


    // SET GET FUNCTIONS //
    public void TakeDamage()
    {
        OpponentHealth -= HealthDecreaseAmount;
        SetHealthBar();
        OpponentHealth = Mathf.Max(
            OpponentHealth, 0
        );
    }

    public int GetCurrentCheckpoint()
    { return m_currentCheckpointIndex; }
}
