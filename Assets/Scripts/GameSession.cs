using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int _playerLives = 3;

    [SerializeField] TextMeshProUGUI livesText;
    
    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start() 
    {
        livesText.text = _playerLives.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (_playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    void TakeLife()
    {
        _playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = _playerLives.ToString();        
    }

    void ResetGameSession()
    {
        Application.Quit();
        //SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
