using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class TTSManager : MonoBehaviour
{
    public static TTSManager Instance { get; private set; }

    [Header("-----------ISO 639-1 Setting-----------")]
    private const string _englishState = "En";
    private const string _japaneseState = "Ja";
    private const string _koreanState = "Ko";
    private const string _chinesseState = "zh-CN";
    private const string _spanishState = "Es";
    private const string _url = "https://translate.google.com/translate_tts?ie=URF-8&total=1&idx=0&textlen=32&client=tw-ob&q=";

    [Header("--------------Setting Field--------------")]
    private string _defaultFilePath;
    private const string _defaultTTSFolder = "TTSFiles";
    private string _defaultTTSFileName = "TemptTTS";
    private AudioSource _audioSource;
    private StringBuilder _sb;
    private Coroutine _ttsCor = null;
    private const string _audioFormet = ".mp3";

    #region Unity Life Cycle
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        _defaultFilePath = Application.dataPath + "/" + _defaultTTSFolder;
    }

    private void Start()
    {
        _audioSource = this.GetComponent<AudioSource>();
    }
    #endregion

    #region TTS Speaker
    public void SpeakTTS(string sentence, TTSLanguage type = TTSLanguage.Korean)
    {
        if (string.IsNullOrEmpty(sentence))
        {
            Debug.LogError("Empty Input");
            return;
        }

        StartSpeakingTTS(sentence, type);
    }

    private void StartSpeakingTTS(string sentence, TTSLanguage type)
    {
        if (_ttsCor != null)
        {
            StopCoroutine(_ttsCor);
            _ttsCor = null;
        }

        _ttsCor = StartCoroutine(PlayTTS(_url + GetTTSUrl(sentence, GetLanguageState(type), type)));
    }

    private IEnumerator PlayTTS(string str)
    {
        WWW www = new WWW(str);
        yield return www;

        _audioSource.clip = www.GetAudioClip(false, true, AudioType.MPEG);
        _audioSource.Play();
    }

    private string GetTTSUrl(string text, string stateName, TTSLanguage type = TTSLanguage.Default)
    {
        if (type.Equals(TTSLanguage.Chinese))
        {
            _sb = new StringBuilder(3);
            _sb.Append(text);
            _sb.Append("&tl=");
            _sb.Append(stateName);
            return _sb.ToString();
        }

        _sb = new StringBuilder(4);
        _sb.Append(text);
        _sb.Append("&tl=");
        _sb.Append(stateName);
        _sb.Append("-gb");

        return _sb.ToString();
    }
    #endregion

    #region TTS Creator
    public void CreateTTSFile(string sentence, TTSLanguage type = TTSLanguage.Korean, string fileName = null, string filePath = null)
    {
        
        if (string.IsNullOrEmpty(fileName))
        {
            _sb = new StringBuilder(3);
            _sb.Append(_defaultTTSFileName);
            _sb.Append("_");
            _sb.Append(GetLanguageState(type));

            fileName = _sb.ToString();
        }

        if (string.IsNullOrEmpty(filePath))
        {
            filePath = _defaultFilePath;
        }

        // No Directory => Create
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        string ttsUrl = _url + GetTTSUrl(sentence, GetLanguageState(type), type);
        byte[] ttsData = DownloadTTSData(ttsUrl);


        if (ttsData != null)
        {
            string outputPath = Path.Combine(filePath, fileName + _audioFormet);
            File.WriteAllBytes(outputPath, ttsData);
            Debug.Log("TTS file created: " + outputPath);
        }
        else
        {
            Debug.LogError("Failed to download TTS data.");
        }
    }

    private byte[] DownloadTTSData(string url)
    {
        using (WWW www = new WWW(url))
        {
            while (!www.isDone)
            {
                // Waiting...
            }

            if (string.IsNullOrEmpty(www.error))
            {
                return www.bytes;
            }
            else
            {
                Debug.LogError("TTS download error: " + www.error);
                return null;
            }
        }
    }

    private string GetLanguageState(TTSLanguage type)
    {
        switch (type)
        {
            case TTSLanguage.Korean:
                return _koreanState;
            default:
            case TTSLanguage.English:
                return _englishState;
            case TTSLanguage.Japanese:
                return _japaneseState;
            case TTSLanguage.Chinese:
                return _chinesseState;
            case TTSLanguage.Spanish:
                return _spanishState;
        }
    }
    #endregion
}

public enum TTSLanguage
{
    Default = 0,
    Korean,
    English,
    Japanese,
    Chinese,
    Spanish
}
