using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterUser : MonoBehaviour
{
    public InputField UsernameInput;
    public InputField PasswordInput;
    public InputField ConfirmPasswordInput;
    public Button SubmitButton;
    public Text errorText;
    void Start()
    {
        SubmitButton.onClick.AddListener(() =>
        {
            if (ConfirmPasswordInput.text != PasswordInput.text)
            {
                Debug.Log("Password do not match!");
                errorText.text = "Password do not match!";
            }
            else
            {
                StartCoroutine(Main.Instance.web.RegisterUser(UsernameInput.text, PasswordInput.text, ConfirmPasswordInput.text));
            }
        });
    }
}

