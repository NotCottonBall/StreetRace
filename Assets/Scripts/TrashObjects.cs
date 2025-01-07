using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Analytics;

public class TrashObjects : MonoBehaviour
{
    public int TrashStored = 0;
    public float ThrowForce = 8000.0f;
    public GameObject trashObjectPrefab;

    [Space]
    [SerializeField] private Transform m_spawnPoint;

    [Space]
    [SerializeField] private TextMeshProUGUI m_trashStoredText;


    void Start()
    {
        m_trashStoredText.text = TrashStored.ToString();
    }

    void Update()
    {
        ThrowTrash();
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


    // FUNCTIONS //
    void ThrowTrash()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if(TrashStored > 0)
            {
                GameObject trashProjectile = Instantiate(
                    trashObjectPrefab, m_spawnPoint.position,
                    Quaternion.identity
                );
                trashProjectile.AddComponent<Rigidbody>();
                trashProjectile.tag = "Projectile";
                trashProjectile.GetComponent<Collider>().isTrigger = false;
                
                Vector3 direction = (transform.forward).normalized;
                
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
}
