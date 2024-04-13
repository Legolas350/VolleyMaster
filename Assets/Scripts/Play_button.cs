using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;


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
                GameObject reglesNvGameObject = GameObject.Find("Rnv" + (i + 1) + "_button");
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

        // Ajouter des écouteurs de clic pour chaque bouton de niveau
        foreach(Button button in Regles_nv_buttons)
        {
            button.onClick.AddListener(OnReglesNvButtonClick);
        }

        // Charger le CSV
        LoadCSV(@"C:\Users\lilou\Documents\CSNumVi\Impliquer\VolleyMaster\Assets\Regles_formatclassique.csv");
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
        // On desactive les boutons de menu
        if (reglesButton != null)
            reglesButton.gameObject.SetActive(false);
        if (postesButton != null)
            postesButton.gameObject.SetActive(false);
        if (jeuButton != null)
            jeuButton.gameObject.SetActive(false);
        if (techniqueButton != null)
            techniqueButton.gameObject.SetActive(false);

       // Activer les boutons de niveau
        //foreach(Button button in Regles_nv_buttons)
        //{
         //   button.gameObject.SetActive(true);
        //}
        
        // Désactiver les boutons de niveau pour les niveaux avec un "totbonnesrep" supérieur à 3
        DisableButtonsForHighTotbonnesrep();

        int niveau = Play_button.niveau; // Niveau spécifié
    }

    void DisableButtonsForHighTotbonnesrep()
    {
        // Parcourir les données CSV pour vérifier les "totbonnesrep" pour chaque niveau
        for (int i = 0; i < 6; i++) // Supposant que vous avez 6 niveaux
        {
            int niveauToCheck = i + 1; // Niveau commence à partir de 1
            int totalCorrectAnswers = GetTotalCorrectAnswersForLevel(niveauToCheck);
    
            // Si le total des bonnes réponses dépasse 3, désactiver le bouton de niveau correspondant
            if (totalCorrectAnswers < (3 * nbofQuestion))
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
                return parsedLevel == niveauToCheck;
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


    public void OnReglesNvButtonClick()
    {
        // Récupérer le bouton qui a été cliqué
        Button clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        // Récupérer le dernier caractère du nom du bouton pour obtenir le niveau
        niveau = int.Parse(clickedButton.name.Substring(3, 1)); // "Rnv1_button" => extrait "1"

        // Charger la scène Regles
        SceneManager.LoadScene("Regles");
    }

    // Fonction d'écouteur de clic pour le bouton "Postes"
    void OnPostesButtonClick()
    {
        SceneManager.LoadScene("Postes");
    }
    

    // Fonction d'écouteur de clic pour le bouton "Jeu"
    void OnJeuButtonClick()
    {
        SceneManager.LoadScene("Jeu");
    }

    // Fonction d'écouteur de clic pour le bouton "Technique"
    void OnTechniqueButtonClick()
    {
        SceneManager.LoadScene("Technique");
    }
}
