using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;



public class KioskOrder : MonoBehaviour
{
    public TextMeshProUGUI CustomerName;
    public TextMeshProUGUI OrderDetails;
    public TextMeshProUGUI total;
    public TextMeshProUGUI subtotal;
    public TextMeshProUGUI tax;
    public GameObject FullPizzaPrefab;
    public GameObject HalfPizzaPrefab;
    public GameObject OtherItemPrefab;
    public GameObject PizzaButton;
    public GameObject DrinkButton;
    public GameObject SidesButton;
    public GameObject ButtonBGS;
    public GameObject PizzaWindow;
    public GameObject DrinkWindow;
    public GameObject SidesWindow;
    public int orderSubtotal = 0;
    public double taxRate = .06; //MAY NEED TO CHECK THIS

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MenuPopup()
    {
        PizzaButton.SetActive(false);        
        DrinkButton.SetActive(false);
        SidesButton.SetActive(false);
        ButtonBGS.SetActive(false);

    }

    void AddFullPizzaOrder()
    {

    }

    void AddHalfPizzaOrder()
    {
    }

    void AddOtherItemOrder()
    {
    }
}
