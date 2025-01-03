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
    public float PlayerSpeedDecay = 100.0f;
    public float PlayerSpeedDecayTime = 5.0f;
    public float PlayerSpeedPenalty = 300.0f;

    public float MouseSensitivity = 1.0f;

    private string[] m_availableKeys = new string[]
    {
        "R", "T",
        "F", "G",
        "V", "B"
    };
    
    [SerializeField] private TextMeshProUGUI m_keyToPress;
    private Queue<KeyCode> m_keyCodesToPress = new Queue<KeyCode>();

    [SerializeField] private Camera m_playerCamera;
    [SerializeField] private Transform m_cameraOffset;

    private Rigidbody m_playerRB;

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
        // m_keyToPress.text = m_keyCodesToPress.Peek().ToString() + "";
        UpdateKeyToPressText();
        if(Input.anyKeyDown)
        {
            if(Input.GetKeyDown(m_keyCodesToPress.Peek()))
            {
                if(PlayerCurrentSpeed < PlayerMaxSpeed)
                {
                    PlayerCurrentSpeed += PlayerSpeedBoost * Time.deltaTime;
                }
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
        else
        { StartCoroutine(speedDecayTimer(PlayerSpeedDecayTime)); }

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
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        UnityEngine.Vector3 moveDirection =
            transform.right * horizontalMove *
            PlayerCurrentSpeed * Time.deltaTime;
        m_playerRB.AddForce(moveDirection);
        
        if(Input.GetButton("ForwardMove"))
        {
            m_playerRB.AddForce(
                transform.forward * PlayerCurrentSpeed * Time.deltaTime
            );
        }        
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
        m_keyToPress.text = "";
        foreach(KeyCode key in m_keyCodesToPress)
        { m_keyToPress.text += key.ToString() + " "; }
    }

    void applyPenalty()
    {
        float newSpeed = PlayerCurrentSpeed - PlayerSpeedPenalty;
        PlayerCurrentSpeed = Mathf.Lerp(PlayerCurrentSpeed, newSpeed, 2.5f);
        PlayerCurrentSpeed = Mathf.Max(PlayerCurrentSpeed, 500.0f);
    }

    IEnumerator speedDecayTimer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        PlayerCurrentSpeed -= PlayerSpeedDecay * Time.deltaTime;
        PlayerCurrentSpeed = Mathf.Max(PlayerCurrentSpeed, 500.0f);
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
