using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileBattle : MonoBehaviour
{
    [SerializeField] List<SpinCircle> circleList = new List<SpinCircle>();
    [SerializeField] int plusCount = 1;

    public void OnClick()
    {
        bool test = false;
        foreach (SpinCircle circle in circleList)
        {
            float angleDifference = 0 + circle.transform.localEulerAngles.z;
            if (circle.CircleLength > angleDifference)
            {
                Debug.Log(circle.Monster.GetNickname() + " : " + angleDifference);
                circle.Monster.coolTime += plusCount;
                test = true;
            }
        }

        if (test == false)
        {
            Debug.Log("失敗…");
        }

    }
}
