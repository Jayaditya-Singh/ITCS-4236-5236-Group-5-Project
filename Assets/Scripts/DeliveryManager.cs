using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance;

    public bool hasLetter { get; private set; }

    [SerializeField] private GameObject[] targetHighlights;

    private int currentDelivery = 0;

    [SerializeField] private GameObject winText;

    [SerializeField] private PathVisualizer pathVisualizer;

    [SerializeField] private Transform player;

    [SerializeField] private Transform[] houseTargets;

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

        hasLetter = true;
        targetHighlights[currentDelivery].SetActive(true);

        pathVisualizer.ShowPath(player.position, houseTargets[currentDelivery].position);

    }

    public void DeliverLetter()
    {

        hasLetter = false;
        targetHighlights[currentDelivery].SetActive(false);

        pathVisualizer.ClearPath();
        currentDelivery++;
        if (currentDelivery >= targetHighlights.Length)
        {

            if (winText != null)
            {
                SceneManager.LoadScene("WinCredits");
            }
        }
    }
}
