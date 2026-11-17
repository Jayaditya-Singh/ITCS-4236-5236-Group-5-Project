using UnityEditor;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance;

    public bool hasLetter { get; private set; }

    [SerializeField] private GameObject[] targetHighlights;

    private int currentDelivery = 0;

    public int CurrentDelivery
    {
        get { return currentDelivery; }
    }
    private void Awake()
    {
        Instance = this;

        foreach (GameObject highlight in targetHighlights)
        {
            highlight.SetActive(false);
        }


    }

    public void PickUpLetter()
    {
        Debug.Log("ENTERED PickUpLetter");

        hasLetter = true;
        targetHighlights[currentDelivery].SetActive(true);

        Debug.Log("Picked up letter");
    }

    public void DeliverLetter()
    {
        Debug.Log("ENTERED DeliverLetter");

        hasLetter = false;
        targetHighlights[currentDelivery].SetActive(false);

        currentDelivery++;
        Debug.Log("Delivered letter");
    }
}
