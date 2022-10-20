using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grilla : MonoBehaviour
{
    public GameObject bananPrefab;
	public Text text;

    // Start is called before the first frame update
    void Start()
    {
        clickDamage = 1;
        passiveDamage = 0;
        multiplier = 1;
    }

    private bool hovering => ((Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position)).magnitude <= 2.5;

    private bool mDown;
	public int _money = 0;

    public int clickDamage;
    public int passiveDamage;
    public int multiplier;

    public int money {
        get => _money;
        set {
            _money = value;
            text.text = "Money: $" + _money;
        }
    }
	
    // Update is called once per frame
    void Update()
    {
        addPassiveMoney();
        if (Input.GetMouseButtonDown(0) && hovering) {
            mDown = true;
            addMoney();
            Instantiate(bananPrefab).transform.position = new Vector3(Random.Range(-7.5f, -0.5f), 5, -1);
        }

        if (Input.GetMouseButtonUp(0)) {
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
        timeCoolDown -= Time.deltaTime;
    }

    void addMoney() {
        addMoney(clickDamage);
    }

    void addMoney(int addedMoney) {
        money += addedMoney * multiplier;
        text.text = "Money: $" + money;
    }
}
