using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("General Config")]
    [Space(1)]
    [SerializeField] private float _masterVolume = 1f;
    [SerializeField] private AudioMixerGroup _musicGroup;
    [SerializeField] private AudioMixerGroup _sfxGroup;

    [Header("Sound SOs")]
    [Space(1)]

    [Header("SFX")]
    [Space(1)]
    [SerializeField] private SoundSO _shootSound;
    [SerializeField] private SoundSO _jumpSound;
    [SerializeField] private SoundSO _splatterSound;
    [SerializeField] private SoundSO _jetpackSound;
    [SerializeField] private SoundSO _grenadeBeepSound;
    [SerializeField] private SoundSO _grenadeExplosionSound;
    [SerializeField] private SoundSO _grenadeThrowSound;
    [SerializeField] private SoundSO _playerHitSound;
    [SerializeField] private SoundSO _megaKillSound;
    

    [Header("Music")]
    [Space(1)]
    [SerializeField] private SoundSO _fightMusic;
    [SerializeField] private SoundSO _discoMusic;

    private AudioSource _currentMusicAS;
    private DiscoBallManager _discoBallManager;

    #region Unity Methods

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        _discoBallManager = FindObjectOfType<DiscoBallManager>();
    }

    private void Start()
    {
        PlayFightMusic();
    }

    private void OnEnable()
    {
        Gun.OnShoot += Gun_OnShoot;
        PlayerController.OnJump += PlayerController_OnJump;
        Health.OnDeathSFX += Health_OnDeath;
        DiscoBallManager.OnDiscoBallHit += DiscoBallManager_OnDiscoBallHit;
        PlayerController.OnJetpack += PlayerController_OnJetpack;
        GrenadeThrower.OnThrow += GrenadeThrower_OnThrow;
        Enemy.OnPlayerHit += Enemy_OnPlayerHit;
        MegaKill.OnMegaKill += MegaKill_OnMegaKill;
    }

    private void OnDisable()
    {
        Gun.OnShoot -= Gun_OnShoot;
        PlayerController.OnJump -= PlayerController_OnJump;
        Health.OnDeathSFX -= Health_OnDeath;
        DiscoBallManager.OnDiscoBallHit -= DiscoBallManager_OnDiscoBallHit;
        PlayerController.OnJetpack -= PlayerController_OnJetpack;
        GrenadeThrower.OnThrow -= GrenadeThrower_OnThrow;
        Enemy.OnPlayerHit -= Enemy_OnPlayerHit;
        MegaKill.OnMegaKill -= MegaKill_OnMegaKill;
    }

    #endregion

    #region Sound Methods

    private void PlaySound(SoundSO soundSO)
    {
        //Create game object for audio
        GameObject newGameObject = new GameObject();
        newGameObject.name = soundSO.Clip.name;

        AudioSource audioSource = newGameObject.AddComponent<AudioSource>();

        SetAudioConfigs(soundSO, ref audioSource);
        audioSource.Play();

        //Destory audio after it is ended, if it is not looped
        if (!soundSO.Loop)
        {
            Destroy(newGameObject, audioSource.clip.length);
        }
    }

    private void SetAudioConfigs(SoundSO soundSO, ref AudioSource audioSource)
    {
        audioSource.volume = soundSO.Volume * _masterVolume;
        audioSource.loop = soundSO.Loop;
        audioSource.clip = soundSO.Clip;
        audioSource.pitch = soundSO.Pitch;

        if (soundSO.RandomizePitch)
        {
            audioSource.pitch = RandomizePitch(soundSO);
        }

        AudioMixerGroup audioMixerGroup = DetermineAudioMixerGroup(soundSO.AudioType);
        audioSource.outputAudioMixerGroup = audioMixerGroup;

        if (audioMixerGroup == _musicGroup)
        {
            if (_currentMusicAS != null)
            {
                _currentMusicAS.Stop();
                Destroy(_currentMusicAS.gameObject);
            }
            _currentMusicAS = audioSource;
        }
    }

    private AudioMixerGroup DetermineAudioMixerGroup(SoundSO.AudioTypes audioType) => audioType switch
    {
        SoundSO.AudioTypes.SFX => _sfxGroup,
        SoundSO.AudioTypes.Music => _musicGroup,
        _ => throw new System.NotImplementedException(),
    };

    private float RandomizePitch(SoundSO soundSO)
    {
        float randomPitch = Random.Range(
                -soundSO.RandomPitchRangeModifier,
                soundSO.RandomPitchRangeModifier);

        return soundSO.Pitch + randomPitch;
    }

    #endregion

    #region SFX

    private void Gun_OnShoot()
    {
        PlaySound(_shootSound);
    }

    private void PlayerController_OnJump()
    {
        PlaySound(_jumpSound);
    }

    private void Health_OnDeath()
    {
        PlaySound(_splatterSound);
    }

    private void PlayerController_OnJetpack()
    {
        PlaySound(_jetpackSound);
    }

    public void Grenade_OnToggleGrenadeLight()
    {
        PlaySound(_grenadeBeepSound);
    }

    public void Grenade_OnExplosion()
    {
        PlaySound(_grenadeExplosionSound);
    }

    private void GrenadeThrower_OnThrow()
    {
        PlaySound(_grenadeThrowSound);
    }

    private void Enemy_OnPlayerHit(Enemy sender)
    {
        PlaySound(_playerHitSound);
    }

    private void MegaKill_OnMegaKill()
    {
        PlaySound(_megaKillSound);
    }

    #endregion

    #region Music

    private void DiscoBallManager_OnDiscoBallHit()
    {
        PlaySound(_discoMusic);
        Utils.RunAfterDelay(this, PlayFightMusic, _discoBallManager.DiscoBallPartyTime);
    }

    private void PlayFightMusic()
    {
        PlaySound(_fightMusic);
    }

    #endregion
}
