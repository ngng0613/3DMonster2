using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static bool setupCompleted { get; set; } = false;

    public float inputInterval = 10.0f;
    float intervalCount = 0;

    public enum InputAxisType
    {
        type4,
        type8
    }
    public InputAxisType axisType = InputAxisType.type4;

    delegate void D<T>(T obj);

    public static Action InputEnter;
    public static Action InputCancel;

    public static Action InputSubButton1;
    public static Action InputSubButton2;

    public static Action InputNeutral;
    public static Action InputUp;
    public static Action InputDown;
    public static Action InputRight;
    public static Action InputLeft;
    public static Action InputUpperRight;
    public static Action InputUpperLeft;
    public static Action InputLowerRight;
    public static Action InputLowerLeft;


    float timeCount = 0;

    // Update is called once per frame
    void Update()
    {
        if (!setupCompleted)
        {
            return;
        }

        timeCount += Time.deltaTime;
        if (timeCount >= inputInterval)
        {

            if (Input.GetButtonDown("決定") && InputEnter != null)
            {
                InputEnter.Invoke();
                timeCount = 0;
            }
            if (Input.GetButtonDown("キャンセル") && InputCancel != null)
            {
                InputCancel.Invoke();
                timeCount = 0;
            }
            if (Input.GetButtonDown("サブ1") && InputSubButton1 != null)
            {
                InputSubButton1.Invoke();
                timeCount = 0;
            }
            if (Input.GetButtonDown("サブ2") && InputSubButton2 != null)
            {
                InputSubButton2.Invoke();
                timeCount = 0;
            }
            if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0 && InputNeutral != null)
            {
                InputNeutral.Invoke();

            }
            if (axisType == InputAxisType.type4)
            {
                if (Input.GetAxisRaw("Vertical") > 0 && InputUp != null)
                {
                    InputUp.Invoke();
                    timeCount = 0;
                }
                else if (Input.GetAxisRaw("Vertical") < 0 && InputDown != null)
                {
                    InputDown.Invoke();
                    timeCount = 0;
                }
                else if (Input.GetAxisRaw("Horizontal") > 0 && InputRight != null)
                {
                    InputRight.Invoke();
                    timeCount = 0;
                }
                else if (Input.GetAxisRaw("Horizontal") < 0 && InputLeft != null)
                {
                    InputLeft.Invoke();
                    timeCount = 0;
                }
            }
            else if (axisType == InputAxisType.type8)
            {
                if (Input.GetAxisRaw("Vertical") > 0 && Input.GetAxisRaw("Horizontal") == 0 && InputUp != null)
                {
                    InputUp.Invoke();
                }
                else if (Input.GetAxisRaw("Vertical") < 0 && Input.GetAxisRaw("Horizontal") == 0 && InputDown != null)
                {
                    InputDown.Invoke();
                }
                else if (Input.GetAxisRaw("Horizontal") > 0 && Input.GetAxisRaw("Vertical") == 0 && InputRight != null)
                {
                    InputRight.Invoke();
                }
                else if (Input.GetAxisRaw("Horizontal") < 0 && Input.GetAxisRaw("Vertical") == 0 && InputLeft != null)
                {
                    InputLeft.Invoke();
                }
                else if (Input.GetAxisRaw("Vertical") > 0 && Input.GetAxisRaw("Horizontal") < 0 && InputUpperRight != null)
                {
                    InputUpperRight.Invoke();
                }
                else if (Input.GetAxisRaw("Vertical") > 0 && Input.GetAxisRaw("Horizontal") > 0 && InputUpperLeft != null)
                {
                    InputUpperLeft.Invoke();
                }
                else if (Input.GetAxisRaw("Vertical") < 0 && Input.GetAxisRaw("Horizontal") < 0 && InputLowerRight != null)
                {
                    InputLowerRight.Invoke();
                }
                else if (Input.GetAxisRaw("Vertical") < 0 && Input.GetAxisRaw("Horizontal") > 0 && InputLowerLeft != null)
                {
                    InputLowerLeft.Invoke();
                }
            }

        }
    }
    public static void ResetInputSettings()
    {
        InputEnter = null;
        InputCancel = null;
        InputSubButton1 = null;
        InputSubButton2 = null;
        InputUp = null;
        InputDown = null;
        InputLeft = null;
        InputRight = null;
        InputUpperRight = null;
        InputUpperLeft = null;
        InputLowerRight = null;
        InputLowerLeft = null;
    }
}
