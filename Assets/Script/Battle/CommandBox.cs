using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CommandBox : MonoBehaviour
{
    public Text myText { get; set; }
    [SerializeField] Image myImage;
    [SerializeField] Sprite imageBlue;
    [SerializeField] Sprite imageOrange;
    [SerializeField] int list_Id;
    [SerializeField] SkillBase skill;
    [SerializeField] Image light;
    [SerializeField] float blinkTime;

    Sequence sequence;

    public enum ImageColor
    {
        Blue,
        Orange,
    }


    public void BlinkOn()
    {
        sequence = DOTween.Sequence();

        sequence.Append(light.DOFade(0.5f, blinkTime)).SetLoops(-1, LoopType.Yoyo);

    }
    public void BlinkOff()
    {
        sequence.Kill();
        Color colorTemp = light.color;
        colorTemp.a = 0;
        light.color = colorTemp;

    }

    public void SetId(int id)
    {
        this.list_Id = id;

    }

    public int GetId()
    {
        return list_Id;
    }

    public void SetSkill(SkillBase skill)
    {
        this.skill = skill;

    }

    public SkillBase GetSkill()
    {
        return skill;
    }

    public void ChangeImage(ImageColor color)
    {
        if (color == ImageColor.Blue)
        {
            myImage.sprite = imageBlue;
        }
        else if (color == ImageColor.Orange)
        {
            myImage.sprite = imageOrange;
        }

    }

    public void OnPush()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f))
                .Append(transform.DOScale(Vector3.one, 0.1f));

    }
}
