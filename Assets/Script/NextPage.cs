using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;



public class NextPage : MonoBehaviour
{
    public Button checkButton;
    public string[] questions;
    public TMP_Text questionText;
    private int currentRound = 0;
    public GameObject objectToDeactivate; // Assign in Inspector
    public GameObject objectToActivate; // Assign in Inspector
    public Player[] players;
    private bool firstActivation=false;

    // Start is called before the first frame update
    void Start()
    {
        checkButton.onClick.AddListener(NextRound);
        questionText.text = questions[currentRound];
    }

    // Update is called once per frame
    void Update()
    {
        if (firstActivation)
        {
            foreach (Player player in players)
            {
                player.SetPosition(currentRound-1);
            }
            firstActivation=false;
        }
    }
    void NextRound()
    {
        
        currentRound++;

        if (currentRound ==7)
        {
            Debug.Log("Game over!");
            SceneManager.LoadScene("Menu");
            // Handle end of game here

        }

        if (currentRound ==1)
        {
            objectToDeactivate.SetActive(false);
            objectToActivate.SetActive(true);
            firstActivation=true;
            // Reset players' positions and colors here
            Debug.Log("Activation Management");

        }

        Debug.Log("Text & Players management");
        questionText.text = questions[currentRound];
        foreach (Player player in players)
            {
                player.SetPosition(currentRound-1);
            }

    }
}
