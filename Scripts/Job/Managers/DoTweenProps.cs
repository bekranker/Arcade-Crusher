using UnityEngine;

public class DoTweenProps : MonoBehaviour
{
    public static DoTweenProps Instance { get; private set; }

    [Header("Player UI")]
    public Vector3 PunchScale_PlayerUI = new Vector3(1.2f, 1.2f, 1.2f);
    public float Delay_PlayerUI = 0.5f;

    [Header("Slot Effect Props")]
    public Vector3 PunchScale_Slot = new Vector3(.5f, .5f, .5f);
    public float Delay_SlotDelay = 0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}