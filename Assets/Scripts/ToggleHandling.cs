using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleHandling : MonoBehaviour
{
    public Toggle Full;
    public Toggle Half;
    public GameObject FullAddButton;
    public GameObject HalfAddButton;
    
    public static bool FullPizza = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Full.onValueChanged.AddListener(OnFullChanged);
        Half.onValueChanged.AddListener(OnHalfChanged);
    }

    public void OnFullChanged(bool isOn)
    {
        if (isOn)
        {
            Half.isOn = false;
            FullPizza = true;
            FullAddButton.SetActive(true);
            HalfAddButton.SetActive(false);
        }
        else
        {
            Half.isOn = true;
            FullPizza = false;
            HalfAddButton.SetActive(true);
            FullAddButton.SetActive(false);
        }
    }

    public void OnHalfChanged(bool isOn)
    {
        if (isOn)
        {
            Full.isOn = false;
            FullPizza = false;
            HalfAddButton.SetActive(true);
            FullAddButton.SetActive(false);
        }
        else
        {
            Full.isOn = true;
            FullPizza = true;
            FullAddButton.SetActive(true);
            HalfAddButton.SetActive(false);
        }

    }
}
