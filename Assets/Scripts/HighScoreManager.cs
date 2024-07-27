using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    public GameObject scoreRowPrefab; // Asigna el prefab de la fila en el Inspector
    public Transform scoreContainer; // Asigna el contenedor de puntuaciones en el Inspector

    private void Start()
    {
        UpdateHighScoreTable();
    }

    public void UpdateHighScoreTable()
    {
        // Limpia el contenedor de puntuaciones
        foreach (Transform child in scoreContainer)
        {
            Destroy(child.gameObject);
        }

        // Verifica si hay puntuaciones altas
        if (GameManager.Instance.highScores == null || GameManager.Instance.highScores.Count == 0)
        {
            // Si no hay puntuaciones, puedes agregar un mensaje informativo o simplemente salir
            Debug.Log("No hay puntuaciones altas para mostrar.");
            return;
        }

        // Crea nuevas filas para cada puntuación alta
        foreach (HighScore highScore in GameManager.Instance.highScores)
        {
            GameObject rowObject = Instantiate(scoreRowPrefab, scoreContainer);
            Text[] texts = rowObject.GetComponentsInChildren<Text>();
            texts[0].text = highScore.playerName; // Nombre del jugador
            texts[1].text = highScore.score.ToString(); // Puntuación
        }
    }
}
