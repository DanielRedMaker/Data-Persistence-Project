using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject canvas;
    public string ScoreName;
    public TMP_InputField ImputFieldName;
    public List<HighScore> highScores = new List<HighScore>();
    //ENCAPSULATION
    private const int MAX_HIGH_SCORES = 5;

    private void Awake()
    {
        //si la instancia es diferente de nulo o vacio
        if (Instance != null)
        {
            //destruimos el gameObject duplicado (MainManager adicional)
            Destroy(gameObject);
            return;
        }
        Instance = this;


        //esto hace que los datos no se destruyan al cambiar de escena
        DontDestroyOnLoad(gameObject);
        //carga los datos si es que hay
        LoadHighScores();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    //---------------------------------------------------------------------
    [System.Serializable]
    public class HighScore
    {
        public string playerName;
        public int score;
    }

    [System.Serializable]
    public class HighScoreList
    {
        public List<HighScore> highScores;
    }
    //---------------------------------------------------------------------

    //POLIMORFYSN
    public void OnStartButtonPressed()
    {
        // Obtiene el texto del InputField
        string playerName = ImputFieldName.text;

        ScoreName = playerName;

        // Carga la siguiente escena
        SceneManager.LoadScene("main");

        canvas.SetActive(false);
    }
    public void SaveHighScore(string playerName, int score)
    {
        HighScore newHighScore = new HighScore
        {
            playerName = playerName,
            score = score,
        };

        highScores.Add(newHighScore);
        highScores = highScores.OrderByDescending(h => h.score).ToList();

        if (highScores.Count > MAX_HIGH_SCORES)
        {
            highScores.RemoveAt(highScores.Count - 1);
        }

        SaveHighScores();
    }
   
    //ABSTRACCTION
    private void SaveHighScores()
    {
        HighScoreList highScoreList = new HighScoreList { highScores = this.highScores };
        string json = JsonUtility.ToJson(highScoreList);
        File.WriteAllText(Application.persistentDataPath + "/highscores.json", json);
    }
    private void LoadHighScores()
    {
        string path = Application.persistentDataPath + "/highscores.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScoreList loadedData = JsonUtility.FromJson<HighScoreList>(json);
            highScores = loadedData.highScores;
        }
    }
}
