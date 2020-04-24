using System.Collections;
using UnityEngine;
using CommandTerminal;

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
        InitializeSources();
        RegisterCommands();
    }

    private void InitializeSources()
    {
        loopSource.volume = source.volume;
        loopSource.clip = loop;
        loopSource.loop = true;
        source.clip = intro;
        source.loop = false;
    }

    private void RegisterCommands()
    {
        Terminal.Shell.AddCommand("music_pause", CmdPause, 0, 0, "Pauses the game music.");
        Terminal.Shell.AddCommand("music_resume", CmdResume, 0, 0, "Resumes the game music.");
        Terminal.Shell.AddCommand("music_setvolume", CmdSetVolume, 1, 1, "Set music volume. Args: Volume (Float).");
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

    private void CmdPause(CommandArg[] args)
    {
        if (Terminal.IssuedError) return;
        loopSource.Pause();
    }

    private void CmdResume(CommandArg[] args)
    {
        if (Terminal.IssuedError) return;
        loopSource.UnPause();
    }

    private void CmdSetVolume(CommandArg[] args)
    {
        if (Terminal.IssuedError) return;
        float volume = args[0].Float;
        loopSource.volume = volume;
    }
}
