using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class CheckTargetsButton : MonoBehaviour
{
    public Button checkButton;
    public Player[] players;
    public string[] questions;
    public TMP_Text questionText;
    private int currentRound = 0;
    private Color Transparent = new Color(0, 0, 0, 0);

    void Start()
    {
        checkButton.onClick.AddListener(CheckAllTargets);
        questionText.text = questions[currentRound];
    }

    void CheckAllTargets()
    {
        bool allPlayersAtTargets = true;

        foreach (Player player in players)
        {
            if (player.IsAtTarget(currentRound))
            {
                
                player.SetPlayerColor(Transparent);
            }
            else
            {
                Color semiTransparentRed = new Color(1, 0, 0, 0.5f);
                player.SetPlayerColor(semiTransparentRed);
                allPlayersAtTargets = false;
            }
        }

        if (allPlayersAtTargets)
        {
            Debug.Log("All players are at their targets.");
            NextRound();

        }
        else
        {
            Debug.Log("Not all players are at their targets.");
        }
    }

    void NextRound()
    {
        currentRound++;

        if (currentRound < questions.Length)
        {
            questionText.text = questions[currentRound];
            // Reset players' positions and colors here
            foreach (Player player in players)
            {
                player.ResetPosition();
                player.SetPlayerColor(Transparent);
            }
        }
        else
        {
            Debug.Log("Game over!");
            SceneManager.LoadScene("Menu");
            // Handle end of game here
        }
    }
}