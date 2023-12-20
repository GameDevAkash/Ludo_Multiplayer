using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    [SerializeField] GameObject SignInPanel;
    [SerializeField] GameObject SignUpPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SignUp()
    {
        SignInPanel.SetActive(false);
        SignUpPanel.SetActive(true);
    }

    public void SignIn()
    {
        SignInPanel.SetActive(true);
        SignUpPanel.SetActive(false);
    }

    public void SignInSuccess()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
