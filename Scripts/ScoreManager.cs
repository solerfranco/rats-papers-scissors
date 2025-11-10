
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public GameController gameController;
    [SerializeField]
    private TextMeshProUGUI score;
    [SerializeField]
    private TMP_InputField inputName;

    public UnityEvent<string, int> submitScoreEvent;

    public void SubmitScore()
    {
        score.text = gameController.score.ToString();
        submitScoreEvent.Invoke(inputName.text, int.Parse(score.text));
    }
}
