using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float PlayerCurrentSpeed;
    public float PlayerRunSpeed = 1000.0f;
    public float PlayerSpeedBoost = 500.0f;
    public float PlayerSpeedDecay = 250.0f;
    
    private Text KeyToPress;
    private KeyCode KeyCodeToPress;

    public float MouseSensitivity = 1.0f;

    [SerializeField] private Camera m_playerCamera;
    [SerializeField] private Transform m_cameraOffset;

    private Rigidbody m_playerRB;


    void Start()
    {
        m_playerRB = GetComponent<Rigidbody>();
        PlayerCurrentSpeed = PlayerRunSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        moveCamera();
    }

    void FixedUpdate()
    {
        movePlayer();
    }

    // FUNCTIONS //
    void moveCamera()
    {
        m_playerCamera.transform.position = m_cameraOffset.position;
        Quaternion camRot = Quaternion.Lerp(
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
        Vector3 moveDirection =
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
}
