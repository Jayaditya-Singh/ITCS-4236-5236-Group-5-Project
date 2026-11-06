using UnityEngine;

public class DeliveryHouse : MonoBehaviour
{
    private bool playerNearby;

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("HOUSE E PRESSED");

            if (DeliveryManager.Instance.hasLetter)
            {
                DeliveryManager.Instance.DeliverLetter();
                Debug.Log("Letter delivered to delivery house.");
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
