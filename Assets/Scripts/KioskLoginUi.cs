using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class KioskLoginUi : MonoBehaviour
{
    public GameObject LoginUi; //ui components to be enabled/disabled
    public GameObject DeliveryUi;
    public TMP_InputField username; //input fields for username and password
    public TMP_InputField password;
    private string guestuser = "guest";
    private string guestpass = "12345";
    public bool isGuest = false;
    public TextMeshProUGUI Greeting;

    private void Start()
    {
        //First screen should be login screen, so display login elements when scene starts
        LoginUi.SetActive(true);
        DeliveryUi.SetActive(false);

        StaticValues.User = ""; // clear static values
        StaticValues.Pass = "";

        isGuest = false; //reset guest status on start
    }
    public void SetGuestTrue()
    {
        isGuest = true;
    }

    public void ToSignUp()
    {
        StaticValues.User = ""; // clear static values and load signup scene
        StaticValues.Pass = "";
        SceneManager.LoadSceneAsync("KioskSignup");
    }
    public void ToLogin()
    {
        SceneManager.LoadSceneAsync("Kiosk");
    }
    public void ToDelivery()
    {
        //First, store username and password in static values for use in next scene(s)
        if (isGuest)
        {
            StaticValues.CustomerName = "Guest User";
            StaticValues.User = guestuser;
            StaticValues.Pass = guestpass;
        }
        else
        {
            //ALSO NEEDS TO SET CUSTOMER NAME FROM DATABASE
            StaticValues.User = username.text; 
            StaticValues.Pass = password.text; 
        }
        // toggle ui elements
        LoginUi.SetActive(false);
        DeliveryUi.SetActive(true);
    }
    public void ToOrder()
    {
        SceneManager.LoadSceneAsync("KioskOrder"); //Loads order scene
    }

    public void SetDeliveryTrue()
    {
        StaticValues.Delivery = true;
    }
    public void SetDeliveryFalse()
    {
        StaticValues.Delivery = false;
    }

    public void Signout()
    {
        StaticValues.User = ""; //Reset static values and reload login scene
        StaticValues.Pass = "";
        StaticValues.CustomerName = "Guest User";
        SceneManager.LoadSceneAsync("Kiosk");


    }
    public void SetLoginValues()
    {
        //UNTIL LOGIN() IS DONE, SETTING USER AS CUSTOMER NAME
        if (!isGuest)
        {
            StaticValues.CustomerName = username.text;
            StaticValues.Pass = password.text;
            StaticValues.User = username.text;
        }
    }

    // this one may be up to you guys to do!
    public void Login()
    {
        //login validation code here



        Greeting.text = "Ciao, " + StaticValues.CustomerName + "!"; //set greeting text
        ToDelivery(); //proceed to delivery/takeout selection

    }
}
