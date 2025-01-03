using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PickupObjects : MonoBehaviour
{
    public int TrashStored = 0;

    [SerializeField] private TextMeshProUGUI m_trashStoredText;


    void Start()
    {
        m_trashStoredText.text = TrashStored.ToString();
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
