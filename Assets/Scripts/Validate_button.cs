using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class Validate_button : MonoBehaviour
{
    public TextMeshProUGUI QuestionText;
    public List<TextMeshProUGUI> AnswerTexts;
    public TextMeshProUGUI Location_text;
    public GameObject validationButton;
    public GameObject correctionButton;
    public TextMeshProUGUI correctionText;

    private List<string[]> csvData = new List<string[]>();
    private int bonneReponseIndex; // Index de la bonne réponse pour la question actuelle
    private int totbonnesrep = 0;
    private int selectedIndex; // Declare selectedIndex here

    // Déclarer les boutons Answer_1 à Answer_4
    public Button Answer_1;
    public Button Answer_2;
    public Button Answer_3;
    public Button Answer_4;

    public Image netImage; // Ajoutez cette ligne pour référencer l'image "Net"


    void Start()
    {
        // Ajouter des écouteurs d'événements pour les boutons de réponse
        Answer_1.onClick.AddListener(() => ClickonAnswer(1));
        Answer_2.onClick.AddListener(() => ClickonAnswer(2));
        Answer_3.onClick.AddListener(() => ClickonAnswer(3));
        Answer_4.onClick.AddListener(() => ClickonAnswer(4));

        validationButton.GetComponent<Button>().onClick.AddListener(ValidateAnswer); // Attach ValidateAnswer to the click event of the validation button

        correctionButton.GetComponent<Button>().onClick.AddListener(DisplayNewQuestion); // Attach DisplayNewQuestion to the click event of the correction button

        netImage.GetComponent<Button>().onClick.AddListener(ReturnToMenu); // Ajoutez cette ligne pour gérer le clic sur l'image "Net"

        // Charger le CSV et afficher la première question
        LoadCSV(@"C:\Users\lilou\Documents\CSNumVi\Impliquer\VolleyMaster\Assets\Regles_formatclassique.csv");
        DisplayRandomQuestion();
    }

    void LoadCSV(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                // Ignorer la première ligne (noms de colonnes)
                reader.ReadLine();

                // Lire le reste du fichier
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');
                    csvData.Add(values);
                }
            }
        }
        else
        {
            Debug.LogError("CSV file not found at: " + filePath);
        }
    }

    void DisplayRandomQuestion()
    {
        int niveau = Play_button.niveau; // Niveau spécifié

        // Mise à jour du texte de Location_text avec le niveau
        Location_text.text = "Section Règles - Niveau " + niveau;

        // Filtrer les lignes du CSV pour le niveau spécifié
        List<string[]> filteredData = csvData.FindAll(line =>
        {
            int parsedLevel;
            if (int.TryParse(line[6], out parsedLevel))
            {
                return parsedLevel == niveau;
            }
            else
            {
                return false;
            }
        });

        if (filteredData.Count == 0)
        {
            Debug.LogWarning("Aucune question trouvée pour le niveau " + niveau);
            return;
        }

        // Sélectionner une ligne aléatoire parmi les données filtrées
        int randomIndex = Random.Range(0, filteredData.Count);
        string[] randomLine = filteredData[randomIndex];

        // Sauvegarder l'index de la bonne réponse
        bonneReponseIndex = int.Parse(randomLine[5]);

        // Afficher la question
        QuestionText.text = randomLine[0];

        // Afficher les réponses
        for (int i = 0; i < AnswerTexts.Count; i++)
        {
            AnswerTexts[i].text = randomLine[i + 1];
        }

        Debug.Log("L'index de la bonne réponse est : " + bonneReponseIndex); // Ajout du message de débogage


        // Afficher le bouton de validation
        validationButton.SetActive(true);
    }

    public void ClickonAnswer(int selectedIndex)
    {
        this.selectedIndex = selectedIndex;
    }

    public void ValidateAnswer()
    {
        validationButton.SetActive(false); // Cacher le bouton Valider

        if (selectedIndex == bonneReponseIndex)
        {
            // Si la réponse est correcte, incrémentez le compteur de bonnes réponses
            totbonnesrep++;
            correctionText.text = "Bravo !";
            Debug.Log("Bonne réponse ! Nombre total de bonnes réponses : " + totbonnesrep);
            correctionButton.GetComponent<Image>().color = Color.green; // Change la couleur du bouton correctionButton en vert
        }
        else
        {
            correctionText.text = "Dommage...";
            Debug.Log("Mauvaise réponse !");
            correctionButton.GetComponent<Image>().color = Color.red; // Change la couleur du bouton correctionButton en rouge
        }

        correctionButton.SetActive(true); // Afficher le bouton Correction
    }

    public void DisplayNewQuestion()
    {
        correctionButton.SetActive(false); // Cacher le bouton Correction
        DisplayRandomQuestion(); // Afficher une nouvelle question
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu"); 
    }
}
