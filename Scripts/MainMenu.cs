using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    private GameObject MainMenuAccountInput;
    private GameObject MainMenuAccount;
    private GameObject MainMenuBtnArea;
    private void Start()
    {
        MainMenuAccountInput = GameObject.FindGameObjectWithTag("MainMenuAccountInput");
        MainMenuAccountInput.SetActive(false);

        MainMenuAccount = GameObject.FindGameObjectWithTag("MainMenuAccount");

        MainMenuBtnArea = GameObject.FindGameObjectWithTag("MainMenu");
        MainMenuBtnArea.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    private int loginOrSignup = 0;
    public void Login()
    {
        loginOrSignup = 1;
        MainMenuAccountInput.SetActive(true);
        GameObject.FindGameObjectWithTag("AccountInfoBtn").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Log ind";
    }

    public void Signup()
    {
        loginOrSignup = 2;
        MainMenuAccountInput.SetActive(true);
        GameObject.FindGameObjectWithTag("AccountInfoBtn").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Opret Konto";
    }

    public void Register()
    {
        string username = GameObject.FindGameObjectWithTag("UsernameInput").GetComponent<TMP_InputField>().text;
        string password = GameObject.FindGameObjectWithTag("PasswordInput").GetComponent<TMP_InputField>().text;
        if (loginOrSignup == 1) // "loginOrSignup", værdi 1 er login, og værdi 2 er signup knappen
        {
            StartCoroutine(PostRequest(path: "/login", username: username, password: password));
        }
        else 
        {
            StartCoroutine(PostRequest(path: "/signup", username: username, password: password));
        }
    }

    IEnumerator PostRequest(string path, string username, string password)
    {
        WWWForm data = new WWWForm();
        data.AddField("username", username);
        data.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:5000" + path, data))
        {
            yield return www.SendWebRequest();

            if (www.downloadHandler.text == "Account doesn't exist")
            {
                Debug.Log("Failed");
            }
            else if (www.downloadHandler.text == "Success")
            {
                Debug.Log("Success");
                MainMenuAccount.SetActive(false);
                MainMenuAccountInput.SetActive(false);
                MainMenuBtnArea.SetActive(true);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
}
