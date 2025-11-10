
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

[System.Serializable]
public class LocalEntry
{
    public string Username;
    public int Score;

    public LocalEntry(string username, int score)
    {
        Username = username;
        Score = score;
    }
}

public class LeaderBoard : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;

    private List<LocalEntry> localEntries = new List<LocalEntry>();

    private string publicLeaderBoardKey =
        "7247d148d58cce272525a79c09fc77d2c5a2947c60c70250d2b59c5864ebc752";

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderBoardKey, ((msg) =>
        {
            if (msg == null || msg.Length == 0)
            {
                Debug.LogWarning("No se pudo obtener el leaderboard. Usando tabla local.");
                UpdateLocalLeaderboard();
                return;
            }

            for (int i = 0; i < names.Count; i++)
            {
                names[i].text = "";
                scores[i].text = "";
            }

            for (int i = 0; i < Mathf.Min(names.Count, msg.Length); i++)
            {
                string displayName = msg[i].Username.Split('_')[0];
                names[i].text = displayName;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        string uniqueUsername = username + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss");

        LeaderboardCreator.UploadNewEntry(publicLeaderBoardKey, uniqueUsername, score, ((success) =>
        {
            if (success)
            {
                GetLeaderboard();
            }
            else
            {
                Debug.LogWarning("Error al subir. Guardando localmente.");
                localEntries.Add(new LocalEntry(username, score));
                UpdateLocalLeaderboard();
            }
        }));
    }

    private void UpdateLocalLeaderboard()
    {
        // Ordenar por puntaje descendente
        localEntries.Sort((a, b) => b.Score.CompareTo(a.Score));

        for (int i = 0; i < names.Count; i++)
        {
            if (i < localEntries.Count)
            {
                names[i].text = localEntries[i].Username;
                scores[i].text = localEntries[i].Score.ToString();
            }
            else
            {
                names[i].text = "";
                scores[i].text = "";
            }
        }
    }

}

// Secret key 1fcea51090445e3f131e383d18b4c6f1950f85584f37940c23dd774880c9ef0780c8127d292293adcbec7bbc67a052efe09d7797524ccffcb4ca8a1181c02148ddd0a75a495bff54d1f7f42ef41a61484cb3d83ec917d9c60c85c897e0265a8a544387b2e847d8aca890dff6d7ec0dc3711a21fab2fee920c692544c96eb4996
