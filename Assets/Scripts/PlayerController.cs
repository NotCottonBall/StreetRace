using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float PlayerCurrentSpeed;
    public float PlayerRunSpeed = 1000.0f;
    public float PlayerMaxSpeed = 1500.0f;
    public float PlayerSpeedBoost = 20000.0f;

    [Space]
    public float PlayerSpeedDecay = 500.0f;
    public float PlayerSpeedDecayTimeout = 0.1f;
    public float PlayerSpeedPenalty = 300.0f;
    private bool isDecaying = false;

    [Space]
    public float MouseSensitivity = 1.0f;
    [SerializeField] private Camera m_playerCamera;
    [SerializeField] private Transform m_cameraOffset;

    private string[] m_availableKeys = new string[]
    {
        "R", "T",
        "F", "G",
        "V", "B"
    };

    private Queue<KeyCode> m_keyCodesToPress = new Queue<KeyCode>();

    private Rigidbody m_playerRB;

    [Space]
    [SerializeField] private TextMeshProUGUI m_keyToPressText;
    [SerializeField] private TextMeshProUGUI m_speedText;
    [SerializeField] private Slider m_speedSB;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        m_playerRB = GetComponent<Rigidbody>();
        PlayerCurrentSpeed = PlayerRunSpeed;

        for(int i = 0; i < 3; i++)
        { PickRandomKey(); }
        UpdateKeyToPressText();
    }

    // Update is called once per frame
    void Update()
    {
        // m_keyToPressText.text = m_keyCodesToPress.Peek().ToString() + "";
        UpdateKeyToPressText();
        if(Input.anyKeyDown)
        {
            if(Input.GetKeyDown(m_keyCodesToPress.Peek()))
            {
                PlayerCurrentSpeed += PlayerSpeedBoost * Time.deltaTime;
                PlayerCurrentSpeed =
                    Mathf.Min(PlayerCurrentSpeed, PlayerMaxSpeed);
                m_keyCodesToPress.Dequeue();
                PickRandomKey();
                UpdateKeyToPressText();
            }
            else if(Input.GetKeyDown(KeyCode.R) ||
                    Input.GetKeyDown(KeyCode.T) ||
                    Input.GetKeyDown(KeyCode.F) ||
                    Input.GetKeyDown(KeyCode.G) ||
                    Input.GetKeyDown(KeyCode.V) ||
                    Input.GetKeyDown(KeyCode.B)
            )
            { applyPenalty(); }
        }
        else if(!Input.anyKeyDown && !isDecaying)
        { StartCoroutine(speedDecayTimer(PlayerSpeedDecayTimeout)); }

        moveCamera();
        setSpeedUI();
    }

    void FixedUpdate()
    {
        movePlayer();
    }

    // FUNCTIONS //
    void moveCamera()
    {
        m_playerCamera.transform.position = m_cameraOffset.position;
        UnityEngine.Quaternion camRot = UnityEngine.Quaternion.Lerp(
            m_playerCamera.transform.rotation,
            m_cameraOffset.rotation,
            5.0f * Time.deltaTime
        );
        m_playerCamera.transform.rotation = camRot;

        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(0.0f, mouseX * MouseSensitivity, 0.0f);
    }

    void movePlayer()
    {
        float hMove = Input.GetAxisRaw("Horizontal");
        float vMove = Input.GetAxisRaw("Vertical");

        UnityEngine.Vector3 moveDir = new UnityEngine.Vector3(
            hMove, 0.0f, vMove
        ).normalized * PlayerCurrentSpeed * Time.deltaTime;

        m_playerRB.AddForce(
            transform.TransformDirection(moveDir), ForceMode.Force
        );
    }


    // OTHER FUNCTIONS //
    void PickRandomKey()
    {
        int index = UnityEngine.Random.Range(0, m_availableKeys.Length);
        KeyCode key = (KeyCode)System.Enum.Parse(typeof(KeyCode),
            m_availableKeys[index]);
        m_keyCodesToPress.Enqueue(key);
    }

    void UpdateKeyToPressText()
    {
        m_keyToPressText.text = "";
        foreach(KeyCode key in m_keyCodesToPress)
        { m_keyToPressText.text += key.ToString() + " "; }
    }

    void applyPenalty()
    {
        float newSpeed = PlayerCurrentSpeed - PlayerSpeedPenalty;
        PlayerCurrentSpeed = Mathf.Lerp(PlayerCurrentSpeed, newSpeed, 2.5f);
        PlayerCurrentSpeed = Mathf.Max(PlayerCurrentSpeed, 500.0f);
    }

    IEnumerator speedDecayTimer(float waitTime)
    {
        isDecaying = true;
        yield return new WaitForSeconds(waitTime);
        PlayerCurrentSpeed -= PlayerSpeedDecay * Time.deltaTime;
        PlayerCurrentSpeed = Mathf.Max(PlayerCurrentSpeed, 500.0f);
        Debug.Log("Speed Reduced");
        isDecaying = false;
    }


    // UI //
    void setSpeedUI()
    {
        m_speedText.text =
            (PlayerCurrentSpeed / PlayerMaxSpeed * 100).ToString("0");

        m_speedSB.value = Mathf.Lerp(
            m_speedSB.value, PlayerCurrentSpeed / PlayerMaxSpeed, 2.5f
        );
    }
}
