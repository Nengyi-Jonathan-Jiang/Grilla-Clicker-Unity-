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
        
    }

    private bool hovering => ((Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position)).magnitude <= 2.5;

    private bool mDown;
	private int money = 0;
	
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

    void addPassiveMoney() {

    }

    void addMoney() {
		money++;
		text.text = "Money: $" + money;
    }
}
