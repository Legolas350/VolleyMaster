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
    public Image netImage; 

    public Button Logo;

    public Button ReturnButton;


    void Start()
    {
        // Ajouter des écouteurs d'événements pour les boutons de réponse
        Answer_1.onClick.AddListener(() => ClickonAnswer(1));
        Answer_2.onClick.AddListener(() => ClickonAnswer(2));
        Answer_3.onClick.AddListener(() => ClickonAnswer(3));
        Answer_4.onClick.AddListener(() => ClickonAnswer(4));

        validationButton.GetComponent<Button>().onClick.AddListener(ValidateAnswer); // Attach ValidateAnswer to the click event of the validation button

        correctionButton.GetComponent<Button>().onClick.AddListener(DisplayNewQuestion); // Attach DisplayNewQuestion to the click event of the correction button
        // Charger le CSV et afficher la première question
        LoadCSV(Path.Combine(Application.dataPath, "Regles_formatclassique.csv"));
        DisplayRandomQuestion();

        // Récupère le bouton "Logo" et ajoute un écouteur pour son clic
        Button logoButton = GameObject.Find("Logo").GetComponent<Button>();
        logoButton.onClick.AddListener(RetournerAuMenu);

        // Ajouter un écouteur de clic pour le bouton "Return_button"
        ReturnButton.onClick.AddListener(RetournerAuMenu);
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
            Debug.Log("CSV file not found at: " + filePath);
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
                   return parsedLevel == niveau && int.Parse(line[7]) < 2;
            }
            else
            {
                return false;
            }
        });

        if (filteredData.Count == 0)
        {
            Debug.LogWarning("Aucune question trouvée pour le niveau " + niveau);
            // Afficher un message indiquant que toutes les questions du niveau ont été terminées
            QuestionText.text = "Bravo ! Vous avez fini toutes les questions de ce niveau";
            // Cacher les boutons de réponse et le bouton de validation
            Answer_1.gameObject.SetActive(false);
            Answer_2.gameObject.SetActive(false);
            Answer_3.gameObject.SetActive(false);
            Answer_4.gameObject.SetActive(false);
            validationButton.SetActive(false);
            ReturnButton.gameObject.SetActive(true);
            return;
        }

        // Vérifier si toutes les questions du niveau ont été répondues correctement
        int totalCorrectAnswers = 0;
        foreach (var line in filteredData)
        {
            int questionTotalCorrectAnswers = int.Parse(line[7]);
            if (questionTotalCorrectAnswers >= 2)
            {
                totalCorrectAnswers++;
            }
        }

        if (totalCorrectAnswers == filteredData.Count)
        {
            // Afficher un message indiquant que toutes les questions du niveau ont été terminées
            QuestionText.text = "Bravo ! Vous avez fini toutes les questions de ce niveau";
            // Cacher les boutons de réponse et le bouton de validation
            Answer_1.gameObject.SetActive(false);
            Answer_2.gameObject.SetActive(false);
            Answer_3.gameObject.SetActive(false);
            Answer_4.gameObject.SetActive(false);
            validationButton.SetActive(false);
            ReturnButton.gameObject.SetActive(true);
            return;
        }

        // Sélectionner une ligne aléatoire parmi les données filtrées
        int randomIndex = Random.Range(0, filteredData.Count);
        string[] randomLine = filteredData[randomIndex];

        // Sauvegarder l'index de la bonne réponse
        bonneReponseIndex = int.Parse(randomLine[5]);

        // Stocker le total des bonnes réponses pour cette question spécifique
        totbonnesrep = int.Parse(randomLine[7]); // Assumant que le total des bonnes réponses est à l'index 7

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
            // Réponse correcte
            // Enregistrer les données mises à jour dans le fichier CSV
            SaveCSV();

            // Si la réponse est correcte, incrémentez le score total des bonnes réponses
            totbonnesrep++;

            correctionText.text = "Bravo !";
            Debug.Log("Bonne réponse ! Nombre total de bonnes réponses : " + totbonnesrep);
            correctionButton.GetComponent<Image>().color = Color.green; // Change la couleur du bouton correctionButton en vert

            // Afficher la bonne réponse en vert
            switch (bonneReponseIndex)
            {
                case 1:
                    Answer_1.GetComponent<Image>().color = Color.green;
                    break;
                case 2:
                    Answer_2.GetComponent<Image>().color = Color.green;
                    break;
                case 3:
                    Answer_3.GetComponent<Image>().color = Color.green;
                    break;
                case 4:
                    Answer_4.GetComponent<Image>().color = Color.green;
                    break;
            }
            }
            else
            {
                // Réponse incorrecte
                correctionText.text = "Dommage...";
                Debug.Log("Mauvaise réponse !");
                // Change la couleur du bouton cliqué en rouge
                switch (selectedIndex)
                {
                    case 1:
                        Answer_1.GetComponent<Image>().color = Color.red;
                        break;
                    case 2:
                        Answer_2.GetComponent<Image>().color = Color.red;
                        break;
                    case 3:
                        Answer_3.GetComponent<Image>().color = Color.red;
                        break;
                    case 4:
                        Answer_4.GetComponent<Image>().color = Color.red;
                        break;
                }
                // Afficher la bonne réponse en vert
                switch (bonneReponseIndex)
                {
                    case 1:
                        Answer_1.GetComponent<Image>().color = Color.green;
                        break;
                    case 2:
                        Answer_2.GetComponent<Image>().color = Color.green;
                        break;
                    case 3:
                        Answer_3.GetComponent<Image>().color = Color.green;
                        break;
                    case 4:
                        Answer_4.GetComponent<Image>().color = Color.green;
                        break;
                }
                correctionButton.GetComponent<Image>().color = Color.red; // Change la couleur du bouton correctionButton en rouge
            }

            correctionButton.SetActive(true); // Afficher le bouton Correction

    }


    public void DisplayNewQuestion()
    {
        // Réinitialiser la couleur des boutons à leur couleur normale
        ResetButtonColors();

        correctionButton.SetActive(false); // Cacher le bouton Correction
        DisplayRandomQuestion(); // Afficher une nouvelle question
    } 
    private void ResetButtonColors()
    {
         // Réinitialiser la couleur de tous les boutons à leur couleur normale
        Answer_1.GetComponent<Image>().color = Color.white;
        Answer_2.GetComponent<Image>().color = Color.white;
        Answer_3.GetComponent<Image>().color = Color.white;
        Answer_4.GetComponent<Image>().color = Color.white;
    }

    void RetournerAuMenu()
    {
        SceneManager.LoadScene("Menu");
        LoadPlayButtonScript();
    }

    void LoadPlayButtonScript()
    {
        GameObject playButtonObject = GameObject.Find("Play_button"); // Assurez-vous que le nom est correct
        if (playButtonObject != null)
        {
            Play_button playButtonScript = playButtonObject.GetComponent<Play_button>();
            if (playButtonScript != null)
            {
                // Chargez votre script Play_button ici
            }
            else
            {
                Debug.LogWarning("Script Play_button introuvable sur l'objet Play_button.");
            }
        }
        else
        {
            Debug.LogWarning("Objet Play_button introuvable dans la scène.");
        }
    }

    void SaveCSV()
    {
        string filePath =Path.Combine(Application.dataPath, "Regles_formatclassique.csv");

        // Vérifier si le fichier existe
        if (!File.Exists(filePath))
        {
            Debug.Log("Fichier CSV non trouvé à l'emplacement: " + filePath);
            return;
        }

        // Parcourir chaque ligne du fichier CSV
        for (int i = 0; i < csvData.Count; i++)
        {
            // Vérifier si la question correspond à celle actuellement affichée
            if (csvData[i][0] == QuestionText.text) // Assumant que la question est dans la première colonne
            {
                // Mettre à jour le total des bonnes réponses pour cette question spécifique
                int currentTotal = int.Parse(csvData[i][7]); // Récupérer le total actuel des bonnes réponses
                currentTotal++; // Incrémenter le total
                csvData[i][7] = currentTotal.ToString(); // Mettre à jour le total des bonnes réponses dans la colonne correspondante

                // Réécrire le fichier CSV avec les données mises à jour
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (string[] rowData in csvData)
                    {
                        writer.WriteLine(string.Join(",", rowData));
                    }
                }

                Debug.Log("Données sauvegardées dans le fichier CSV avec succès.");
                return; // Sortir de la boucle une fois que la question a été trouvée et mise à jour
            }
        }

        // Afficher un message d'erreur si la question actuelle n'a pas été trouvée dans le fichier CSV
        Debug.Log("La question actuelle n'a pas été trouvée dans le fichier CSV.");
    }


}