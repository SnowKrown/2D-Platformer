using System.Collections;
using UnityEngine;
using CommandTerminal;

public class TimeScaler : MonoBehaviour
{
    [SerializeField] private AudioClip slowMotionClip = default;
    [SerializeField] private AudioClip fastMotionClip = default;
    [SerializeField] private AudioClip resetFromSlowClip = default;
    [SerializeField] private AudioClip resetFromFastClip = default;
    [SerializeField] private float defaultResetTime = 0.5F;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Terminal.Shell.AddCommand("timescaler_to", CmdTimeScaler, 2, 4, "Set the timescale. Args: To(Float), Duration(Float), Reset(Bool), Ease:(Int)");
        Terminal.Shell.AddCommand("timescaler_reset", CmdTimeScaler, 0, 0, "Reset the timescale.");
    }

    public void ScaleTo(float to, float duration, bool resetAfterComplete = true, iTween.EaseType ease = iTween.EaseType.easeOutQuad)
    {
        audioSource.clip = GetAudio(to);
        audioSource.Play();
        iTween.Stop(gameObject);
        iTween.ValueTo(gameObject, GetHash(to, duration, resetAfterComplete, ease));
    }

    private Hashtable GetHash(float to, float duration, bool resetAfterComplete, iTween.EaseType ease)
    {
        Hashtable hash = new Hashtable();
        hash.Add("from", Time.timeScale);
        hash.Add("to", to);
        hash.Add("time", duration);
        hash.Add("onupdatetarget", gameObject);
        hash.Add("onupdate", nameof(UpdateTimeScale));
        hash.Add("easetype", ease);
        hash.Add("ignoretimescale", true);

        if (resetAfterComplete)
        {
            hash.Add("oncomplete", nameof(ResetAfterComplete));
            hash.Add("oncompletetarget", gameObject);
        }

        return hash;
    }

    private void ResetAfterComplete()
    {
        ScaleTo(1, defaultResetTime, false);
    }

    private AudioClip GetAudio(float to)
    {
        if (to == 1)
            return Time.timeScale < 1 ? resetFromSlowClip : resetFromFastClip;
        else
            return to < 1 ? slowMotionClip : fastMotionClip;
    }

    private void UpdateTimeScale(float value)
    {
        Time.timeScale = value;
    }

    private void OnApplicationQuit()
    {
        Time.timeScale = 1;
    }

    private void CmdTimeScaler(CommandArg[] args)
    {
        if (Terminal.IssuedError) return;
        float to = args[0].Float;
        float duration = args[1].Float;
        bool reset = args.Length > 2 ? args[2].Bool : true;
        iTween.EaseType ease = args.Length > 3 ? (iTween.EaseType)args[3].Int : iTween.EaseType.easeOutQuad;
        ScaleTo(to, duration, reset, ease);
    }

    private void CmdTimeReset(CommandArg[] args)
    {
        if (Terminal.IssuedError) return;
        ScaleTo(1, defaultResetTime, false);
    }
}
