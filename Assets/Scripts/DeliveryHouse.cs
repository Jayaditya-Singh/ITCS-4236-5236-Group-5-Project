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
                DeliveryManager.Instance.IsCorrectHouse(houseNumber))
            {
                DeliveryManager.Instance.DeliverLetter();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}