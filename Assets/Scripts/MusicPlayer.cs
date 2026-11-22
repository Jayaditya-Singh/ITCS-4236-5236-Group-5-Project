using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    static MusicPlayer instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // avoid duplicates when returning to the title scene
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        GetComponent<AudioSource>().Play();
    }
}