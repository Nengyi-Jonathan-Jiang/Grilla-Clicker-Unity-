using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BigInteger = System.Numerics.BigInteger;

class UIText {
    public GameObject textObject;
    public Text text;
    public RectTransform transform;

    public UIText(GameObject canvas, int font_size, Color text_color, Font font) {
        textObject = new GameObject("Text");
        textObject.transform.SetParent(canvas.transform);

        transform = textObject.AddComponent<RectTransform>();
        transform.localScale = Vector2.one;

        text = textObject.AddComponent<Text>();
        text.fontSize = font_size;
        text.color = text_color;
        text.font = font;
        text.transform.position = Vector3.zero;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
    }
}

public class ShopItem : MonoBehaviour
{
    public string descriptionText;
    public bool isPassive;
    public bool isMultiplier;

    public String startValueString, startCostString;

    public BigInteger startValue;
    public BigInteger startCost;

    public Font font;

    public Grilla grilla;

    public GameObject canvas;

    public BigInteger currentCost;
    public BigInteger currentValue;

    private UIText description;
    private UIText cost;
    private UIText bonus;

    // Start is called before the first frame update

    private Vector2 position  => transform.position;

    public void updateTextPosition() {
        description.transform.anchoredPosition = 10 * position + new Vector2(0, 5);
        cost.transform.anchoredPosition = 10 * position + new Vector2(0, -6);
        bonus.transform.anchoredPosition = 10 * position + new Vector2(0, -6);
    }

    void Start()
    {
        startCost = BigInteger.Parse(startCostString);
        startValue = BigInteger.Parse(startValueString);

        currentCost = startCost;
        currentValue = isMultiplier ? 1 : 0;

        description = new UIText(canvas, 6, Color.white, font);
        description.text.text = descriptionText;
        description.transform.sizeDelta = new Vector2(68, 8);

        cost = new UIText(canvas, 4, Color.yellow, font);
        cost.transform.sizeDelta = new Vector2(68, 8);

        bonus = new UIText(canvas, 4, Color.red, font);
        bonus.transform.sizeDelta = new Vector2(68, 8);
        bonus.text.alignment = TextAnchor.UpperRight;

        updateText();
        updateTextPosition();
    }

    void updateText() {
        cost.text.text = fancyNumber(currentCost) + " bananas";
        string nextValueText = fancyNumber(nextValue());
        string currValueText = fancyNumber(currentValue);
        if (isMultiplier) {
            bonus.text.text = currValueText + "x > " + nextValueText + "x";
        }
        else if (isPassive) {
            bonus.text.text = currValueText + "/s > " + nextValueText + "/s";
        }
        else {
            bonus.text.text = "+" + currValueText + " > +" + nextValueText;
        }
    }

    BigInteger nextValue() {
        if (isMultiplier) {
            char t = currentValue.ToString()[0];
            return t == '0' ? startValue : t == '1' || t == '5' ? currentValue * 2 : currentValue * 5 / 2;
        }
        return currentValue + startValue;
    }

    BigInteger nextCost() {
        if (isMultiplier) {
            double lv = (double)nextValue() / 2f - 1;
            return (BigInteger) Math.Round(Math.Pow(3.0, lv) * (double)startCost);
        }
        else {
            double lv = (double)currentValue / (double)startValue + 1;
            return (BigInteger) Math.Round(Math.Pow(1.2, lv) * (double)startCost);
        }
    }

    bool canBuy = false;

    // Update is called once per frame
    void Update()
    {
        if (!canBuy && grilla.money >= currentCost) {
            canBuy = true;
            bonus.text.color = Color.green;
        }

        if (BetterInput.GetMouseDown() && isHovering && canBuy) {
            buy();
        }

        if (canBuy && grilla.money < currentCost) {
            canBuy = false;
            bonus.text.color = Color.red;
        }

    }

    void buy() {
        if (isMultiplier) {
            grilla.multiplier = grilla.multiplier * nextValue() / currentValue;
        }
        else if (isPassive) {
            grilla.passiveDamage += nextValue() - currentValue;
        }
        else {
            grilla.clickDamage += nextValue() - currentValue;
        }
        grilla.money -= currentCost;
        currentValue = nextValue();
        currentCost = nextCost();
        updateText();
    }

    bool isHovering {
        get {
            Vector2 n = 2 * (BetterInput.mousePosition - (position + new Vector2(1.75f, -.5f)));
            return Math.Abs(n.x) <= 3.5 && Math.Abs(n.y) <= 1;
        }
    }

    private static readonly string[] suffixes = {
        "K", "M", "B", "T", "q", "Q", "s", "S", "O", "N", "D"
    };

    public static String fancyNumber(BigInteger num) {

        if (num < 10000)
            return num.ToString();

        int digits = num.ToString().Length - 1;

        int sigDigits = (int)(num / BigInteger.Pow(10, digits - 2));

        String suffix = suffixes[(digits - 3) / 3];

        String numString = sigDigits.ToString();

        switch (digits % 3) {
            case 0:
                return numString[0] + "." + numString[1] + numString[2] + suffix;
            case 1:
                return "" + numString[0] + numString[1] + "." + numString[2] + suffix;
            case 2:
            default:
                return numString + suffix;
        }
    }

}
