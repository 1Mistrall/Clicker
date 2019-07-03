using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GameManager : MonoBehaviour

{
    public static GameManager Instance;
    public Camera Camera;
    [Range(3,10)]
    public int Tries = 5;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI LevelText;
    public GameObject FlowerPrefab;
    public GameObject GameOverCanvas;
    public GameObject PauseCanvas;

    private Spawner _spawner;
    private int _score;
    private int _tries;
    private int _level;
    private Dictionary<int, float> _levels = new Dictionary<int, float>();
    private int[] ScoreBorders = { 1000, 2000, 3500, 5000};

    private void Awake()
    {
        if (Instance != null) 
        {
            Destroy(gameObject);
        }
        else 
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (_spawner == null)
        {
            GameObject go = new GameObject(name: "Spawner");
            _spawner = go.AddComponent<Spawner>();
            _spawner.SetManager(this);
        }

        ResetValues();
        InitLevels();
        if (_levels.TryGetValue(_level, out float time))
        {
            _spawner.SetSpawnTime(time);
        }

    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void OnFlowerDieEvent(Flower flower, double amount)
    {
        if(Tries <= 0)
        {
            GameOver();
        }
        else if (amount <= double.Epsilon )
        {
            Tries -= 1;
        }
        _score += Convert.ToInt32(amount * 100);
        SetScoreText();
        SetLevel();
    }

    public void SetScoreText()
    {
        ScoreText.SetText("Score: " + _score);
    }

    public void SetLevelText()
    {
        LevelText.SetText("Level: " + _level);
    }

    public void SetLevel()
    {
            if(_level < _levels.Count-1 && _score > ScoreBorders[_level])
            {
                _level++;
                if (_levels.TryGetValue(_level, out float time))
                {
                    _spawner.SetSpawnTime(time);
                }
                return;
        }
        SetLevelText();
    }

    public void InitLevels()
    {
        _levels.Add(0, 2.5F);
        _levels.Add(1, 2F);
        _levels.Add(2, 1F);
        _levels.Add(3, 0.7F);
        _levels.Add(4, 0.3F);
        Debug.Log(_levels.Count);
    }

    public void ResetValues()
    {
        _score = 0;
        SetScoreText();

        _level = 0;
        SetLevelText();

        Tries = 5;
    }

    public void Restart()
    {
        ResetValues();

        if (_levels.TryGetValue(_level, out float time))
        {
            _spawner.SetSpawnTime(time);
        }

        if (GameOverCanvas.activeSelf)
        {
            GameOverCanvas.SetActive(false);
        }

        if (PauseCanvas.activeSelf)
        {
            PauseCanvas.SetActive(false);
        }

        _spawner.Restart();
        Time.timeScale = 1;
    }

    private void GameOver()
    {
        _spawner.StopAllCoroutines();
        GameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void Pause()
    {
        PauseCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        PauseCanvas.SetActive(false);
        Time.timeScale = 1;
    }
}
