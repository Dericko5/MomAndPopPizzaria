using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KioskSignup : MonoBehaviour
{
    public TMP_InputField emailphone, emailphoneConfirm;
    public TextMeshProUGUI invalidatorText;
    public TMP_InputField password, passwordConfirm;

    public void Signup()
    {
        //Checks if all the boxes are filled
        if (String.IsNullOrEmpty(emailphone.text) || String.IsNullOrEmpty(emailphoneConfirm.text) ||
            String.IsNullOrEmpty(password.text) || String.IsNullOrEmpty(passwordConfirm.text))
        {
            invalidatorText.text = "! Please make sure to fill in all of the boxes.";
        }
        else
        {
            //Checks if the emails and passwords match to set for the Login Screen
            bool emailphoneMatch = emailphone.text == emailphoneConfirm.text;
            bool passwordMatch = password.text == passwordConfirm.text;

            if (!emailphoneMatch || !passwordMatch)
            {
                if (!emailphoneMatch && !passwordMatch)
                {
                    invalidatorText.text = "! Emails and passwords do not match!";
                }
                else if (!emailphoneMatch)
                {
                    invalidatorText.text = "! Emails do not match!";
                }
                else if (!passwordMatch)
                {
                    invalidatorText.text = "! Passwords do not match!";
                }
            }
            else
            {
                //Sets client's username and password to what was entered in the boxes and sends client back to the login screen if all checks succeed
                StaticValues.User = emailphone.text;
                StaticValues.Pass = password.text;
                StaticValues.CustomerName = emailphone.text;
                ToLogin();
            }
        } 
    }
    public void ToLogin()
    {
        SceneManager.LoadSceneAsync("Kiosk");
    }
}
