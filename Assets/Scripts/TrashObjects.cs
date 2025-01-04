using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Analytics;

public class TrashObjects : MonoBehaviour
{
    public int TrashStored = 0;
    public float ThrowForce = 6000.0f;
    public GameObject trashObjectPrefab;

    [SerializeField] private Transform m_spawnPoint;
    public Transform TrashThrowPoint;

    [SerializeField] private TextMeshProUGUI m_trashStoredText;


    void Start()
    {
        m_trashStoredText.text = TrashStored.ToString();
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if(TrashStored > 0)
            {
                GameObject trashProjectile = Instantiate(
                    trashObjectPrefab, m_spawnPoint.position, Quaternion.identity
                );
                trashProjectile.AddComponent<Rigidbody>();
                trashProjectile.tag = "Untagged";
                
                Vector3 direction = 
                    (TrashThrowPoint.position - m_spawnPoint.position).normalized;
                
                Rigidbody rb = trashProjectile.GetComponent<Rigidbody>();
                if(rb == null)
                { Debug.LogError("No Rigidbody Found On GameObject!"); }

                rb.AddForce(
                    direction * ThrowForce * Time.deltaTime,
                    ForceMode.Impulse
                );

                TrashStored--;
                m_trashStoredText.text = TrashStored.ToString();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Trash"))
        {
            TrashStored++;
            m_trashStoredText.text = TrashStored.ToString();
            Destroy(other.gameObject);
        }
    }
}
