using UnityEngine;
using ZilyanusLib.Audio;

public class CollectSound : MonoBehaviour
{
    [SerializeField] private Collectables _collectableComponent;
    [SerializeField] private AudioClip _collectSound;
    private void OnEnable()
    {
        _collectableComponent.OnCollect += PlayCollectSound;
    }
    private void OnDisable()
    {
        _collectableComponent.OnCollect -= PlayCollectSound;
    }
    private void PlayCollectSound()
    {
        AudioClass.PlayAudio(_collectSound, .45f, "General", "Sound", 1, .2f);
    }
}