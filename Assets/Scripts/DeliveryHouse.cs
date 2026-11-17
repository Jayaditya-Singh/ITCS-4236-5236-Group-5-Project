using UnityEngine;

public class DeliveryHouse : MonoBehaviour
{
    [SerializeField] private int houseNumber;

    private bool playerNearby;

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (DeliveryManager.Instance.hasLetter &&
                DeliveryManager.Instance.CurrentDelivery == houseNumber)
            {
                DeliveryManager.Instance.DeliverLetter();
                Debug.Log("Correct House.");
            }
            else
            {
                Debug.Log("Wrong House");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Player is near the delivery house. Press 'E' to deliver the letter.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            Debug.Log("Player left the delivery house area.");
        }
    }
}
