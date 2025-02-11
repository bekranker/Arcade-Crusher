using UnityEngine;
using UnityEngine.SceneManagement;

public class MapHandler : MonoBehaviour
{
    public void ArcadeSaloon()
    {
        PlayerPrefs.SetString("LastSeenAt", "ArcadeSaloon");
        SceneManager.LoadScene("ArcadeSaloon");
    }
    public void Work()
    {
        PlayerPrefs.SetString("LastSeenAt", "Work");
        SceneManager.LoadScene("Work");
    }
    public void Home()
    {
        PlayerPrefs.SetString("LastSeenAt", "Home");
        SceneManager.LoadScene("Home");
    }
    public void LoadingScreen()
    {

    }
}