using UnityEngine;

public class TwinsToTheMoonGround : MonoBehaviour
{
    [SerializeField] private LoseScreen _loseScreen;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _loseScreen.LoseGame();
        }
    }
}