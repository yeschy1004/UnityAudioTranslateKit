using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TTSCreatorUI : MonoBehaviour
{
    [Header("--------------Input Field--------------")]
    [SerializeField]
    private InputField _fileNameInputfield;
    [SerializeField]
    private InputField _ttsInputfield;

    [Header("--------------Button Field--------------")]
    [SerializeField]
    private Button _krBtn;
    [SerializeField]
    private Button _enBtn;
    [SerializeField]
    private Button _jpBtn;
    [SerializeField]
    private Button _cnBtn;
    [SerializeField]
    private Button _spBtn;

    [Header("--------------Util Field---------------")]
    [SerializeField]
    private Button _eraseBtn;

    public string fileName => _fileNameInputfield.text;
    public string ttsSentence => _ttsInputfield.text;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _krBtn?.onClick.AddListener(OnClickKrBtn);
        _enBtn?.onClick.AddListener(OnClickEnBtn);
        _jpBtn?.onClick.AddListener(OnClickJpBtn);
        _cnBtn?.onClick.AddListener(OnClickCnBtn);
        _spBtn?.onClick.AddListener(OnClickSpBtn);

        _eraseBtn?.onClick.AddListener(OnClickEraseBtn);
    }

    #region UI Action
    private void OnClickKrBtn()
    {
        TTSManager.Instance.SpeakTTS(ttsSentence, TTSLanguage.Korean);
        TTSManager.Instance.CreateTTSFile(ttsSentence, TTSLanguage.Korean, fileName);
    }

    private void OnClickEnBtn()
    {
        TTSManager.Instance.SpeakTTS(ttsSentence, TTSLanguage.English);
        TTSManager.Instance.CreateTTSFile(ttsSentence, TTSLanguage.English, fileName);
    }

    private void OnClickJpBtn()
    {
        TTSManager.Instance.SpeakTTS(ttsSentence, TTSLanguage.Japanese);
        TTSManager.Instance.CreateTTSFile(ttsSentence, TTSLanguage.Japanese, fileName);
    }

    private void OnClickCnBtn()
    {
        TTSManager.Instance.SpeakTTS(ttsSentence, TTSLanguage.Chinese);
        TTSManager.Instance.CreateTTSFile(ttsSentence, TTSLanguage.Chinese, fileName);
    }

    private void OnClickSpBtn()
    {
        TTSManager.Instance.SpeakTTS(ttsSentence, TTSLanguage.Spanish);
        TTSManager.Instance.CreateTTSFile(ttsSentence, TTSLanguage.Spanish, fileName);
    }

    private void OnClickEraseBtn()
    {
        if (string.IsNullOrEmpty(ttsSentence)) return;

        _ttsInputfield.text = string.Empty;
    }
    #endregion
}
