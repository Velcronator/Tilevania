using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int playerLives = 3;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI coinsText;

    // Singleton pattern
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject gameManager = new GameObject("GameManager");
                _instance = gameManager.AddComponent<GameManager>();
                DontDestroyOnLoad(gameManager);
            }
            return _instance;
        }
    }

    public int CoinsCount { get; private set; }
    //public event Action<int> OnLivesChanged;
    //public event Action<int> OnCoinsCountChanged;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        InitialiseStuff();
    }

    private void InitialiseStuff()
    {
        playerLives = 3;
        CoinsCount = 0;
    }

    private void Start()
    {
        livesText.text = playerLives.ToString();
        coinsText.text = CoinsCount.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    private void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    private void TakeLife()
    {
        playerLives--;
        livesText.text = playerLives.ToString();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public int GetPlayerLives()
    {
        return playerLives;
    }

    public void AddScore(int score)
    {
        CoinsCount += score;
        coinsText.text = CoinsCount.ToString();
    }
}
