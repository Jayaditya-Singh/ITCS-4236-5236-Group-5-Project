using UnityEngine;

public class PostOffice : MonoBehaviour
{
    public AudioClip RecivingLetterSound; //Sound effect for receiving a letter

    private bool playerNearby;

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {

            if (!DeliveryManager.Instance.hasLetter)
            {
                DeliveryManager.Instance.PickUpLetter();

                AudioSource.PlayClipAtPoint(RecivingLetterSound, Camera.main.transform.position);
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