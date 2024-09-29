using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ShopElementScript : MonoBehaviour
{
    public Image Image;
    public RectTransform Rect;
    public TMP_Text Text;
    public TMP_Text ButtonText;

    public Image ButtonImage;
    public Image CoinImage;


    public ProductType ProductType;


    public ShopScript Shop;

    public bool Sold;


    public void ButtonClick ()
    {
        if(!Sold)
        {
            Shop.Buy(this);
        }
        else
        {
            Shop.ChangeWhearState(this);
        }
    }
}
