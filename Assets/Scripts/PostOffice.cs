using UnityEngine;

public class PostOffice : MonoBehaviour
{
    private bool playerNearby;

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("POST OFFICE E PRESSED");

            if (!DeliveryManager.Instance.hasLetter)
            {
                DeliveryManager.Instance.PickUpLetter();
                Debug.Log("Letter picked up from post office.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered!");
        
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Player entered.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            Debug.Log("Player left the post office area.");
        }
    }
}
