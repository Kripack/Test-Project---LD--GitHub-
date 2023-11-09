using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;

public class GeneralSettings : MonoBehaviour
{
    public int TargetFPS = 70;
    public static GeneralSettings Instance;
    public static bool Pause = false;

    public FlagColorController _flagColorController;
    public UITextEffect _uiTextEffect;

    #region Example
    public ExampleData exampleData;
    [System.Serializable]
    public class ExampleData
    {
        public Vector3 Pos;
    }
    #endregion

    #region General
    public SettingsData settings;
    [System.Serializable]
    public class SettingsData
    {
        public bool NeedDestroyCastle = true;
        public bool FinishLine = false;
    }
    #endregion

    #region UI
    public UIData _uiData;
    [System.Serializable]
    public class UIData
    {
        public GameObject WinPanel;
        public GameObject LosePanel;
        public float WaitBeforePanel = 2f;
        public GameObject NextLevelButton;
        public GameObject NextZoneButton;
        public GameObject ResetLevelButton;
        public GameObject ZonePanel;
        public GameObject ZoneNew;
        public TextMeshProUGUI ZoneText;
        public Slider ZoneSlider;
        public CustomToggle Music;
        public CustomToggle Vibro;
        public GameObject SettingsPanel;
        public TextMeshProUGUI LevelNo;
        public GameObject Logo;
        public GameObject Tutorial;
        public float WaitTutorial = 3f;
        public TextMeshProUGUI CoinsCounter;
        public TextMeshProUGUI WinCoinsCounter;
        public float WinCoinsCounterLerp = 2f;
    }
    #endregion

    #region privates vars
    private float _startCoins;
    #endregion

    #region WIN
    public void WinFunc()
    {
        StartCoroutine(Win());
    }
    public void LoseFunc()
    {
        StartCoroutine(Lose());
    }
    public IEnumerator Win()
    {
        if (Result)
        {
            yield break;
        }
        Result = true;
        SoundController.Instance.PlaySound(SoundController.SoundIds.StoneDestroyWin);
        FindObjectOfType<PlayerController>().SetMove(false);
        Scenes = ScenesRoll.Instance;
        _uiData.CoinsCounter.text = GetCoin().ToString("0");
        yield return new WaitForSecondsRealtime(_uiData.WaitBeforePanel);
        _uiData.WinCoinsCounter.gameObject.SetActive(false);
        _uiData.WinPanel.SetActive(true);
        SoundController.Instance.PlaySound(SoundController.SoundIds.Win);
        yield return new WaitForSeconds(0.5f);
        // win coins lerp
        _uiData.WinCoinsCounter.text = "+0";
        _uiData.WinCoinsCounter.gameObject.SetActive(true);
        float nowCoins = 0f;
        float endCoins = GetCoin() - _startCoins;
        while (nowCoins < endCoins)
        {
            nowCoins = Mathf.Lerp(nowCoins, endCoins * 1.05f, Time.fixedDeltaTime * _uiData.WinCoinsCounterLerp);
            _uiData.WinCoinsCounter.text = "+" + ((int)nowCoins).ToString("0");
            yield return new WaitForFixedUpdate();
        }
        _uiData.WinCoinsCounter.text = "+" + ((int)endCoins).ToString("0");
        // end
        yield return new WaitForSeconds(0.5f);
        int levelComplete = 1 + Scenes.GetLevelNo();
        int zonePercent = levelComplete % 5 == 0 ? 100 : Mathf.FloorToInt(((float)(levelComplete % 5) / 5f) * 100f);
        int fromZonePercent = zonePercent - 20;
        Debug.Log("zone percent = " + zonePercent.ToString() + "%");
        Scenes.SaveSceneName();

        _uiData.ZoneText.text = fromZonePercent.ToString() + '%';
        _uiData.ZoneSlider.value = (float)fromZonePercent * 0.01f;

        DOTween.To(() => fromZonePercent, x => fromZonePercent = x, zonePercent, 1.5f)
                    .OnUpdate(() =>
                    {
                        _uiData.ZoneText.text = fromZonePercent.ToString() + '%';
                        _uiData.ZoneSlider.value = (float)fromZonePercent * 0.01f;
                    })
                    .OnComplete(() =>
                    {
                        _uiData.ZoneText.text = fromZonePercent.ToString() + '%';
                        _uiData.ZoneSlider.value = (float)fromZonePercent * 0.01f;
                    });

        _uiData.NextZoneButton.gameObject.SetActive(levelComplete % 5 == 0);
        _uiData.ZoneNew.SetActive(levelComplete % 5 == 0);
        _uiData.NextLevelButton.SetActive(levelComplete % 5 != 0);
        _uiData.ZonePanel.SetActive(true);
    }
    #endregion

