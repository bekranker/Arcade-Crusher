using UnityEngine;
using UnityEngine.SceneManagement;

public class MapHandler : MonoBehaviour
{
    [SerializeField] private ChargeHandler _chargeHandler;
    public void ArcadeSaloon()
    {
        MoveAction("Saloon");
    }
    public void Work()
    {
        MoveAction("Work");
    }
    public void Home()
    {
        MoveAction("Home");
    }

    private void MoveAction(string key)
    {
        if (!_chargeHandler.EnoughCharge())
        {
            Debug.Log("Enerji bitmiş");
            return;
        }
        PlayerPrefs.SetString("LastSeenAt", key);
        SceneManager.LoadScene(key);
    }
}