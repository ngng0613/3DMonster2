using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class StageData : MonoBehaviour
{
    public List<MonsterBase> EnemyData = new List<MonsterBase>();
    public string StageName;
    [TextArea] public string _stageInfoText;
    [SerializeField] Image _backgroundImage1;
    [SerializeField] Image _backgroundImage2;
    [SerializeField] Color[] _neutralColor;
    [SerializeField] Color[] _activeColor;

    Vector3 _background1DefaultPos;
    Vector3 _bacground2DefaultPos;

    public TextMeshProUGUI NameText;
    public bool IsChoose = false;

    private void OnValidate()
    {
        NameText.text = StageName;
    }
    public void UpdateView()
    {
        NameText.text = StageName;
    }


    public void ChangeState(bool isChoose)
    {
        this.IsChoose = isChoose;

        if (isChoose == true)
        {
            _backgroundImage1.color = _activeColor[0];
            _backgroundImage2.color = _activeColor[1];
        }
        else
        {
            _backgroundImage1.color = _neutralColor[0];
            _backgroundImage2.color = _neutralColor[1];
        }
    }






}
