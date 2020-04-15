using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameMusic : MonoBehaviour
{
    [SerializeField] private AudioClip intro = default;
    [SerializeField] private AudioClip loop = default;
    [SerializeField] private float introLoopDelay = 0.35F;

    private AudioSource source;
    private AudioSource loopSource;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        loopSource = gameObject.AddComponent<AudioSource>();
        loopSource.volume = source.volume;
        loopSource.clip = loop;
        loopSource.loop = true;
        source.clip = intro;
        source.loop = false;
    }

    private IEnumerator Start()
    {
        source.Play();
        yield return new WaitForSeconds(source.clip.length - introLoopDelay);
        loopSource.Play();
    }

    private void Update()
    {
        source.pitch = Time.timeScale;
        loopSource.pitch = Time.timeScale;
    }
}
