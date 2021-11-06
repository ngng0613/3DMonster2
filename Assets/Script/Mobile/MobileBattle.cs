using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MobileBattle : MonoBehaviour
{
    [SerializeField] List<SpinCircle> circleList = new List<SpinCircle>();
    [SerializeField] int plusCount = 1;

    private float criticalPower = 0;
    [SerializeField] Image criticalGauge;
    [SerializeField] TextMeshProUGUI critGaugeText;

    [SerializeField] MonsterBase tempMonster;

    private void Start()
    {
        tempMonster.SetCurrentHPValue(tempMonster.GetMaxHPValue());
    }

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
                tempMonster.TakeDamage(5);
                test = true;
            }
        }
        if (test == false)
        {
            Debug.Log("失敗…");
            if (criticalPower > 0)
            {
                criticalPower -= 5;
            }
        }
        else
        {
            if (criticalPower < 100)
            {
                criticalPower += 5;
                criticalGauge.transform.localScale = new Vector3(1.0f, criticalPower / 100, 1.0f);
                critGaugeText.text = "必殺\n" + criticalPower;
            }
        }

    }
}
