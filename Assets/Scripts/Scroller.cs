using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    public ShopItem[] items;

    public float width = 7;
    public float height;
    public Vector2 position {
        get => (Vector2) transform.position - new Vector2(0, 2.5f * items.Length / 2);
        set {
            transform.position += (Vector3) (value - position);

            foreach (ShopItem item in items) {
                item.updateTextPosition();
            }
        }
    }

    void Awake() {
        height = items.Length * 2.5f;
        for (int i = 0; i < items.Length; i++) {
            items[i].transform.localPosition = new Vector3(0, -(i + 0.5f) * 2.5f, 0);
        }
    }

    bool isHovering {
        get {
            Vector2 n = 2 * (BetterInput.mousePosition - position);
            return Math.Abs(n.x) <= width && Math.Abs(n.y) <= height;
        }
    }

    bool dragging = false;
    Vector2 lastMousePos;

    // Update is called once per frame
    void Update() {
        if (BetterInput.GetMouseDown() && isHovering) {
            dragging = true;
            lastMousePos = BetterInput.mousePosition;
        }
        if (BetterInput.GetMouse() && dragging) {
            float delta = BetterInput.mousePosition.y - lastMousePos.y;
            position += new Vector2(0, delta);
            lastMousePos += new Vector2(0, delta);
            if (position.y < -height/2f) {
                position = new Vector2(position.x, -height / 2f);
            }
            if (position.y > height / 2f) {
                position = new Vector2(position.x, height / 2f);
            }
        }
        if (BetterInput.GetMouseUp()) {
            dragging = false;
        }
    }
}
