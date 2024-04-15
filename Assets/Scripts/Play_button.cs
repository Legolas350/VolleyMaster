using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class Play_button : MonoBehaviour
{
    public Button playButton;
    public Button reglesButton;
    public Button postesButton;
    public Button jeuButton;
    public Button techniqueButton;
    public Image netImage;
    public Button[] Regles_nv_buttons;
    public static int niveau = 6;
    public int nbofQuestion = 0;

    public int totalCorrectAnswers = 0;
    private List<string[]> csvData = new List<string[]>();
    public float percentage;
    public TMP_Text[] Slider_text;
    
    // Déclaration des sliders
    public Slider[] sliders;

    public Button PNNv1_button;
    public Button PNNv2_button;

    public Button Logo;
    public TextMeshProUGUI Location_text;
    public Button Location;

    void Start()
    {
        // Si les boutons ne sont pas référencés dans l'inspecteur, on les trouve automatiquement
        if (playButton == null)
            playButton = GameObject.Find("Play_button").GetComponent<Button>();

        if (reglesButton == null)
        {
            GameObject reglesButtonGameObject = GameObject.Find("Regles_button");
            if (reglesButtonGameObject != null)
            {
                reglesButton = reglesButtonGameObject.GetComponent<Button>();
                if (reglesButton == null)
                {
                    UnityEngine.Debug.LogError("Le composant Button n'a pas été trouvé sur le GameObject 'Regles_button'.");
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Le GameObject 'Regles_button' n'a pas été trouvé dans la scène.");
            }
        }

        if (postesButton == null)
        {
            GameObject postesButtonGameObject = GameObject.Find("Postes_button");
            if (postesButtonGameObject != null)
            {
                postesButton = postesButtonGameObject.GetComponent<Button>();
                if (postesButton == null)
                {
                    UnityEngine.Debug.LogError("Le composant Button n'a pas été trouvé sur le GameObject 'Postes_button'.");
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Le GameObject 'Postes_button' n'a pas été trouvé dans la scène.");
            }
        }

        if (jeuButton == null)
        {
            GameObject jeuButtonGameObject = GameObject.Find("Jeu_button");
            if (jeuButtonGameObject != null)
            {
                jeuButton = jeuButtonGameObject.GetComponent<Button>();
                if (jeuButton == null)
                {
                    UnityEngine.Debug.LogError("Le composant Button n'a pas été trouvé sur le GameObject 'Jeu_button'.");
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Le GameObject 'Jeu_button' n'a pas été trouvé dans la scène.");
            }
        }

        if (techniqueButton == null)
        {
            GameObject techniqueButtonGameObject = GameObject.Find("Technique_button");
            if (techniqueButtonGameObject != null)
            {
                techniqueButton = techniqueButtonGameObject.GetComponent<Button>();
                if (techniqueButton == null)
                {
                    UnityEngine.Debug.LogError("Le composant Button n'a pas été trouvé sur le GameObject 'Technique_button'.");
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Le GameObject 'Technique_button' n'a pas été trouvé dans la scène.");
            }
        }

        if (netImage == null)
        {
            GameObject netImageGameObject = GameObject.Find("NetImage");
            if (netImageGameObject != null)
            {
                netImage = netImageGameObject.GetComponent<Image>();
                if (netImage == null)
                {
                    UnityEngine.Debug.LogError("Le composant Image n'a pas été trouvé sur le GameObject 'NetImage'.");
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Le GameObject 'NetImage' n'a pas été trouvé dans la scène.");
            }
            
        }

        if (Regles_nv_buttons == null || Regles_nv_buttons.Length == 0)
        {
            Regles_nv_buttons = new Button[6]; // Créer un tableau pour stocker les boutons de niveau

            for (int i = 0; i < 6; i++)
            {
                GameObject reglesNvGameObject = GameObject.Find("RNv" + (i + 1) + "_button");
                if (reglesNvGameObject != null)
                {
                    Button reglesNvButton = reglesNvGameObject.GetComponent<Button>();
                    if (reglesNvButton != null)
                    {   
                        reglesNvButton.enabled = false;
                        Regles_nv_buttons[i] = reglesNvButton;
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("Le composant Button n'a pas été trouvé sur le GameObject 'Rnv" + (i + 1) + "_button'.");
                    }
                }
                else
                {
                    UnityEngine.Debug.LogError("Le GameObject 'Rnv" + (i + 1) + "_button' n'a pas été trouvé dans la scène.");
                }
            }

                // Désactiver les pourcentages au début du jeu
            foreach (TMP_Text textComponent in Slider_text)
            {
                textComponent.gameObject.SetActive(false);
            }    

            Location.gameObject.SetActive(false);
        }


        // On attache une fonction au clic sur le bouton "Play"
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayButtonClick);
        else
            UnityEngine.Debug.LogError("Le GameObject 'Play_button' n'a pas été trouvé dans la scène.");

        // Ajouter des écouteurs de clic pour chaque bouton
        if (reglesButton != null)
            reglesButton.onClick.AddListener(OnReglesButtonClick);

        if (postesButton != null)
            postesButton.onClick.AddListener(OnPostesButtonClick);

        if (jeuButton != null)
            jeuButton.onClick.AddListener(OnJeuButtonClick);

        if (techniqueButton != null)
            techniqueButton.onClick.AddListener(OnTechniqueButtonClick);

        if (PNNv1_button != null)
            PNNv1_button.onClick.AddListener(OnCoursButtonClick);

        if (PNNv2_button != null)
            PNNv2_button.onClick.AddListener(OnExercicesButtonClick);

        // Ajouter des écouteurs de clic pour chaque bouton de niveau
        foreach(Button button in Regles_nv_buttons)
        {
            button.onClick.AddListener(OnReglesNvButtonClick);
        }

        // Charger le CSV
        LoadCSV(Path.Combine(Application.dataPath, "Regles_formatclassique.csv"));
    
        // Récupérer les références des sliders
        sliders = new Slider[6]; // 6 niveaux

        // Activer temporairement les GameObjects des sliders pour les référencer
        for (int i = 0; i < 6; i++)
        {
            GameObject sliderGameObject = GameObject.Find("Slider" + (i + 1));
            if (sliderGameObject != null)
            {
                sliders[i] = sliderGameObject.GetComponent<Slider>();
                if (sliders[i] == null)
                {
                    UnityEngine.Debug.LogError("Le composant Slider n'a pas été trouvé sur le GameObject 'Slider" + (i + 1) + "'.");
                }
                else
                {
                    // Activer le GameObject du slider pour pouvoir y accéder
                    sliderGameObject.SetActive(true);
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Le GameObject 'Slider" + (i + 1) + "' n'a pas été trouvé dans la scène.");
            }
        }

        // Après l'initialisation des sliders, désactivez-les à nouveau
        for (int i = 0; i < 6; i++)
        {
            GameObject sliderGameObject = GameObject.Find("Slider" + (i + 1));
            if (sliderGameObject != null)
            {
                sliderGameObject.SetActive(false);
            }
        }

        if (Slider_text == null || Slider_text.Length == 0)
        {
            Slider_text = new TMP_Text[6]; // Créer un tableau pour stocker les textes des de niveau

            for (int i = 0; i < 6; i++)
            {
                GameObject Slider_text_nvobject = GameObject.Find("Pourcent" + (i + 1));
                if (Slider_text_nvobject != null)
                {
                    TMP_Text Pourcentnvtext = Slider_text_nvobject.GetComponent<TMP_Text>();
                    if (Pourcentnvtext != null)
                    {   
                        Pourcentnvtext.enabled = false;
                        Slider_text[i] = Pourcentnvtext;
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("Le composant Text slider n'a pas été trouvé sur le GameObject 'Pourcent" + (i + 1));
                    }
                }
                else
                {
                    UnityEngine.Debug.LogError("Le GameObject 'Pourcent" + (i + 1) + "n a pas été trouvé dans la scène.");
                }
            }
        }

        foreach (TMP_Text textComponent in Slider_text)
        {
            textComponent.gameObject.SetActive(false);
        }

        // Récupère le bouton "Logo" et ajoute un écouteur pour son clic
        Button logoButton = GameObject.Find("Logo").GetComponent<Button>();
        logoButton.onClick.AddListener(RetournerAuMenu);
    }

    void OnPlayButtonClick()
    {
        // On désactive le bouton "Play"
        if (playButton != null)
            playButton.gameObject.SetActive(false);

        // On active les boutons de menu
        if (reglesButton != null)
            reglesButton.gameObject.SetActive(true);
        if (postesButton != null)
            postesButton.gameObject.SetActive(true);
        if (jeuButton != null)
            jeuButton.gameObject.SetActive(true);
        if (techniqueButton != null)
            techniqueButton.gameObject.SetActive(true);

        // Désactiver l'image "Net"
        if (netImage != null)
            netImage.gameObject.SetActive(false);

  

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

    // Fonction d'écouteur de clic pour le bouton "Règles"
    void OnReglesButtonClick()
    {
        Location.gameObject.SetActive(true);
        // Mise à jour du texte de Location_text 
        Location_text.text = "Section Règles";

        // On desactive les boutons de menu
        if (reglesButton != null)
            reglesButton.gameObject.SetActive(false);
        if (postesButton != null)
            postesButton.gameObject.SetActive(false);
        if (jeuButton != null)
            jeuButton.gameObject.SetActive(false);
        if (techniqueButton != null)
            techniqueButton.gameObject.SetActive(false);

        //Activer les sliders de niveau
        foreach(Slider slider in sliders)
        {
            slider.gameObject.SetActive(true);
        }

        // Activer les pourcentages
        foreach (TMP_Text textComponent in Slider_text)
        {
            textComponent.gameObject.SetActive(true);
        }
        
        // Désactiver les boutons de niveau pour les niveaux avec un "totbonnesrep" supérieur à 3
        DisableButtonsForHighTotbonnesrep();

        int niveau = Play_button.niveau; // Niveau spécifié

        // Mettre à jour les sliders avec le pourcentage de questions à plus de 3 totbonnesrep
        UpdateSliders();
    }

    void DisableButtonsForHighTotbonnesrep()
    {
        // Parcourir les données CSV pour vérifier les "totbonnesrep" pour chaque niveau
        for (int i = 0; i < 6; i++) // Supposant que vous avez 6 niveaux
        {
            int niveauToCheck = i + 1; // Niveau commence à partir de 1
            int totalCorrectAnswers = GetTotalCorrectAnswersForLevel(niveauToCheck);
    
            // Si le total des bonnes réponses dépasse 3, désactiver le bouton de niveau correspondant
            if (totalCorrectAnswers < (2 * nbofQuestion))
            {
                Regles_nv_buttons[i].gameObject.SetActive(true);
            }
            else
            {   
                // Rendre le bouton non cliquable et le griser
                Regles_nv_buttons[i].gameObject.SetActive(true);
                Regles_nv_buttons[i].interactable = false;
                Color disabledColor = new Color(0.7f, 0.7f, 0.7f, 1f); // Couleur grise
                Regles_nv_buttons[i].image.color = disabledColor; // Changer la couleur de l'image
            }
        }
    }

    void UpdateSliders()
    {
        percentage = 0;

        for (int i = 0; i < 6; i++)
        {
            int niveauToCheck = i + 1;
            int totalCorrectAnswers = GetTotalCorrectAnswersForLevel(niveauToCheck);
            int totalQuestions = GetTotalQuestionsForLevel(niveauToCheck);
            if (totalQuestions == 0)
                {
                    percentage = 1;
                    Debug.LogError("je suis dans if de " + i);
                }
            else 
                {
                    percentage = (int)(((float)totalCorrectAnswers / (totalQuestions * 2)) * 100);
                    Debug.LogError("je suis dans else de " + i);

                }

            Debug.LogError("nv ; " + (i+1) + ", pourcentage:" + percentage + ", totalCorrectAnswers" + totalCorrectAnswers + ", totalQuestions" + totalQuestions);

            // Mettre à jour la valeur du slider
            if (sliders[i] != null)
                {
                sliders[i].value = (percentage/100);
                // Accéder au composant Text du texte "Pourcent" associé et mettre à jour son texte
                Slider_text[i].text = percentage.ToString("F0") + "%";

            }
        }
    }

    int GetTotalCorrectAnswersForLevel(int niveauToCheck)
    {
        totalCorrectAnswers = 0;
        nbofQuestion = 0;
        // Filtrer les lignes du CSV pour le niveau spécifié
        List<string[]> filteredData = csvData.FindAll(line =>
        {
            int parsedLevel;
            if (int.TryParse(line[6], out parsedLevel))
            {
                return parsedLevel == (niveauToCheck);
            }
            else
            {
                return false;
            }
        });

        // Calculer le nombre total de bonnes réponses pour ce niveau
        foreach (var line in filteredData)
        {
            int questionTotalCorrectAnswers;
            if (int.TryParse(line[7], out questionTotalCorrectAnswers))
            {
                totalCorrectAnswers += questionTotalCorrectAnswers;
                nbofQuestion++;
            }
        }

        return totalCorrectAnswers;
    }

    int GetTotalQuestionsForLevel(int niveauToCheck)
    {
        int totalQuestions = 0;

        // Filtrer les lignes du CSV pour le niveau spécifié
        List<string[]> filteredData = csvData.FindAll(line =>
        {
            int parsedLevel;
            if (int.TryParse(line[6], out parsedLevel))
            {
                return parsedLevel == (niveauToCheck);
            }
            else
            {
                return false;
            }
        });

        // Calculer le nombre total de questions pour ce niveau
        totalQuestions = filteredData.Count;

        return totalQuestions;
    }

    void OnReglesNvButtonClick()
    {

        // Récupérer le bouton qui a été cliqué
        Button clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        // Récupérer le dernier caractère du nom du bouton pour obtenir le niveau
        niveau = int.Parse(clickedButton.name.Substring(3, 1)); // "Rnv1_button" => extrait "1"

        // Charger la scène Regles
        SceneManager.LoadScene("Regles");
    }

//////////////////////////////////////////////////////////////////////////////////////////

    // Fonction d'écouteur de clic pour le bouton "Postes"
    void OnPostesButtonClick()
    {
        Location.gameObject.SetActive(true);

        // On desactive les boutons de menu
        if (reglesButton != null)
            reglesButton.gameObject.SetActive(false);
        if (postesButton != null)
            postesButton.gameObject.SetActive(false);
        if (jeuButton != null)
            jeuButton.gameObject.SetActive(false);
        if (techniqueButton != null)
            techniqueButton.gameObject.SetActive(false);

        // Activer les boutons pour les cours et les exercices

        PNNv1_button.gameObject.SetActive(true);
        PNNv2_button.gameObject.SetActive(true);

        // Mise à jour du texte de Location_text 
        Location_text.text = "Section Récéption";

    }
    
    void OnCoursButtonClick()
    {
        // Charger la scène des cours
        SceneManager.LoadScene("CoursPostes");
    }

    void OnExercicesButtonClick()
    {
        // Charger la scène des exercices
        SceneManager.LoadScene("Drag&Drop");
    }

    // Fonction d'écouteur de clic pour le bouton "Jeu"
    void OnJeuButtonClick()
    {
        Location.gameObject.SetActive(true);
        // Mise à jour du texte de Location_text 
        Location_text.text = "Section Défense";
        SceneManager.LoadScene("Jeu");
    }

    // Fonction d'écouteur de clic pour le bouton "Technique"
    void OnTechniqueButtonClick()
    {
        Location.gameObject.SetActive(true);
        // Mise à jour du texte de Location_text 
        Location_text.text = "Section Technique";
        SceneManager.LoadScene("Technique");
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
}

