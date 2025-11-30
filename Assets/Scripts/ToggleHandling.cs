using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleHandling : MonoBehaviour
{
    public Toggle Full;
    public Toggle Half;
    public GameObject FullAddButton;
    public GameObject HalfAddButton;
    public GameObject FullToppings;
    public GameObject HalfToppings;
    public static List<string> ToppingsListFull = new List<string>();
    public static List<string> ToppingsListHalf1 = new List<string>();
    public static List<string> ToppingsListHalf2 = new List<string>();
    public static int UniqueToppings;

    public static bool FullPizza = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetToppings();
        
        // add listeners to all toppings toggles
        for (int i = 0; i < FullToppings.transform.childCount; i++)
        {
            Toggle toggle = FullToppings.transform.GetChild(i).GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(delegate { ToppingSelected(toggle); });
        }
        for (int j = 0; j < 2; j++)
        {
            GameObject halfcollection = HalfToppings.transform.GetChild(j).gameObject;
            for (int i = 0; i < halfcollection.transform.childCount; i++) 
            {
                Toggle toggle = halfcollection.transform.GetChild(i).GetComponent<Toggle>();
                toggle.onValueChanged.AddListener(delegate { ToppingSelected(toggle); });
            }
        }
      
        Full.onValueChanged.AddListener(OnFullChanged);
        Half.onValueChanged.AddListener(OnHalfChanged);
    }

    public void OnFullChanged(bool isOn)
    {
        //reset all topping toggles when any switch is made
        ResetToppings();
        if (isOn)
        {
            Half.isOn = false;
            FullPizza = true;
            FullAddButton.SetActive(true);
            HalfAddButton.SetActive(false);
            FullToppings.SetActive(true);
            HalfToppings.SetActive(false);
        }
        else
        {
            Half.isOn = true;
            FullPizza = false;
            HalfAddButton.SetActive(true);
            FullAddButton.SetActive(false);
            FullToppings.SetActive(false);
            HalfToppings.SetActive(true);
        }
    }

    public void OnHalfChanged(bool isOn)
    {
        //reset all topping toggles when any switch is made
        ResetToppings();

        if (isOn)
        {
            Full.isOn = false;
            FullPizza = false;
            HalfAddButton.SetActive(true);
            FullAddButton.SetActive(false);
            FullToppings.SetActive(false);
            HalfToppings.SetActive(true);
        }
        else
        {
            Full.isOn = true;
            FullPizza = true;
            FullAddButton.SetActive(true);
            HalfAddButton.SetActive(false);
            FullToppings.SetActive(true);
            HalfToppings.SetActive(false);
        }

    }
    public void ToppingSelected(Toggle topping)
    {
        switch(topping.transform.parent.name)
        {
            case "ToppingsFull":
                if (topping.isOn)
                {
                    ToppingsListFull.Add(topping.name);
                }
                else
                {
                    ToppingsListFull.Remove(topping.name);
                }
                break;
            case "ToppingsHalf1":
                if (topping.isOn)
                {
                    ToppingsListHalf1.Add(topping.name);
                }
                else
                {
                    ToppingsListHalf1.Remove(topping.name);
                }
                break;
            case "ToppingsHalf2":
                if (topping.isOn)
                {
                    ToppingsListHalf2.Add(topping.name);
                    
                }
                else
                {
                    ToppingsListHalf2.Remove(topping.name);
                }
                break;
        }
        IEnumerable<string> commonItems = ToppingsListHalf1.Intersect(ToppingsListHalf2);
        UniqueToppings = ToppingsListHalf1.Count + ToppingsListHalf2.Count - commonItems.Count();

        Debug.Log("Full Toppings: " + string.Join(", ", ToppingsListFull));
        Debug.Log("Half1 Toppings: " + string.Join(", ", ToppingsListHalf1));
        Debug.Log("Half2 Toppings: " + string.Join(", ", ToppingsListHalf2));

    }

    public void ResetToppings()
    {
        ToppingsListFull.Clear();
        ToppingsListFull.Add("Cheese");
        ToppingsListHalf1.Clear();
        ToppingsListHalf1.Add("Cheese");
        ToppingsListHalf2.Clear();
        ToppingsListHalf2.Add("Cheese");
        UniqueToppings = 0;


        for (int i = 0; i < FullToppings.transform.childCount; i++)
        {
            Toggle toggle = FullToppings.transform.GetChild(i).GetComponent<Toggle>();
            if (toggle.name != "Cheese")
            {
                toggle.isOn = false;
            }

        }
        for (int j = 0; j < 2; j++)
        {
            GameObject halfcollection = HalfToppings.transform.GetChild(j).gameObject;
            for (int i = 0; i < halfcollection.transform.childCount; i++) 
            {
                Toggle toggle = halfcollection.transform.GetChild(i).GetComponent<Toggle>();
                if (toggle.transform.name != "Cheese")
                {
                    toggle.isOn = false;
                }

            }
        }

    }
}
