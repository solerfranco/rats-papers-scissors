using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject leaderBoard;
    public PlayerMovementSlap player;
    public int score;
    public int combo;
    public TextMeshProUGUI palabrasdeAliento;
    public TextMeshProUGUI showScore;
    public TextMeshProUGUI showCombo;
    public Vector3 offset = new Vector3(0, 2f, 0); // Aj;usta según lo alto que quieras el texto
    
    private string[] listaPalabras = { "Ok!", "Great!", "Good!", "Nice!", "Awesome" };
    private Vector3 comboOffset;
    private int lastCombo = -1;
    private int currentCombo;
    public Color textColor;
    public float tiempoVisible = 1.5f; // segundos que el texto estará visible

    private void Start()
    {
        palabrasdeAliento.color = textColor;
    }
    void Update()
    {

        currentCombo = player.killStreak;

        // Solo actualiza si el combo cambió
        if (currentCombo > lastCombo)
        {

            lastCombo = currentCombo;

            // Genera nueva posición aleatoria
            Vector2 randomOffset = Random.insideUnitCircle * 1.5f;
            comboOffset = new Vector3(randomOffset.x, 2f, randomOffset.y);

            string palabra = listaPalabras[Random.Range(0, listaPalabras.Length)];
            StartCoroutine(MostrarPalabraTemporal(palabra));

        }

        if (currentCombo == 0 && lastCombo != 0)
        {
            lastCombo = 0;
            palabrasdeAliento.gameObject.SetActive(false);
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(player.transform.position + comboOffset);
        palabrasdeAliento.transform.position = screenPos;

        showCombo.text = currentCombo.ToString();
        showScore.text = score.ToString();

        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (leaderBoard.activeSelf) {
            Time.timeScale = 0f;
        }
        else {Time.timeScale = 1f; }

    }

    IEnumerator MostrarPalabraTemporal(string palabra)
    {

        palabrasdeAliento.text = palabra;
        palabrasdeAliento.gameObject.SetActive(true);

        yield return new WaitForSeconds(tiempoVisible);

        palabrasdeAliento.gameObject.SetActive(false);
    }

}
