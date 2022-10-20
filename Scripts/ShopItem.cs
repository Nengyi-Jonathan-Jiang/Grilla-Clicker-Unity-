using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class UIText {
    public GameObject textObject;
    public Text text;
    public RectTransform transform;

    public UIText(GameObject canvas, int font_size, Color text_color, Font font) {
        textObject = new GameObject("Text2");
        textObject.transform.SetParent(canvas.transform);

        transform = textObject.AddComponent<RectTransform>();
        transform.localScale = Vector2.one;

        text = textObject.AddComponent<Text>();
        text.fontSize = font_size;
        text.color = text_color;
        text.font = font;
    }
}

public class ShopItem : MonoBehaviour
{
    public string descriptionText;
    public bool isPassive;
    public bool isMultiplier;
    public float startValue;
    public int startCost;

    public Font font;

    public Grilla grilla;

    public GameObject canvas;

    private int currentCost;
    private float currentValue;

    private UIText description;
    private UIText cost;
    private UIText bonus;

    // Start is called before the first frame update

    private Vector2 position {
        get => transform.position;
        set {
            transform.position = new Vector3(value.x, value.y, transform.position.z);
            description.transform.anchoredPosition = 10 * value + new Vector2(0, 6);
            cost.transform.anchoredPosition = 10 * value + new Vector2(0, -6);
            bonus.transform.anchoredPosition = 10 * value + new Vector2(0, -6);
        }
    }
    void Start()
    {
        currentCost = startCost;
        currentValue = startValue;

        description = new UIText(canvas, 6, Color.white, font);
        description.text.text = descriptionText;
        description.transform.sizeDelta = new Vector2(66, 8);

        cost = new UIText(canvas, 4, Color.yellow, font);
        cost.transform.sizeDelta = new Vector2(66, 8);

        bonus = new UIText(canvas, 4, Color.red, font);
        bonus.transform.sizeDelta = new Vector2(66, 8);
        bonus.text.alignment = TextAnchor.UpperRight;

        updateText();

        position = transform.position;
    }

    void updateText() {
        cost.text.text = "$" + nextCost();
        if (isMultiplier) {
            bonus.text.text = currentValue + "x > " + nextValue() + "x";
        }
        else if (isPassive) {
            bonus.text.text = currentValue + "/s > " + nextValue() + "/s";
        }
        else {
            bonus.text.text = "+" + currentValue + " > +" + nextValue();
        }
    }

    float nextValue() {
        if (isMultiplier) {
            char t = currentValue.ToString()[0];
            return t == '1' || t == '5' ? currentValue * 2 : currentValue * 5 / 2;
        }
        return currentValue + startValue;
    }

    int nextCost() {
        if (isMultiplier) {
            float lv = Mathf.Log10(nextValue() / 2);
            return Mathf.RoundToInt(Mathf.Exp(lv) * startCost);
        }
        else {
            int lv = (int)currentValue / (int)startValue;
            return lv * startCost;
        }
    }

    bool canBuy = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)) {
            position += new Vector2(0, Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            position -= new Vector2(0, Time.deltaTime);
        }

        if (!canBuy && grilla.money >= currentCost) {
            canBuy = true;
            bonus.text.color = Color.green;
        }

        if (Input.GetMouseButtonDown(0) && isHovering && canBuy) {
            grilla.money -= currentCost;
            currentValue = nextValue();
            updateText();
        }

        if (canBuy && grilla.money < currentCost) {
            canBuy = false;
            bonus.text.color = Color.red;
        }

    }

    bool isHovering {
        get {
            Vector2 n = 2 * ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - position);
            return Math.Abs(n.x) <= transform.localScale.x && Math.Abs(n.y) <= transform.localScale.y;
        }
    }
}
