using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    //public static MainManager Instance;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public Button mainMenuButton;
    public Text ScoreText;
    public Text ScoreBest;


    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;



    // Start is called before the first frame update
    void Start()
    {
        mainMenuButton.gameObject.SetActive(false);
        UpdateBestScoreText();

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    //-------------------------------------------------------------
    private void UpdateBestScoreText()
    {
        if (GameManager.Instance.highScores.Count > 0)
        {
            // Suponiendo que la primera entrada de highScores es la más alta
            GameManager.HighScore bestScore = GameManager.Instance.highScores[0];
            ScoreBest.text = $"Best Score: {bestScore.playerName} - {bestScore.score}";
        }
    }


    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $" Name: {GameManager.Instance.ScoreName}  Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
        // Guarda la puntuación si es más alta que la anterior
        GameManager.Instance.SaveHighScore(GameManager.Instance.ScoreName, m_Points);

        // Actualiza la mejor puntuación en la UI
        UpdateBestScoreText();
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        GameManager.Instance.canvas.SetActive(true);
    }
}