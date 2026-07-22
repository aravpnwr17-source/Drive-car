using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

/// <summary>
/// Joke Generator using JokeAPI
/// Fetches random jokes from an external API
/// </summary>
public class JokeGenerator : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI jokeDisplay;
    [SerializeField] private TextMeshProUGUI jokeTypeDisplay;
    [SerializeField] private Button generateButton;
    [SerializeField] private Button copyButton;
    [SerializeField] private LoadingSpinner loadingSpinner;

    [Header("API Settings")]
    [SerializeField] private string jokeApiUrl = "https://v2.jokeapi.dev/joke/Any";
    [SerializeField] private float requestTimeout = 10f;

    private string currentJoke = "";
    private string currentJokeType = "";
    private bool isLoading = false;

    [System.Serializable]
    public class JokeResponse
    {
        public bool error;
        public string type;
        public string joke;
        public string setup;
        public string delivery;
        public int id;
    }

    private void Start()
    {
        if (generateButton != null)
            generateButton.onClick.AddListener(GenerateJoke);

        if (copyButton != null)
            copyButton.onClick.AddListener(CopyJokeToClipboard);

        // Generate initial joke
        GenerateJoke();
    }

    /// <summary>
    /// Generate a new joke by calling the API
    /// </summary>
    public void GenerateJoke()
    {
        if (isLoading) return;

        StartCoroutine(FetchJoke());
    }

    /// <summary>
    /// Coroutine to fetch joke from API
    /// </summary>
    private IEnumerator FetchJoke()
    {
        isLoading = true;

        if (loadingSpinner != null)
            loadingSpinner.SetActive(true);

        if (generateButton != null)
            generateButton.interactable = false;

        using (UnityWebRequest request = UnityWebRequest.Get(jokeApiUrl))
        {
            request.timeout = (int)requestTimeout;

            yield return request.SendWebRequest();

            if (loadingSpinner != null)
                loadingSpinner.SetActive(false);

            if (generateButton != null)
                generateButton.interactable = true;

            if (request.result == UnityWebRequest.Result.ConnectionError || 
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                HandleError(request.error);
            }
            else
            {
                string response = request.downloadHandler.text;
                ParseJokeResponse(response);
            }
        }

        isLoading = false;
    }

    /// <summary>
    /// Parse the API response and display the joke
    /// </summary>
    private void ParseJokeResponse(string jsonResponse)
    {
        try
        {
            JokeResponse jokeData = JsonUtility.FromJson<JokeResponse>(jsonResponse);

            if (jokeData.error)
            {
                HandleError("API returned error");
                return;
            }

            currentJokeType = jokeData.type;

            // Handle both single joke and setup/delivery format
            if (jokeData.type == "twopart")
            {
                currentJoke = $"{jokeData.setup}\n\n{jokeData.delivery}";
            }
            else
            {
                currentJoke = jokeData.joke;
            }

            DisplayJoke();
            Debug.Log($"Joke fetched successfully! Type: {currentJokeType}");
        }
        catch (System.Exception e)
        {
            HandleError($"Failed to parse response: {e.Message}");
        }
    }

    /// <summary>
    /// Display the joke on screen
    /// </summary>
    private void DisplayJoke()
    {
        if (jokeDisplay != null)
        {
            jokeDisplay.text = currentJoke;
            jokeDisplay.alpha = 0f;
            StartCoroutine(FadeInJoke());
        }

        if (jokeTypeDisplay != null)
            jokeTypeDisplay.text = $"Type: {currentJokeType}";
    }

    /// <summary>
    /// Fade in animation for joke text
    /// </summary>
    private IEnumerator FadeInJoke()
    {
        float elapsedTime = 0f;
        float fadeDuration = 0.5f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            jokeDisplay.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        jokeDisplay.alpha = 1f;
    }

    /// <summary>
    /// Copy current joke to clipboard
    /// </summary>
    private void CopyJokeToClipboard()
    {
        if (string.IsNullOrEmpty(currentJoke))
            return;

        GUIUtility.systemCopyBuffer = currentJoke;
        Debug.Log("Joke copied to clipboard!");

        if (copyButton != null)
            StartCoroutine(ShowCopyConfirmation());
    }

    /// <summary>
    /// Show visual confirmation when joke is copied
    /// </summary>
    private IEnumerator ShowCopyConfirmation()
    {
        Text buttonText = copyButton.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            string originalText = buttonText.text;
            buttonText.text = "Copied! ✓";
            yield return new WaitForSeconds(2f);
            buttonText.text = originalText;
        }
    }

    /// <summary>
    /// Handle API errors
    /// </summary>
    private void HandleError(string errorMessage)
    {
        currentJoke = $"❌ Error: {errorMessage}\n\nPlease check your internet connection and try again.";
        currentJokeType = "Error";

        DisplayJoke();
        Debug.LogError($"Joke API Error: {errorMessage}");
    }

    /// <summary>
    /// Get current joke text
    /// </summary>
    public string GetCurrentJoke()
    {
        return currentJoke;
    }

    /// <summary>
    /// Get current joke type
    /// </summary>
    public string GetCurrentJokeType()
    {
        return currentJokeType;
    }

    /// <summary>
    /// Check if currently loading
    /// </summary>
    public bool IsLoading()
    {
        return isLoading;
    }
}
