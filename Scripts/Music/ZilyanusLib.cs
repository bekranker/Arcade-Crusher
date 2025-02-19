using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ZilyanusLib
{
    namespace Audio
    {
        using UnityEngine.Audio;

        public static class AudioClass
        {
            public static AudioClip FindClip(string ClipName, string AudioMixerName = "General", string AudioMixerGroupName = "Sound")
            {
                AudioClip audioClip = Resources.Load("Audio/" + ClipName) as AudioClip;
                if (audioClip == null) { Debug.LogWarning(ClipName + " Not Found"); return null; }
                return audioClip;
            }

            public static void PlayAudio(string ClipName, float volume = 1f, string AudioMixerName = "General", string AudioMixerGroupName = "Sound", float Pitch = 1)
            {
                if (ClipName == "") return;
                AudioClip audioClip = Resources.Load("Audio/" + ClipName) as AudioClip;
                if (audioClip == null) { Debug.LogWarning(ClipName + " Not Found"); return; }
                PlayAudio(audioClip, volume, AudioMixerName, AudioMixerGroupName, Pitch);
            }

            public static void PlayAudio(AudioClip clip, float volume = 1f, string AudioMixerName = "General", string AudioMixerGroupName = "Sound", float Pitch = 1)
            {
                if (clip == null) return;
                // iOS can be problem 
                AudioMixer audioMixer = Resources.Load("Audio/" + AudioMixerName) as AudioMixer;
                AudioMixerGroup group = audioMixer.FindMatchingGroups(AudioMixerGroupName)[0];

                PlayAudio(clip, group, volume, Pitch);
            }

            public static void PlayAudio(AudioClip clip, AudioMixerGroup group, float volume = 1f, float Pitch = 1)
            {
                if (clip == null) return;
                GameObject gameObject = new GameObject("One shot audio");
                gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform);
                gameObject.transform.position = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
                AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
                if (group != null)
                    audioSource.outputAudioMixerGroup = group;
                audioSource.clip = clip;
                audioSource.spatialBlend = 1f;
                audioSource.volume = volume;
                audioSource.pitch = Random.Range(Pitch - 0.05f, Pitch + 0.05f);
                audioSource.Play();
                Object.Destroy(gameObject, clip.length);
            }

            public static void PlayAudio(SoundData soundData, string AudioMixerName = "General", string AudioMixerGroupName = "Sound", float Pitch = 1)
            {
                PlayAudio(soundData.SoundName, soundData.Volume, AudioMixerName, AudioMixerGroupName, Pitch);
            }
        }
    }

    namespace Spawner
    {
#if UNITY_EDITOR
        public class SpawnerClass : MonoBehaviour
        {
            public static GameObject SpawnPrefab(GameObject Prefab, Transform Parent = null)
            {

                //Does not work runtime only editor
                GameObject ParentGameObject = new GameObject();
                PrefabUtility.InstantiatePrefab(Prefab, ParentGameObject.transform);
                GameObject SpawnedPrefab = ParentGameObject.transform.GetChild(0).gameObject;
                SpawnedPrefab.transform.SetParent(Parent);
                DestroyImmediate(ParentGameObject);
                return SpawnedPrefab;

            }
        }
#endif
    }
}

