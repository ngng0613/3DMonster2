using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HpGauge : MonoBehaviour
{
    public bool isActive { get; set; } = true;
    [SerializeField] Text nameText;
    [SerializeField] Image hpGauge;
    [SerializeField] Image elementImage;

    string monsterName = "";
    int maxHp = 0;
    int currentHp = 0;
    float displayHp = 0;
    [SerializeField] Camera cameraComponent;

    [SerializeField] bool setupCompleted = false;
    [SerializeField] float tweenTime = 1.0f;

    private void Update()
    {
        if (!setupCompleted)
        {
            return;
        }
        hpGauge.transform.localScale = new Vector3(displayHp / maxHp, 1, 1);
        if (cameraComponent != null)
        {
            this.transform.rotation = cameraComponent.transform.rotation;
        }

    }


    public void Setup(string monsterName, int maxHp, int currentHp, Camera cameraComponent, Sprite elementIcon)
    {
        this.monsterName = monsterName;
        this.maxHp = maxHp;
        this.currentHp = currentHp;
        displayHp = currentHp;

        this.cameraComponent = cameraComponent;
        elementImage.sprite = elementIcon;
        UpdateStatus(this.currentHp);
        setupCompleted = true;
    }

    public void UpdateStatus(int currentHp)
    {
        this.currentHp = currentHp;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(DOTween.To(() => this.displayHp, (x) => this.displayHp = x, this.currentHp, tweenTime));

    }

    public void Display(bool onOff)
    {

    }
}
