using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnShoot = default;
    [SerializeField]
    private Bullet bullet = default;
    [SerializeField]
    private Transform spawnPoint = default;
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera = default;
    [SerializeField]
    private AudioClip shootSound = default;

    private AudioSource source;
    private CinemachineBasicMultiChannelPerlin cameraNoise;

    private void Awake()
    {
        cameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (source == null)
            source = GetComponent<AudioSource>();

        if (source == null)
            source = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (OnShoot!= null)
                OnShoot.Invoke();

            Bullet instancedBullet = Instantiate(bullet, spawnPoint.position, Quaternion.identity);
            instancedBullet.Shoot(transform.localScale.x);
            cameraNoise.m_FrequencyGain = 50;
            Invoke(nameof(DisableNoise), 0.1F);
            source.clip = shootSound;
            source.pitch = Random.Range(0.75F, 1F);
            source.Play();
        }
    }

    private void DisableNoise()
    {
        cameraNoise.m_FrequencyGain = 0;
    }
}
