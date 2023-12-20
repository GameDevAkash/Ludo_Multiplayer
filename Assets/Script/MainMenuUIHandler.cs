using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIHandler : MonoBehaviour
{
    [SerializeField] GameObject SettingsPanel;
    [SerializeField] GameObject Main;
    // Start is called before the first frame update
    public void backBtn()
    {
        SceneManager.LoadScene("Login");
    }

    public void quitBtn()
    {
        Application.Quit();
    }

    public void settingsBtn()
    {
        Main.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    public void signOut()
    {
        SceneManager.LoadScene("Login");
    } 

    public void SettingsBack()
    {
        Main.SetActive(true);
        SettingsPanel.SetActive(false);
    }
}