    #region LOSE
    public IEnumerator Lose()
    {
        yield return new WaitForSeconds(0.1f);
        if (Result)
        {
            yield break;
        }
        Result = true;
        SoundController.Instance.PlaySound(SoundController.SoundIds.StoneDestroy);
        FindObjectOfType<PlayerController>().SetMove(false);
        _uiData.CoinsCounter.text = GetCoin().ToString();
        yield return new WaitForSecondsRealtime(_uiData.WaitBeforePanel);
        _uiData.LosePanel.SetActive(true);
        SoundController.Instance.PlaySound(SoundController.SoundIds.Lose);
        yield return new WaitForSeconds(0.5f);
        _uiData.ResetLevelButton.SetActive(true);
    }
    #endregion

    #region Start && Settings
    private ScenesRoll Scenes;
    private void Awake()
    {
        Instance = this;
        Pause = false;
        Time.timeScale = 1f;
        Application.targetFrameRate = TargetFPS;
    }
    private void Start()
    {
        Application.targetFrameRate = TargetFPS;
        _uiData.Vibro.SetValue(PlayerPrefs.GetInt("Vibro", 1) == 1 ? true : false, false);
        _uiData.Music.SetValue(PlayerPrefs.GetInt("Music", 1) == 1 ? true : false, false);
        _uiData.LevelNo.text = "Level " + (ScenesRoll.Instance.GetLevelNo() + 1).ToString();
        _uiData.CoinsCounter.text = PlayerPrefs.GetInt("Coins", 0).ToString();
        AddCoin(0);
        _tutorialUpdate = StartCoroutine(MouseDownUpdate());
    }
    public void SettingsOpenHide()
    {
        if (_uiData.SettingsPanel.activeSelf)
        {
            _uiData.SettingsPanel.SetActive(false);
            Pause = false;
            Time.timeScale = 1f;
        }
        else
        {
            _uiData.SettingsPanel.SetActive(true);
            Pause = true;
            Time.timeScale = 0f;
        }
    }
    public void UpdateSettings()
    {
        PlayerPrefs.SetInt("Vibro", GetVibro() ? 1 : 0);
        PlayerPrefs.SetInt("Music", GetMusic() ? 1 : 0);
    }
    public bool GetVibro()
    {
        return _uiData.Vibro.GetValue();
    }
    public bool GetMusic()
    {
        return _uiData.Music.GetValue();
    }
    #endregion

    #region Coins
    private float _coinsBonus = 1f;
    public void SetCoinBonus(float bonus)
    {
        _coinsBonus = bonus;
    }
    public void AddCoin(float count, Transform obj = null)
    {
        float startCoins = PlayerPrefs.GetFloat("Coins", 0);
        startCoins += count * _coinsBonus;
        PlayerPrefs.SetFloat("Coins", startCoins);
        _uiData.CoinsCounter.text = (startCoins).ToString("0");
        if (obj)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(obj.position);
            _uiTextEffect.ShowEffect(pos, count * _coinsBonus);
        }
    }
    public float GetCoin()
    {
        return Mathf.Round(PlayerPrefs.GetFloat("Coins", 0));
    }
    public void RemoveCoin(float count)
    {
        float startCoins = PlayerPrefs.GetFloat("Coins", 0);
        startCoins -= count;
        startCoins = Mathf.Max(startCoins, 0);
        PlayerPrefs.SetFloat("Coins", startCoins);
        _uiData.CoinsCounter.text = (startCoins).ToString("0");
    }
    #endregion

    #region Tutorial && Logo
    private Coroutine _tutorialUpdate;
    public void SetTutorial(bool state)
    {
        if (_tutorialUpdate != null)
            StopCoroutine(_tutorialUpdate);
        if (state)
        {
            _tutorialUpdate = StartCoroutine(MouseDownUpdate());
        }
        _uiData.Tutorial.SetActive(state);
    }
    private IEnumerator MouseDownUpdate()
    {
        float timer = _uiData.WaitTutorial;
        while (!HaveResult())
        {
            bool down = Input.GetMouseButton(0);

            if (down || _uiData.SettingsPanel.activeSelf)
            {
                timer = _uiData.WaitTutorial;
                _uiData.Tutorial.SetActive(false);
            }
            else
            {
                if (timer > 0f)
                {
                    timer -= 0.04f;
                }
                else
                {
                    _uiData.Tutorial.SetActive(true);
                }
            }
            yield return new WaitForSeconds(0.04f);
        }
    }
    public void StartZoneDown()
    {
        if (!Playning)
        {
            Playning = true;
            Debug.Log("Start Game!");
            Vibration.Vibrate(Vibration.VibroType.Short);
            SoundController.Instance.PlaySound(SoundController.SoundIds.Catapult);
            _startCoins = GetCoin();
            _uiData.Logo.SetActive(false);
            PlayerController.Controller.SetMove(true);
        }
    }
    #endregion

    #region Getters && Scenes manipulation
    private bool Result = false;
    private bool Playning = false;

    public bool HaveResult()
    {
        return Result;
    }

    public void ResetScene()
    {
        FindObjectOfType<ObjsLoader>().ReloadScene();

        //UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void LoadNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Scenes.GetNextScene(), UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    #endregion

    public static void VibrateStop()
    {
        Vibration.Cancel();
    }

}
