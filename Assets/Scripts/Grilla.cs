using BigInteger = System.Numerics.BigInteger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grilla : MonoBehaviour
{
    public GameObject bananPrefab;
	public Text text;

    float lastFrameTime, deltaTime;

    // Start is called before the first frame update
    void Start()
    {
        clickDamage = 1;
        passiveDamage = 0;
        multiplier = 1;

        lastFrameTime = Time.realtimeSinceStartup;
    }

    private bool hovering => (BetterInput.mousePosition - (Vector2)transform.position).magnitude <= 2.5;

    private bool mDown;
	public BigInteger _money = 0;

    public BigInteger clickDamage;
    public BigInteger passiveDamage;
    public BigInteger multiplier;

    public BigInteger money {
        get => _money;
        set {
            _money = value;
            text.text = "Bananas: " + ShopItem.fancyNumber(_money);
        }
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime = Time.realtimeSinceStartup - lastFrameTime;
        lastFrameTime += deltaTime;

        addPassiveMoney();
        if (BetterInput.GetMouseDown() && hovering) {
            mDown = true;
            addMoney();
            Instantiate(bananPrefab).transform.position = new Vector3(Random.Range(-7.5f, -0.5f), 5, -1);
        }

        if (BetterInput.GetMouseUp()) {
            mDown = false;
        }

        transform.localScale = Vector2.one * 5.0f * (mDown ? 1.05f : 1.0f);
    }

    float timeCoolDown = 0;

    void addPassiveMoney() {
        while (timeCoolDown <= 0) {
            addMoney(passiveDamage);
            timeCoolDown += 1f;
        }
        timeCoolDown -= deltaTime;
    }

    void addMoney() {
        addMoney(clickDamage);
    }

    void addMoney(BigInteger addedMoney) {
        money += addedMoney * multiplier;
    }
}