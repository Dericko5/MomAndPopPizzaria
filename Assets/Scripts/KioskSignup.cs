using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KioskSignup : MonoBehaviour
{
    public TMP_InputField emailphone;
    public TMP_InputField emailphoneConfirm;
    public TMP_InputField password;
    public TMP_InputField passwordConfirm;
    
    public void Signup()
    {
        //Signup logic here
    }
    public void ToLogin()
    {
        SceneManager.LoadSceneAsync("Kiosk");
    }
}
