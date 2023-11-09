using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    public static SoundController Instance;

    public enum SoundIds
    {
        Castle, // разрушение замка
        Catapult, // запуск камня
        Coin, // подбор монеты (задача со звездочкой)
        FinalChest, // Открытие финального сундука
        Fort, // разрушение форта
        Jump, // камень прыгает с трамплина или с трассы на трассу
        Landing, // при приземлении камня после трамплина или при первом приземлении камня после большого прыжка с трассы на трассу
        Letters, // разрушение букв(задача со звездочкой)
        Lose, // При появлении надписи Lose на экране проигрыша
        MenuClick, // открытие, закрытие меню, тап по переключателям вибро и звука
        SceneryDestroy, // уничтожение елок или заборов или любых разрушаемых объектов(финишные сундуки) кроме букв, фортов, замка  (задача со звездочкой)
        StoneDestroy, // когда камень полностью сломался.Но не проигрывать на финишной прямой
        Trap, // при контакте с ловушкой с шипами
        UnitPickup, // наезд на врага или овцу (задача со звездочкой)
        Win, // При появлении надписи Win на экране победы
        FinalChestDestroy, // разрушение финальных сундуков
        StoneDestroyWin, // когда камень полностью сломался и победил.
    };

    [Serializable]
    public class SoundItem
    {
        public SoundIds Id;
        public AudioClip Clip;
        [Range(0f, 5f)] public float Volume = 1f;
        public float Cooldown = 0.1f;
        public bool PitchChange = false;
        public float PitchMinDelay = 0.1f;
        public float PitchAdd = 0.1f;

        [HideInInspector] public float nowCooldown = 0f;
        [HideInInspector] public float nowPitch = 1f;
        [HideInInspector] public float nowDelay = 0f;
    }

    [SerializeField] private AudioSource _prefab;
    [SerializeField] private SoundItem[] _sounds;
    private Transform _player;
    private List<AudioSource> _sources = new List<AudioSource>();

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < _sounds.Length; i++)
        {
            if (_sounds[i].PitchChange)
            {
                _sounds[i].nowPitch = 1f;
                _sounds[i].nowDelay = 0f;
            }
            _sounds[i].nowCooldown = 0f;
        }
        _player = FindObjectOfType<PlayerController>().transform;
    }

    private void Update()
    {
        for (int i = 0; i < _sounds.Length; i++)
        {
            if (_sounds[i].nowCooldown > 0f)
            {
                _sounds[i].nowCooldown -= Time.deltaTime;
            }
            if (_sounds[i].PitchChange && _sounds[i].nowDelay > 0f)
            {
                _sounds[i].nowDelay -= Time.deltaTime;
            }
        }
    }

    public void PlaySound(SoundIds id)
    {
        if (!GeneralSettings.Instance.GetMusic())
            return;

        for (int i = 0; i < _sounds.Length; i++)
        {
            if (_sounds[i].Id == id)
            {
                if (_sounds[i].nowCooldown <= 0f)
                {
                    if (_sounds[i].nowDelay <= 0f)
                        _sounds[i].nowPitch = 1f;
                    _sounds[i].nowCooldown = _sounds[i].Cooldown;

                    AudioSource source = GetAudioSource();
                    source.transform.position = _player.position;
                    source.clip = _sounds[i].Clip;
                    source.volume = _sounds[i].Volume;
                    source.pitch = _sounds[i].nowPitch;
                    source.Play();

                    if (_sounds[i].PitchChange)
                    {
                        _sounds[i].nowPitch += _sounds[i].PitchAdd;
                        _sounds[i].nowDelay = _sounds[i].PitchMinDelay;
                    }
                }
                return;
            }
        }
    }

    private AudioSource GetAudioSource()
    {
        if (_sources.Count > 0)
        {
            for (int i = 0; i < _sources.Count; i++)
            {
                if (!_sources[i].isPlaying)
                    return _sources[i];
            }
        }
        _sources.Add(Instantiate(_prefab, transform));
        return _sources[^1];
    }


#if UNITY_EDITOR
    [Button("Play Castle")]
    public void DebugPlay_0()
    {
        PlaySound(SoundIds.Castle);
    }
    [Button("Play Catapult")]
    public void DebugPlay_1()
    {
        PlaySound(SoundIds.Catapult);
    }
    [Button("Play Coin")]
    public void DebugPlay_2()
    {
        PlaySound(SoundIds.Coin);
    }
    [Button("Play FinalChest")]
    public void DebugPlay_3()
    {
        PlaySound(SoundIds.FinalChest);
    }
    [Button("Play Fort")]
    public void DebugPlay_4()
    {
        PlaySound(SoundIds.Fort);
    }
    [Button("Play Jump")]
    public void DebugPlay_5()
    {
        PlaySound(SoundIds.Jump);
    }
    [Button("Play Landing")]
    public void DebugPlay_6()
    {
        PlaySound(SoundIds.Landing);
    }
    [Button("Play Letters")]
    public void DebugPlay_7()
    {
        PlaySound(SoundIds.Letters);
    }
    [Button("Play Lose")]
    public void DebugPlay_8()
    {
        PlaySound(SoundIds.Lose);
    }
    [Button("Play MenuClick")]
    public void DebugPlay_9()
    {
        PlaySound(SoundIds.MenuClick);
    }
    [Button("Play SceneryDestroy")]
    public void DebugPlay_10()
    {
        PlaySound(SoundIds.SceneryDestroy);
    }
    [Button("Play StoneDestroy")]
    public void DebugPlay_11()
    {
        PlaySound(SoundIds.StoneDestroy);
    }
    [Button("Play Trap")]
    public void DebugPlay_12()
    {
        PlaySound(SoundIds.Trap);
    }
    [Button("Play UnitPickup")]
    public void DebugPlay_13()
    {
        PlaySound(SoundIds.UnitPickup);
    }
    [Button("Play Win")]
    public void DebugPlay_14()
    {
        PlaySound(SoundIds.Win);
    }
    [Button("Play FinalChestDestroy")]
    public void DebugPlay_15()
    {
        PlaySound(SoundIds.FinalChestDestroy);
    }
    [Button("Play StoneDestroyWin")]
    public void DebugPlay_16()
    {
        PlaySound(SoundIds.StoneDestroyWin);
    }
#endif

}
