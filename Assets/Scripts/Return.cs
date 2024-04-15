using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RetourAuMenu : MonoBehaviour
{
    void Start()
    {
        // Récupère le bouton "Logo" et ajoute un écouteur pour son clic
        Button logoButton = GameObject.Find("Logo").GetComponent<Button>();
        logoButton.onClick.AddListener(RetournerAuMenu);
    }

    void RetournerAuMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
