using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance;

    public AudioClip DeliveringLetterSound; //Sound effect for delivering a letter

    public bool hasLetter { get; private set; }

    [SerializeField] private GameObject[] targetHighlights;
    [SerializeField] private GameObject winText;

    [SerializeField] private PathVisualizer pathVisualizer;
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] houseTargets;

    private int currentDelivery = 0;
    private bool finished = false;

    private List<int> deliveryOrder = new List<int>();
    private int deliveriesThisGame;

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

        // Create house order
        for (int i = 0; i < targetHighlights.Length; i++)
        {
            deliveryOrder.Add(i);
        }

        // Shuffle
        for (int i = 0; i < deliveryOrder.Count; i++)
        {
            int randomIndex = Random.Range(i, deliveryOrder.Count);

            int temp = deliveryOrder[i];
            deliveryOrder[i] = deliveryOrder[randomIndex];
            deliveryOrder[randomIndex] = temp;
        }

        // Random number of deliveries
        deliveriesThisGame =
            Random.Range(3, targetHighlights.Length + 1);

        Debug.Log("Deliveries this game: " + deliveriesThisGame);
    }

    public void PickUpLetter()
    {
        if (finished) return;

        hasLetter = true;

        int targetIndex = deliveryOrder[currentDelivery];

        targetHighlights[targetIndex].SetActive(true);

        pathVisualizer.ShowPath(
            player.position,
            houseTargets[targetIndex].position
        );
    }

    public void DeliverLetter()
    {
        hasLetter = false;

        if (DeliveringLetterSound != null)
        {
            AudioSource.PlayClipAtPoint(DeliveringLetterSound, Camera.main.transform.position);
        }

        int targetIndex = deliveryOrder[currentDelivery];

        targetHighlights[targetIndex].SetActive(false);

        pathVisualizer.ClearPath();

        currentDelivery++;

        if (currentDelivery >= deliveriesThisGame)
        {
            finished = true;
            Invoke(nameof(LoadWinScene), 1f); // wait 1 second so the sound can play
        }
    }

    private void LoadWinScene()
    {
        SceneManager.LoadScene("WinCredits");
    }

    public bool IsCorrectHouse(int houseNumber)
    {
        return houseNumber == deliveryOrder[currentDelivery];
    }
}