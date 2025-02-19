using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicLooper: MonoBehaviour
{
    [SerializeField] GameObject NewMusic;
    [SerializeField] List<MusicLooper> NewMusics;
    [SerializeField] float Volume;
    [SerializeField] float Speed;
    private void Awake()
    {
        NewMusic = FindFirstObjectByType<MusicLooper>().gameObject;
        if (NewMusic != null && NewMusic != gameObject)
        {
            if (NewMusic.GetComponent<AudioSource>().clip != GetComponent<AudioSource>().clip)
            {
                NewMusic.GetComponent<MusicLooper>().CloseMusic(true);
            }
            else
            {
                GetComponent<MusicLooper>().CloseMusic(false);
            }
        }
        DontDestroyOnLoad(gameObject);
        DOVirtual.Float(0, Volume, Speed, (x) =>
        {
            GetComponent<AudioSource>().volume = x;
        });
    }

#if UNITY_EDITOR
    [MenuItem("GameObject/Audio/Music Looper")]
    static void CreateMusicObject()
    {
        GameObject MusicObject = new GameObject();
        MusicObject.name = "Music Looper";
        MusicObject.AddComponent<MusicLooper>();
        MusicObject.GetComponent<AudioSource>().loop = true;
    }
    #endif

    public void CloseMusic(bool ease)
    {
        if (ease)
        {
            DOVirtual.Float(Volume, 0, Speed, (x) =>
            {
                GetComponent<AudioSource>().volume = x;
            }).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
