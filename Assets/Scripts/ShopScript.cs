using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public enum ProductType
{
    Cloth,
    FrogStyle,
    LineStyle,
}


public enum ClothType
{
    Mask,
    Object,
}

[Serializable]
public class Product
{
    public string Text;
    public int Price;
    public bool Have;
}


[Serializable]
public class Cloth : Product
{
    public Sprite Sprite;
    public Color Color;

    public ClothType ClothType;

    public Vector2 Position;
    public float Angle;
    public Vector2 Size;
}

[Serializable]
public class LineStyle : Product
{
    public Gradient Gradient;
}

[Serializable]
public class FrogStyle : Product
{
    public Color Color;
}

[Serializable]
public class ButtonState
{
    public string Text;
    public Color Color;
}

public class ShopScript : MonoBehaviour
{
    public MainScript Main;

    public PlayerScript Player;

    public List<ButtonState> ButtonStates = new List<ButtonState>();


    [Header("Shop Element")]
    public CanvasGroup ShopMenu;
    public GameObject ShopElementPrefab;
    public List<ShopElementScript> ShopElements = new List<ShopElementScript>();

    [Header("Clothes")]
    public RectTransform ClothContent;
    public List<Cloth> Clothes = new List<Cloth>();

    [Header("Items")]
    public RectTransform ItemContent;
    public List<Cloth> Items = new List<Cloth>();

    [Header("Frog Styles")]
    public RectTransform FrogStyleContent;
    public List<FrogStyle> FrogStyles = new List<FrogStyle>();

    [Header("Line Styles")]
    public RectTransform LineStyleContent;
    public List<LineStyle> LineStyles = new List<LineStyle>();

    void Start ()
    {
        InitShop ();
    }

    void InitShop ()
    {
        foreach (var cloth in Clothes)
        {
            ShopElementScript shopElement = Instantiate(ShopElementPrefab, ClothContent).GetComponent<ShopElementScript>();
            ShopElements.Add(shopElement);

            shopElement.Shop = this;
            shopElement.Image.sprite = cloth.Sprite;

            shopElement.ButtonImage.color = ButtonStates[0].Color;
            shopElement.ButtonText.text = cloth.Price.ToString();
            shopElement.Text.text = cloth.Text;

            shopElement.Image.SetNativeSize();

            if(shopElement.Image.rectTransform.sizeDelta.x > shopElement.Image.rectTransform.sizeDelta.y)
            {
                float diferent = 350 / shopElement.Image.rectTransform.sizeDelta.x;

                shopElement.Image.rectTransform.sizeDelta = new Vector2(350, shopElement.Image.rectTransform.sizeDelta.y * diferent);
            }
            else
            {
                float diferent = 180 / shopElement.Image.rectTransform.sizeDelta.y;

                shopElement.Image.rectTransform.sizeDelta = new Vector2(shopElement.Image.rectTransform.sizeDelta.x * diferent, 180 );
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(shopElement.transform.parent.GetComponent<RectTransform>());

            //shopElement.Image.rectTransform.sizeDelta *= new Vector2(12, 12);


        }
    }

    public void ShowShopMenu ()
    {
        ShopMenu.alpha = 1;
        ShopMenu.blocksRaycasts = true;
        ShopMenu.interactable = true;
    }

    public void CloseShopMenu ()
    {
        ShopMenu.alpha = 0;
        ShopMenu.blocksRaycasts = false;
        ShopMenu.interactable = false;
    }

    public void Buy (ShopElementScript shopElement)
    {
        if(shopElement.ProductType == ProductType.Cloth)
        {
            Cloth cloth = Clothes[shopElement.transform.GetSiblingIndex()];

            if(Main.Coins >= cloth.Price)
            {
                Main.AddCoins(-cloth.Price);
                cloth.Have = true;

                ChangeButtonToWear(shopElement);
            }
        }
        
    }

    void ChangeButtonToWear (ShopElementScript shopElement)
    {
        shopElement.ButtonImage.color = ButtonStates[1].Color;
        shopElement.ButtonText.text = ButtonStates[1].Text;
        shopElement.CoinImage.gameObject.SetActive(false);
        shopElement.Sold = true;
    }

    void ChangeButtonToUnwear (ShopElementScript shopElement)
    {
        shopElement.ButtonImage.color = ButtonStates[2].Color;
        shopElement.ButtonText.text = ButtonStates[2].Text;
    }

    public void ChangeWhearState (ShopElementScript shopElement)
    {
        if(shopElement.ProductType == ProductType.Cloth)
        {
            if(Player.ClothID == -1)
            {
                ChangeButtonToUnwear(shopElement);

                Player.ClothID = shopElement.transform.GetSiblingIndex();
                WearState ();
            }
            else if(Player.ClothID == shopElement.transform.GetSiblingIndex())
            {
                ChangeButtonToWear(shopElement);
                Player.ClothID = -1;
                WearState ();
            }
            else if(Player.ClothID != shopElement.transform.GetSiblingIndex())
            {
                ChangeButtonToWear(ShopElements[Player.ClothID]);
                ChangeButtonToUnwear(shopElement);

                Player.ClothID = shopElement.transform.GetSiblingIndex();
                WearState ();
            }
        }
    }


    void WearState ()
    {
        if(Player.ClothID == -1)
        {
            Player.ClothRenderer.sprite = null;
        }
        else
        {
            Player.ClothRenderer.sprite = Clothes[Player.ClothID].Sprite;
            Player.ClothRenderer.transform.localPosition = Clothes[Player.ClothID].Position;
            Player.ClothRenderer.transform.localScale = Clothes[Player.ClothID].Size;
            Player.ClothRenderer.transform.eulerAngles = new Vector3(0, 0, Clothes[Player.ClothID].Angle);
        }

        if(Player.ItemID == -1)
        {
            Player.ItemhRenderer.sprite = null;
        }
        else
        {
            Player.ItemhRenderer.sprite = Items[Player.ItemID].Sprite;
            Player.ItemhRenderer.transform.localPosition = Items[Player.ItemID].Position;
            Player.ItemhRenderer.transform.localScale = Items[Player.ItemID].Size;
            Player.ItemhRenderer.transform.eulerAngles = new Vector3(0, 0, Items[Player.ClothID].Angle);
        }
    }

}
