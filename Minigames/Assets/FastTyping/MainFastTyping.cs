using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainFastTyping : MonoBehaviour
{
    [SerializeField] InputField _inputField;
    [SerializeField] Text _placeholder;
    [SerializeField] Text _textChrono;
    [SerializeField] Text _textScore;
    [SerializeField] Text _textBestScore;
    [SerializeField] LoadingAnim _loadingAnim;
    [SerializeField] Image _background;
    
    List<string> _words = new List<string>();
    Queue<Action> _mainThreadQueue = new Queue<Action>();
    HashSet<string> _usedWords = new HashSet<string>();
    float _gameStart;
    float _lastMistake;
    float _remainingTime;
    bool _gameRunning = false;
    int _score;
    Color _startBackgroundColor;
    int _maxTime = 10;

    // Start is called before the first frame update
    void Start()
    {
        _textBestScore.text = "" + BestScores.FastTypingBest;
        _startBackgroundColor = _background.color;
        _placeholder.text = "";
        _inputField.gameObject.SetActive(false);
        _inputField.onValueChanged.AddListener(_ => { OnValueChangedInputField(); });
        _loadingAnim.gameObject.SetActive(true);
        List<string> textFiles = new List<TextAsset>(Resources.LoadAll<TextAsset>("FastTyping")).ConvertAll<string>( ta => { return ta.text; });

        ThreadPool.QueueUserWorkItem(_ => {
            System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
            Regex rg = new Regex(@"^[a-zA-Z]*$");
            foreach(string file in textFiles){
                string[] tmp = file.Split('\n');
                for(int i = 1; i < tmp.Length; i++){
                    string word = tmp[i].Split(',')[0];
                    if(word.Length < 3){
                        continue;
                    }
                    if(word.Length > 12){
                        continue;
                    }
                    if(!rg.IsMatch(word)){
                        continue;
                    }
                    _words.Add(word);
                }
            }
            timer.Stop();
            Debug.Log(timer.ElapsedMilliseconds + " " + _words.Count);
            
            _mainThreadQueue.Enqueue(() => {
                _inputField.gameObject.SetActive(true);
                _loadingAnim.gameObject.SetActive(false);
                Reset();
                EventSystem.current.SetSelectedGameObject(_inputField.gameObject);
            });
        });
    }

    // Update is called once per frame
    void Update()
    {
        lock(_mainThreadQueue){
            while(_mainThreadQueue.Count > 0){
                _mainThreadQueue.Dequeue()?.Invoke();
            }
        }

        if(_gameRunning){
            _remainingTime = _maxTime - (Time.realtimeSinceStartup - _gameStart);
        }
        _textChrono.text = _remainingTime.ToString("00.0");

        if(_remainingTime < 0){
            EndGame();
        }
    }

    void Reset(){
        _score = 0;
        _textScore.text = "0";
        _remainingTime = _maxTime;
        _usedWords.Clear();
        _placeholder.text = GetWord();
    }

    void StartGame(){
        _gameStart = Time.realtimeSinceStartup;
        _gameRunning = true;
    }

    void EndGame(){
        _textChrono.text = _maxTime + "";
        _remainingTime = _maxTime;
        _inputField.SetTextWithoutNotify("");
        _gameRunning = false;
        if(_score >= BestScores.FastTypingBest){
            BestScores.FastTypingBest = _score;
            _textBestScore.text = "" + _score;
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

    string GetWord(){
        string word;
        do{
            word = _words[UnityEngine.Random.Range(0, _words.Count)];
        }
        while(_usedWords.Contains(word));
        return word;
    }

    public void OnValueChangedInputField(){
        if(!_gameRunning){
            StartGame();
        }

        if(_inputField.text.Length > 0){
            if(Time.realtimeSinceStartup - _lastMistake < 1){
                _inputField.SetTextWithoutNotify("");
                return;
            }

            if(_inputField.text.Length > _placeholder.text.Length){
                PlayerMistake();
                return;
            }

            char lastChar = _inputField.text.ToLower()[_inputField.text.Length - 1];
            char compare = _placeholder.text.ToLower()[_inputField.text.Length - 1];
            if(compare != lastChar){
                PlayerMistake();
                return;
            }

            if(_inputField.text.ToLower() == _placeholder.text.ToLower()){
                _inputField.SetTextWithoutNotify("");
                _placeholder.text = GetWord();
                _score++;
                _textScore.text = "" + _score;
                StartCoroutine(Flash(Color.green));
                return;
            }
        }
    }

    void PlayerMistake(){
        _inputField.SetTextWithoutNotify("");
        _placeholder.text = GetWord();
        StartCoroutine(Flash(Color.red, 0.75f));
        _lastMistake = Time.realtimeSinceStartup;
    }

    IEnumerator Flash(Color col, float duration = 0.5f){
        _background.color = col;
        float tStart = Time.realtimeSinceStartup;

        while(Time.realtimeSinceStartup - tStart < duration){
            _background.color = Color.Lerp(col, _startBackgroundColor, Mathf.InverseLerp(0, duration, Time.realtimeSinceStartup - tStart));
            yield return new WaitForEndOfFrame();
        }

        _background.color = _startBackgroundColor;
    }
}
