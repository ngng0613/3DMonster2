using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class StageData : MonoBehaviour
{
    public List<MonsterBase> enemyData = new List<MonsterBase>();
    public string stageName;
    [TextArea] public string stageInfoText;
    [SerializeField] Image backgroundImage1;
    [SerializeField] Image backgroundImage2;
    [SerializeField] Color[] neutralColor;
    [SerializeField] Color[] activeColor;

    Vector3 background1DefaultPos;
    Vector3 bacground2DefaultPos;

    public TextMeshProUGUI nameText;
    public bool isChoose = false;

    private void OnValidate()
    {
        nameText.text = stageName;
    }
    public void UpdateView()
    {
        nameText.text = stageName;
    }


    public void ChangeState(bool isChoose)
    {
        this.isChoose = isChoose;

        if (isChoose == true)
        {
            backgroundImage1.color = activeColor[0];
            backgroundImage2.color = activeColor[1];
        }
        else
        {
            backgroundImage1.color = neutralColor[0];
            backgroundImage2.color = neutralColor[1];
        }
    }






}
