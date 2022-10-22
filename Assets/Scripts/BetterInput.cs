using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterInput : MonoBehaviour
{
    static bool wasMouseDown = false;
    static bool mDown = false;
    static bool mIsDown = false;
    static bool mUp = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mIsDown = Input.GetMouseButton(0) || Input.touchCount > 0;
        mDown = mIsDown && !wasMouseDown;
        mUp = !mIsDown && wasMouseDown;
        wasMouseDown = mIsDown;
    }

    public static bool GetMouseDown() {
        return mDown;
    }
    public static bool GetMouse() {
        return mIsDown;
    }
    public static bool GetMouseUp() {
        return mUp;
    }

    public static Vector2 mousePosition{
        get {
            Vector2 mPos = Input.touchCount > 0 ? Input.touches[0].position : (Vector2) Input.mousePosition;
            return Camera.main.ScreenToWorldPoint(mPos);
        }
    }
}
