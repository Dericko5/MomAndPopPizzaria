using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using System.Collections;



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
    public GameObject CancelWindow;
    public GameObject PaymentComponents;
    public GameObject PaymentSuccessful;
    public Scrollbar OrderScrollBar;
    public bool Sauce = true;
    
    public Vector3 Itemposition = new Vector3(-29, 1, -67.5f);
    decimal itemPrice = 0;
    public static int itemQuantity = 1;
    public int itemOrderType = 0; //1=full pizza, 2=half pizza, 3=other item
    public int lastOrderType = 0;
    public decimal orderSubtotal = 0;
    public decimal orderTotal = 0;
    public decimal taxRate = .06m; 
    public string itemSize = "Small"; //default size
    public string DrinkSize = "S.";
    public string PizzaCrust = "Regular"; //default crust
    public string DrinkName = "Pepsi"; //default drink
    public string SideName = "Breadsticks"; //default side

    

    List<string> Order = new List<string>(); //create list to hold order items

    Dictionary<string, decimal> sizePrices = new Dictionary<string, decimal>() //dictionary to hold size prices
    {
        {"Small", 5.00m},
        {"Medium", 7.00m},
        {"Large", 9.00m},
        {"X Large", 11.00m}
    };

    Dictionary<string, decimal> toppingPrices = new Dictionary<string, decimal>() //dictionary to hold extra topping charges
    {
        {"Small", 0.75m},
        {"Medium", 1.00m},
        {"Large", 1.25m},
        {"X Large", 1.50m}
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
        int ID = StaticValues.OrderID;
        if (ID < 100) // order numbers contain a 3 digit format. If <100, add leading zeroes
        {
            OrderDetails.text = "Order #" + "00" + ID.ToString() + "- ";
        }
        else
        {
            OrderDetails.text = "Order #" + ID.ToString() + "- ";
        }

        if (StaticValues.Delivery)
        {
            OrderDetails.text += "Delivery";
        }
        else
        {
            OrderDetails.text += "Takeout";
        }
        tax.text = "Tax(6%):\t       \t    $0.00";
        subtotal.text = "Subtotal:\t     \t    $0.00";
        total.text = "Total:		         $0.00";
        
        






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
        PizzaButton.SetActive(true); //Deactivates windows and reenables the buttons and ui elements
        DrinkButton.SetActive(true);
        SidesButton.SetActive(true);
        ButtonBGS.SetActive(true);
        SidesWindow.SetActive(false);
        DrinkWindow.SetActive(false);
        PizzaWindow.SetActive(false);
    }

    public void SetItemQuantity(TMP_InputField quantityField) //Specifically for setting quantity of pizzas
    {
        int quantity;
        if (int.TryParse(quantityField.text, out quantity)) // take in value from input field, and set value = to what it outputs, or 1
        {
            itemQuantity = quantity;
            
        }
        else
        {
            itemQuantity = 1; //default to 1 if parsing fails
            quantityField.text = "1";
        }
    }

    public void SetDrinkSize(TMP_Dropdown dDropdown) //sets drink size
    {
        DrinkSize = dDropdown.options[dDropdown.value].text;
        DrinkSize = DrinkSize[0] + ".";
        itemPrice = 1.75m;
    }
    public void SetItemSize(TMP_Dropdown sizeDropdown) //sets sizes for pizzas and sets the variable itemPrice equal to what it returns from the dictionary
    {
        
        itemSize = sizeDropdown.options[sizeDropdown.value].text;
        if (itemSize.Split(' ')[0] == "X")
        {
            itemSize = "X Large";
        }
        else
        {
            itemSize = itemSize.Split(' ')[0];
        }
        itemPrice = (decimal)(sizePrices[itemSize]); 
        
    }

    public void SetPizzaCrust(TMP_Dropdown crustDropdown) //based on value of the crust dropdown, sets crust type (Pan, thin, regular)
    {
        PizzaCrust = crustDropdown.options[crustDropdown.value].text;
    }
    public void SetPizzaSauce()
    {
        Sauce = !Sauce; //changes whenever toggle is changed

    }
    public void SetDrink(TMP_Dropdown drinks)
    {
        DrinkName = drinks.options[drinks.value].text; //based on value of the drink dropdown, sets soda type/flavor
    }

    public void SetSide(TMP_Dropdown sides)
    {
        SideName = sides.options[sides.value].text; //based on value of the sides dropdown, sets side type
    }

    public void AddFullPizzaOrder() // Adds a pizza to the order with uniform toppings (and uses a specific prefab)
    {

        itemPrice = (decimal)(sizePrices[itemSize]);

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

        //Calculate item price up based on toppings
        if (ToggleHandling.ToppingsListFull.Count > 2)
        {
            decimal extraToppings = ToggleHandling.ToppingsListFull.Count - 2;
            itemPrice += toppingPrices[itemSize] * extraToppings;
        }






        GameObject FullPizza = Instantiate(FullPizzaPrefab, Itemposition, Quaternion.identity); 
        FullPizza.transform.SetParent(OrderItems.transform, true);
        FullPizza.transform.localScale = new Vector3(0.8428124f, 0.8428124f, 0.8428124f); // create new instance of prefab, format, and group with other order items to be toggled later

        for (int i = 0; i < itemQuantity -1; i++) //calculate total price based on quantity
        {
            itemPrice += itemPrice;
        }
        string iQString = itemQuantity.ToString();
        string iPString = itemPrice.ToString();
     
        // Now, set values of children (Ui elements) depending on selected values from pizza customization
        //Main line (number of item, size, price
        Transform mainLineOrder = FullPizza.transform.Find("MainLineOrder");
        string itemtext = iQString + "x " + itemSize + " Pizza -       $" + iPString;
        mainLineOrder.GetComponent<TextMeshProUGUI>().text = itemtext;
        string ToppingString = ""; // String that will hold all toppings for order list

        //Toppings
        Transform toppingText = FullPizza.transform.Find("ToppingsStuff");
        for (int k = 1; k <= ToggleHandling.ToppingsListFull.Count; k++)
        {
            
            
            toppingText.GetComponent<TextMeshProUGUI>().text += ToggleHandling.ToppingsListFull[k-1];
            ToppingString += ToggleHandling.ToppingsListFull[k - 1];
            if (k < ToggleHandling.ToppingsListFull.Count)
            {
                toppingText.GetComponent<TextMeshProUGUI>().text += ", ";
                ToppingString += ", ";
            }


        }
        

        //Crust and Sauce
        string crustSauceString = ""; // string to hold crust and sauce for order list
        Transform crustText = FullPizza.transform.Find("CrustStuff");
        crustText.GetComponent<TextMeshProUGUI>().text = PizzaCrust + " Crust\n" ;
        crustSauceString = PizzaCrust + " Crust";

        //Sauce
        if (!Sauce)
        {
            
            crustText.GetComponent<TextMeshProUGUI>().text += "No Sauce";
            crustSauceString += ", No Sauce";
        }
        else
        {
            crustText.GetComponent<TextMeshProUGUI>().text += "Regular Sauce";
            crustSauceString += ", Regular Sauce";
        }
       

        orderSubtotal += itemPrice;
        
        decimal taxAmount = orderSubtotal * taxRate;

        orderTotal = orderSubtotal + taxAmount;


        //calculating spaces for formatting
        string SPACESFORTOTAL = "\t    ";
        string SPACESFORSUBTOTAL = "\t"; 
        string SPACESFORTAX = "\t ";
        int sublength = 10 + orderSubtotal.ToString("F2").Length;
        int taxlength = 10 + taxAmount.ToString("F2").Length;
        int totallength = 7 + orderTotal.ToString("F2").Length;
        for (int i = sublength; i <= 25; i++) //25 is the max length of the subtotal + tax lines
        {
            SPACESFORSUBTOTAL += " ";
        }
        for (int i = taxlength; i <= 25; i++)
        {
            SPACESFORTAX += " ";
        }
        for (int i = totallength; i <= 24; i++) //24 is the max length of the total line
        {
            SPACESFORTOTAL += " ";
        }
        





        //setting ui text values to reflect new order totals
        tax.text = "Tax(6%):" + SPACESFORTAX + "$" + taxAmount.ToString("F2");
        subtotal.text = "Subtotal:" + SPACESFORSUBTOTAL +"$" + orderSubtotal.ToString("F2");
        total.text = "Total:"+SPACESFORTOTAL +"$" + orderTotal.ToString("F2");


        Order.Add(iQString + "x " + itemSize + "Pizza \t" + "Toppings: " + ToppingString + " \t" + crustSauceString + " \t Price = " + itemPrice );

        lastOrderType = 1; //full pizza
        
        itemPrice = 0; //reset item price for next item


        for (int i = 0; i < Order.Count; i++)
        {
            Debug.Log("Order item " + (i + 1) + ": " + Order[i]);
        }
        
    }

    public void AddHalfPizzaOrder()
    {
        // Adds a pizza to the order with toppings that can be different on each half (and uses a specific prefab)
        itemPrice = (decimal)(sizePrices[itemSize]);
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
        //Calculate item price up based on toppings
        if (ToggleHandling.UniqueToppings > 2)
        {
            decimal extraToppings = ToggleHandling.UniqueToppings - 2;
            itemPrice += toppingPrices[itemSize] * extraToppings;
        }

        Transform Toppings1 = PizzaWindow.transform.Find("Toppings(half1)");
        Transform Toppings2 = PizzaWindow.transform.Find("Toppings(half2)");

        GameObject HalfPizza = Instantiate(HalfPizzaPrefab, Itemposition, Quaternion.identity);
        HalfPizza.transform.SetParent(OrderItems.transform, true);
        HalfPizza.transform.localScale = new Vector3(0.8428124f, 0.8428124f, 0.8428124f); //formats new instance of prefab

        for (int i = 0; i < itemQuantity - 1; i++) //calculate total price based on quantity
        {
            itemPrice += itemPrice;
        }
        string iQString = itemQuantity.ToString();
        string iPString = itemPrice.ToString();

        // Now, set values of children (Ui elements) depending on selected values from pizza customization
        //Main line (number of item, size, price
        Transform mainLineOrder = HalfPizza.transform.Find("MainLineOrder1");
        string itemtext = iQString + "x " + itemSize + " Pizza -       $" + iPString;
        mainLineOrder.GetComponent<TextMeshProUGUI>().text = itemtext;

        string ToppingString1 = ""; // Strings that will hold all toppings for order list
        string ToppingString2 = "";

        //Toppings
        Transform toppingText = HalfPizza.transform.Find("ToppingsStuff1");
        for (int k = 1; k <= ToggleHandling.ToppingsListHalf1.Count; k++)
        {


            toppingText.GetComponent<TextMeshProUGUI>().text += ToggleHandling.ToppingsListHalf1[k - 1];
            ToppingString1 += ToggleHandling.ToppingsListHalf1[k - 1];
            if (k < ToggleHandling.ToppingsListHalf1.Count)
            {
                toppingText.GetComponent<TextMeshProUGUI>().text += ", ";
                ToppingString1 += ", ";
            }

            
        }
        Transform toppingText2 = HalfPizza.transform.Find("ToppingsStuff2");
        for (int k = 1; k <= ToggleHandling.ToppingsListHalf2.Count; k++)
        {


            toppingText2.GetComponent<TextMeshProUGUI>().text += ToggleHandling.ToppingsListHalf2[k - 1];
            ToppingString2 += ToggleHandling.ToppingsListHalf2[k - 1];
            if (k < ToggleHandling.ToppingsListHalf2.Count)
            {
                toppingText2.GetComponent<TextMeshProUGUI>().text += ", ";
                ToppingString2 += ", ";
            }


        }
        //Crust and Sauce
        string crustSauceString = ""; // string to hold crust and sauce for order list
        Transform crustText = HalfPizza.transform.Find("CrustStuff");
        crustText.GetComponent<TextMeshProUGUI>().text = PizzaCrust + " Crust\n";
        crustSauceString = PizzaCrust + " Crust";

        //Sauce
        if (!Sauce)
        {

            crustText.GetComponent<TextMeshProUGUI>().text += "No Sauce";
            crustSauceString += ", No Sauce";
            
        }
        else
        {
            crustText.GetComponent<TextMeshProUGUI>().text += "Regular Sauce";
            crustSauceString += ", Regular Sauce";
        }

        orderSubtotal += itemPrice;

        decimal taxAmount = orderSubtotal * taxRate;

        orderTotal = orderSubtotal + taxAmount;


        //calculating spaces for formatting
        string SPACESFORTOTAL = "\t    ";
        string SPACESFORSUBTOTAL = "\t";
        string SPACESFORTAX = "\t ";
        int sublength = 10 + orderSubtotal.ToString("F2").Length;
        int taxlength = 10 + taxAmount.ToString("F2").Length;
        int totallength = 7 + orderTotal.ToString("F2").Length;
        for (int i = sublength; i <= 25; i++) //25 is the max length of the subtotal + tax lines
        {
            SPACESFORSUBTOTAL += " ";
        }
        for (int i = taxlength; i <= 25; i++)
        {
            SPACESFORTAX += " ";
        }
        for (int i = totallength; i <= 24; i++) //24 is the max length of the total line
        {
            SPACESFORTOTAL += " ";
        }






        //setting ui text values to reflect new order totals
        tax.text = "Tax(6%):" + SPACESFORTAX + "$" + taxAmount.ToString("F2");
        subtotal.text = "Subtotal:" + SPACESFORSUBTOTAL + "$" + orderSubtotal.ToString("F2");
        total.text = "Total:" + SPACESFORTOTAL + "$" + orderTotal.ToString("F2");


        Order.Add(iQString + "x " + itemSize + "Pizza \t" + "Half1 Toppings: " + ToppingString1 + "Half2 Toppings: " + ToppingString2 + " \t" + crustSauceString + " \t Price = " + itemPrice);

        lastOrderType = 2; //half pizza
        
        itemPrice = 0; //reset item price for next item


        for (int i = 0; i < Order.Count; i++)
        {
            Debug.Log("Order item " + (i + 1) + ": " + Order[i]);
        }
        

    }

    public void AddDrinkOrder()
    {
        //adds a drink item to the order using the other item prefab
        itemPrice = 1.75m; //all drinks are 1.75
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

        for (int i = 0; i < itemQuantity - 1; i++) //calculate total price based on quantity
        {
            itemPrice += itemPrice;
        }
        string iQString = itemQuantity.ToString();
        string iPString = itemPrice.ToString();

        // Now, set values of children (Ui elements) depending on selected values from drink customization
        //Main line (number of item, size, price)
        Transform mainLineOrder = Drink.transform.Find("MainLineOrder");
        string itemtext = iQString + "x " + DrinkSize + " "+ DrinkName + " - $" + iPString;
        mainLineOrder.GetComponent<TextMeshProUGUI>().text = itemtext;

        orderSubtotal += itemPrice;

        decimal taxAmount = orderSubtotal * taxRate;

        orderTotal = orderSubtotal + taxAmount;


        //calculating spaces for formatting
        string SPACESFORTOTAL = "\t    ";
        string SPACESFORSUBTOTAL = "\t";
        string SPACESFORTAX = "\t ";
        int sublength = 10 + orderSubtotal.ToString("F2").Length;
        int taxlength = 10 + taxAmount.ToString("F2").Length;
        int totallength = 7 + orderTotal.ToString("F2").Length;
        for (int i = sublength; i <= 25; i++) //25 is the max length of the subtotal + tax lines
        {
            SPACESFORSUBTOTAL += " ";
        }
        for (int i = taxlength; i <= 25; i++)
        {
            SPACESFORTAX += " ";
        }
        for (int i = totallength; i <= 24; i++) //24 is the max length of the total line
        {
            SPACESFORTOTAL += " ";
        }






        //setting ui text values to reflect new order totals
        tax.text = "Tax(6%):" + SPACESFORTAX + "$" + taxAmount.ToString("F2");
        subtotal.text = "Subtotal:" + SPACESFORSUBTOTAL + "$" + orderSubtotal.ToString("F2");
        total.text = "Total:" + SPACESFORTOTAL + "$" + orderTotal.ToString("F2");


        itemPrice = 0;// set price to 0

        lastOrderType = 3; //other item
    }

    public void AddSideOrder()
    {
        //adds a drink item to the order using the other item prefab
        itemPrice = 4.00m;
        if (SideName.Equals("Breadsticks ($4.00)")) { itemPrice = 4.00m; SideName = "Breadsticks"; }
        if (SideName.Equals("Breadstick Bites ($2.00)")) { itemPrice = 2.00m; SideName = "BrdStick Bites"; }
        if (SideName.Equals("Chocolate Chip Cookie ($4.00)")) { itemPrice = 4.00m; SideName = "Choc. Chip Cookie"; }
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

        for (int i = 0; i < itemQuantity - 1; i++) //calculate total price based on quantity
        {
            itemPrice += itemPrice;
        }
        string iQString = itemQuantity.ToString();
        string iPString = itemPrice.ToString();

        // Now, set values of children (Ui elements) depending on selected values from drink customization
        //Main line (number of item, size, price)
        Transform mainLineOrder = Side.transform.Find("MainLineOrder");
        string itemtext = iQString + "x " + " " + SideName + " - $" + iPString;
        mainLineOrder.GetComponent<TextMeshProUGUI>().text = itemtext;

        orderSubtotal += itemPrice;

        decimal taxAmount = orderSubtotal * taxRate;

        orderTotal = orderSubtotal + taxAmount;


        //calculating spaces for formatting
        string SPACESFORTOTAL = "\t    ";
        string SPACESFORSUBTOTAL = "\t";
        string SPACESFORTAX = "\t ";
        int sublength = 10 + orderSubtotal.ToString("F2").Length;
        int taxlength = 10 + taxAmount.ToString("F2").Length;
        int totallength = 7 + orderTotal.ToString("F2").Length;
        for (int i = sublength; i <= 25; i++) //25 is the max length of the subtotal + tax lines
        {
            SPACESFORSUBTOTAL += " ";
        }
        for (int i = taxlength; i <= 25; i++)
        {
            SPACESFORTAX += " ";
        }
        for (int i = totallength; i <= 24; i++) //24 is the max length of the total line
        {
            SPACESFORTOTAL += " ";
        }






        //setting ui text values to reflect new order totals
        tax.text = "Tax(6%):" + SPACESFORTAX + "$" + taxAmount.ToString("F2");
        subtotal.text = "Subtotal:" + SPACESFORSUBTOTAL + "$" + orderSubtotal.ToString("F2");
        total.text = "Total:" + SPACESFORTOTAL + "$" + orderTotal.ToString("F2");

        itemPrice = 0;// set price to 0

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

    public void Pay()
    {
        
        StartCoroutine(PaySuccessful());
    }

    IEnumerator PaySuccessful()
    {
        PaymentSuccessful.SetActive(true);
        MenuPopup();
        OrderItems.SetActive(false);

        yield return new WaitForSeconds(2f);

        KioskLoginUi.Signout();

    }

    public void ToCancel() //opens a window asking user to confirm if they want to cancel the order
    {
        MenuPopup();
        OrderItems.SetActive(false);
        CancelWindow.SetActive(true);

    }
    public void returnToOrder() //returns to order if user does not confirm their cancellation
    {
        CancelWindow.SetActive(false);
        HideMenuPopup();
        OrderItems.SetActive(true);
    }

    public void CancelOrder()
    {
        //decrease order ID by 1 to not skip numbers
        StaticValues.OrderID -= 1;
        KioskLoginUi.Signout();
    }
}
