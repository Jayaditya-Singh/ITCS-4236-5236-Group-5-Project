using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance;

    public bool hasLetter { get; private set; }

    [SerializeField] private GameObject targetHighlight;

    private void Awake()
    {
        Instance = this;

        if (targetHighlight != null)
            targetHighlight.SetActive(false);


    }

    public void PickUpLetter()
    {
        Debug.Log("ENTERED PickUpLetter");

        hasLetter = true;
        if (targetHighlight != null)
            targetHighlight.SetActive(true);

        Debug.Log("Picked up letter");
    }

    public void DeliverLetter()
    {
        Debug.Log("ENTERED DeliverLetter");

        hasLetter = false;
        if (targetHighlight != null)
            targetHighlight.SetActive(false);
        Debug.Log("Delivered letter");
    }
}
