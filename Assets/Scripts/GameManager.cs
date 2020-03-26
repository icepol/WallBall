using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject resultsPanel;

    [SerializeField] private Player playerBlue;
    [SerializeField] private Player playerGreen;

    [SerializeField] private Text scoreBlue;
    [SerializeField] private Text scoreGreen;
    [SerializeField] private Text resultMessage;
    [SerializeField] private Text ballsCount;
        
    void Start()
    {
        EventManager.AddListener(Events.LevelStarted, OnLevelStarted);
        EventManager.AddListener(Events.LevelFinished, OnLevelFinished);
        EventManager.AddListener(Events.BallOnGrass, OnBallOnGrass);
        EventManager.AddListener(Events.UpdateUI, OnUpdateUI);
        EventManager.AddListener(Events.IncreaseScore, OnIncreaseScore);
        
        SetupPlayers();

        if (GameState.BallsCount == 11)
        {
            // begin of the game, menu is on screen
            menuPanel.SetActive(true);
            gamePanel.SetActive(false);
            resultsPanel.SetActive(false);
        }
        else
        {
            // in the game
            menuPanel.SetActive(false);
            gamePanel.SetActive(true);
            resultsPanel.SetActive(false);
        }
        
        EventManager.TriggerEvent(Events.UpdateUI);
    }

    private void Update()
    {
        if (GameState.IsGameRunning)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (menuPanel.active)
            {
                // menu is on screen, we just need to run the game
                EventManager.TriggerEvent(Events.LevelStarted);
                AnalyticsEvent.GameStart();
            }
            else if (resultsPanel.active)
            {
                // there is result, we will go back to menu
                SceneManager.LoadScene("Game");
            }
            else
            {
                EventManager.TriggerEvent(Events.LevelStarted);
            }
        }
    }

    // Update is called once per frame
    void OnDestroy()
    {
        EventManager.RemoveListener(Events.LevelStarted, OnLevelStarted);
        EventManager.RemoveListener(Events.LevelFinished, OnLevelFinished);
        EventManager.RemoveListener(Events.BallOnGrass, OnBallOnGrass);
        EventManager.RemoveListener(Events.UpdateUI, OnUpdateUI);
        EventManager.RemoveListener(Events.IncreaseScore, OnIncreaseScore);
    }

    void OnLevelStarted()
    {
        GameState.IsGameRunning = true;
        GameState.IsFirstRun = false;
        
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        resultsPanel.SetActive(false);
    }

    void OnLevelFinished()
    {
        GameState.IsGameRunning = false;
        
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        if (GameState.BallsCount <= 0)
            ShowResults();
        else
        {
            // we are in the middle of the game, play next ball
            SceneManager.LoadScene("Game");
        }
    }

    void OnBallOnGrass()
    {
        GameState.BallsCount--;
        
        EventManager.TriggerEvent(Events.UpdateUI);
        
        StartCoroutine(FinishLevel());
    }

    void OnUpdateUI()
    {
        scoreBlue.text = GameState.Score["Blue"].ToString();
        scoreGreen.text = GameState.Score["Green"].ToString();
        
        ballsCount.text = $"x {GameState.BallsCount}";
    }

    void OnIncreaseScore(string playerName)
    {
        GameState.Score[playerName]++;
        
        EventManager.TriggerEvent(Events.UpdateUI);
    }

    IEnumerator FinishLevel()
    {
        // add score to the round winner
        foreach (Player player in FindObjectsOfType<Player>())
        {
            if (!player.IsPlaying)
                EventManager.TriggerEvent(Events.IncreaseScore, player.Name);
        }

        yield return new WaitForSeconds(1);
        
        EventManager.TriggerEvent(Events.LevelFinished);
    }

    void SetupPlayers()
    {
        Ball ball = FindObjectOfType<Ball>();
        Player[] players = FindObjectsOfType<Player>();
        
        // reset all players
        foreach (Player player in players)
        {
            player.IsPlaying = false;
            SetupScore(player.Name);
        }

        if (GameState.IsFirstRun)
            // random player will start the game
            GameState.ActivePlayer = Random.Range(0, 1);
        else
        {
            // next player will start the game
            GameState.ActivePlayer++;
            if (GameState.ActivePlayer >= players.Length)
                GameState.ActivePlayer = 0;   
        }
        
        players[GameState.ActivePlayer].IsPlaying = true;
        
        ball.transform.SetParent(players[GameState.ActivePlayer].transform);
        ball.transform.position = new Vector2(
            players[GameState.ActivePlayer].transform.position.x, 
            ball.transform.position.y);
    }

    void ShowResults()
    {
        int maxScore = 0;

        Dictionary<string, Player> players = new Dictionary<string, Player>();
        foreach (Player player in FindObjectsOfType<Player>())
        {
            players[player.Name] = player;
        }

        Dictionary<string, int> score = new Dictionary<string, int>();
        foreach (string playerName in GameState.Score.Keys)
        {
            if (GameState.Score[playerName] > maxScore)
            {
                maxScore = GameState.Score[playerName];

                resultMessage.text = $"{playerName} player wins!";
                resultMessage.color = players[playerName].DressColor;
            }
            
            // reset score
            score[playerName] = 0;
        }

        // reset balls count
        GameState.BallsCount = 11;

        GameState.Score = score;
        
        resultsPanel.SetActive(true);
        
        AnalyticsEvent.GameOver();
    }

    void SetupScore(string playerName)
    {
        if (!GameState.Score.ContainsKey(playerName))
            GameState.Score[playerName] = 0;
    }
}
