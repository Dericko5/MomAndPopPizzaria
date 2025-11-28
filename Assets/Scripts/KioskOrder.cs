using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;



public class KioskOrder : MonoBehaviour
{
    public GameObject OrderItems;
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
    public GameObject PaymentComponents;
    public Scrollbar OrderScrollBar;
    
    public Vector3 Itemposition = new Vector3(-29, 1, -67.5f);
    long itemPrice = 0;
    string itemName = "";
    public static int itemQuantity = 1;
    public int itemOrderType = 0; //1=full pizza, 2=half pizza, 3=other item
    public int lastOrderType = 0;
    public long orderSubtotal = 0;
    public long orderTotal = 0;
    public double taxRate = .06; //MAY NEED TO CHECK THIS

    string[] orderItems = new string[50]; // 50 is arbitrary cap on number of unique items in order

    Dictionary<string, double> sizePrices = new Dictionary<string, double>()
    {
        {"Small", 5.00},
        {"Medium", 7.00},
        {"Large", 9.00},
        {"X-Large", 11.00}
    };


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //ui elements that should display on start
        PizzaButton.SetActive(true);
        DrinkButton.SetActive(true);
        SidesButton.SetActive(true);
        ButtonBGS.SetActive(true);
        SidesWindow.SetActive(false);
        DrinkWindow.SetActive(false);
        PizzaWindow.SetActive(false);

        StaticValues.OrderID += 1; //increment order ID for new order
        CustomerName.text = StaticValues.CustomerName;
        OrderDetails.text = "Order #" + StaticValues.OrderID.ToString() + "- ";
        if (StaticValues.Delivery)
        {
            OrderDetails.text += "Delivery";
        }
        else
        {
            OrderDetails.text += "Takeout";
        }
        tax.text = "Tax(6%):\t       \t    $0.00";
        subtotal.text = "Subtotal:\t     \t   $0.00";
        total.text = "Total:		        $0.00";
        Debug.Log("Total string length = " + total.text.Length);



    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    public void MenuPopup()
    {
        PizzaButton.SetActive(false);        
        DrinkButton.SetActive(false);
        SidesButton.SetActive(false);
        ButtonBGS.SetActive(false);
        
    }
    public void ShowPizzaPopup()
    {
        PizzaWindow.SetActive(true);
    }
    public void ShowDrinkPopup()
    {
        DrinkWindow.SetActive(true);
    }

    public void ShowSidePopup()
    {
        SidesWindow.SetActive(true);
    }
    public void HideMenuPopup()
    {
        PizzaButton.SetActive(true);
        DrinkButton.SetActive(true);
        SidesButton.SetActive(true);
        ButtonBGS.SetActive(true);
        SidesWindow.SetActive(false);
        DrinkWindow.SetActive(false);
        PizzaWindow.SetActive(false);
    }

    public void AddFullPizzaOrder()
    {
        string itemSize = "Small"; //default size
        
        //depending on what the previous item was, adjust next item's position
        switch (lastOrderType)
        {
            case 0:
                break;
            case 1: // last item was full pizza
            Itemposition.y -= 23;
            break;
        case 2: // last item was half pizza
            Itemposition.y -= 35;
            break;
        case 3: // last item was other item
            Itemposition.y -= 5;
            break;
        }

        
        
        Transform Toppings = PizzaWindow.transform.Find("Toppings(full)");
        
        
        

        GameObject FullPizza = Instantiate(FullPizzaPrefab, Itemposition, Quaternion.identity);
        FullPizza.transform.SetParent(OrderItems.transform, true);
        FullPizza.transform.localScale = new Vector3(0.8428124f, 0.8428124f, 0.8428124f);

        for (int i = 0; i < itemQuantity - 1; i++)
        {
            itemPrice += itemPrice;
            itemQuantity += 1;
        }
        string iQString = itemQuantity.ToString();
        string iPString = itemPrice.ToString();

        // Now, set values of children depending on selected values from pizza customization
        Transform mainLineOrder = FullPizza.transform.Find("MainLineOrder");
        string itemtext = iQString + "x " + itemSize + " Pizza -       $" + iPString;
        mainLineOrder.GetComponent<TextMeshProUGUI>().text = itemtext;



        lastOrderType = 1; //full pizza
    }

    public void AddHalfPizzaOrder()
    {
        switch (lastOrderType)
        {
            case 0:
                break;
            case 1: // last item was full pizza
                Itemposition.y -= 23;
                break;
            case 2: // last item was half pizza
                Itemposition.y -= 35;
                break;
            case 3: // last item was other item
                Itemposition.y -= 5;
                break;

        }

        Transform Toppings1 = PizzaWindow.transform.Find("Toppings(half1)");
        Transform Toppings2 = PizzaWindow.transform.Find("Toppings(half2)");

        GameObject HalfPizza = Instantiate(HalfPizzaPrefab, Itemposition, Quaternion.identity);
        HalfPizza.transform.SetParent(OrderItems.transform, true);
        HalfPizza.transform.localScale = new Vector3(0.8428124f, 0.8428124f, 0.8428124f);
        lastOrderType = 2; //half pizza
    }

    public void AddDrinkOrder()
    {
        switch (lastOrderType)
        {
            case 0:
                break;
            case 1: // last item was full pizza
                Itemposition.y -= 23;
                break;
            case 2: // last item was half pizza
                Itemposition.y -= 35;
                break;
            case 3: // last item was other item
                Itemposition.y -= 5;
                break;

        }
        GameObject Drink = Instantiate(OtherItemPrefab, Itemposition, Quaternion.identity);
        Drink.transform.SetParent(OrderItems.transform, true);
        Drink.transform.localScale = new Vector3(0.8428124f, 0.8428124f, 0.8428124f);
        lastOrderType = 3; //other item
    }

    public void AddSideOrder()
    {
        switch (lastOrderType)
        {
            case 0:
                break;
            case 1: // last item was full pizza
                Itemposition.y -= 23;
                break;
            case 2: // last item was half pizza
                Itemposition.y -= 35;
                break;
            case 3: // last item was other item
                Itemposition.y -= 5;
                break;

        }
        GameObject Side = Instantiate(OtherItemPrefab, Itemposition, Quaternion.identity);
        Side.transform.SetParent(OrderItems.transform, true);
        Side.transform.localScale = new Vector3(0.8428124f, 0.8428124f, 0.8428124f);
        lastOrderType = 3; //other item
    }

    public void ToPayment()
    {
        //hides menu buttons and shows payment components
        PizzaButton.SetActive(false);
        DrinkButton.SetActive(false);
        SidesButton.SetActive(false);
        ButtonBGS.SetActive(false);
        SidesWindow.SetActive(false);
        DrinkWindow.SetActive(false);
        PizzaWindow.SetActive(false);
        PaymentComponents.SetActive(true);
    }

    public void CancelOrder()
    {
        //decrease order ID by 1 to not skip numbers
        StaticValues.OrderID -= 1;
        KioskLoginUi.Signout();
    }
}
