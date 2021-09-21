using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusDisplayManager : MonoBehaviour
{
    MonsterBase monster;
    [SerializeField] string chara_Name;
    [SerializeField] int chara_Hp;
    [SerializeField] int chara_Mp;

    [SerializeField] int chara_MaxHp;
    [SerializeField] int chara_MaxMp;

    [SerializeField] int chara_BeforeHp;
    [SerializeField] int chara_BeforeMp;

    [SerializeField] float hp_AmountOfDecrease;
    [SerializeField] float mp_AmountOfDecrease;

    bool updateDisplaySwitch;

    Text nameText;
    Text hpText;
    Text mpText;

    Sprite chara_Image;
    Image imageView;
    Image hpGauge;
    Image mpGauge;
    Image hpGauge_Background;
    Image mpGauge_Background;

    [SerializeField] int phase = 1;

    float hpTemp = 0;
    float mpTemp = 0;

    const float gauge_MaxLength = 300.0f;

    float hp_BeforeLength;
    float mp_BeforeLength;
    //hpゲージとバックグラウンドの長さの差
    float hpGauge_DifferenceInlength;
    float mpGauge_DifferenceInlength;

    //汎用カウンター
    int count = 0;

    //初期スケール
    float scale_Default = 0.7f;
    //ポップアップ時スケール
    float scale_Popup = 0.9f;



    // Update is called once per frame
    void Update()
    {
        if (updateDisplaySwitch)
        {

            if (phase == 1)
            {

                //情報更新
                chara_Hp = monster.GetCurrentHPValue();
                chara_Mp = monster.GetCurrentMPValue();

                if (chara_Hp < 0)
                {
                    chara_Hp = 0;
                }
                if (chara_Mp < 0)
                {
                    chara_Mp = 0;
                }

                hp_AmountOfDecrease = chara_BeforeHp - chara_Hp;
                mp_AmountOfDecrease = chara_BeforeMp - chara_Mp;

                hpTemp = chara_BeforeHp;
                mpTemp = chara_BeforeMp;

                hp_BeforeLength = hpGauge.rectTransform.sizeDelta.x;
                mp_BeforeLength = mpGauge.rectTransform.sizeDelta.x;

                count = 0;
                phase = 2;
            }
            if (phase == 2)
            {
                //HP・MPの数値表示処理

                if (chara_BeforeHp > chara_Hp)
                {
                    hpTemp -= hp_AmountOfDecrease / 60.0f * Time.deltaTime * 100;
                    chara_BeforeHp = (int)hpTemp;


                }
                if (chara_BeforeMp > chara_Mp)
                {


                    mpTemp -= mp_AmountOfDecrease / 60.0f * Time.deltaTime * 100;
                    chara_BeforeMp = (int)mpTemp;

                }

                hpText.text = "HP:" + (int)hpTemp + "/" + chara_MaxHp;
                mpText.text = "MP:" + chara_BeforeMp + "/" + chara_MaxMp;

                //ゲージ処理
                Vector2 gaugeLength = hpGauge.rectTransform.sizeDelta;
                gaugeLength.x = gauge_MaxLength * chara_BeforeHp / chara_MaxHp;
                hpGauge.rectTransform.sizeDelta = gaugeLength;

                gaugeLength = mpGauge.rectTransform.sizeDelta;
                gaugeLength.x = gauge_MaxLength * chara_BeforeMp / chara_MaxMp;
                mpGauge.rectTransform.sizeDelta = gaugeLength;

                if (chara_BeforeHp == chara_Hp && chara_BeforeMp == chara_Mp)
                {
                    hpGauge_DifferenceInlength = hp_BeforeLength - hpGauge.rectTransform.sizeDelta.x;
                    mpGauge_DifferenceInlength = mp_BeforeLength - mpGauge.rectTransform.sizeDelta.x;

                    phase = 3;
                }
            }
            if (phase == 3)
            {
                count++;
                if (count < 20)
                {
                    return;
                }
                hp_BeforeLength -= hpGauge_DifferenceInlength / 15.0f;
                mp_BeforeLength -= mpGauge_DifferenceInlength / 15.0f;
                if (hp_BeforeLength <= hpGauge.rectTransform.sizeDelta.x)
                {
                    hp_BeforeLength = hpGauge.rectTransform.sizeDelta.x;
                }
                if (mp_BeforeLength <= mpGauge.rectTransform.sizeDelta.x)
                {
                    mp_BeforeLength = mpGauge.rectTransform.sizeDelta.x;
                }

                Vector2 temp = hpGauge.rectTransform.sizeDelta;
                temp.x = hp_BeforeLength;
                hpGauge_Background.rectTransform.sizeDelta = temp;


                temp = mpGauge.rectTransform.sizeDelta;
                temp.x = mp_BeforeLength;
                mpGauge_Background.rectTransform.sizeDelta = temp;

                if (hpGauge.rectTransform.sizeDelta == hpGauge_Background.rectTransform.sizeDelta
                    && mpGauge.rectTransform.sizeDelta == mpGauge_Background.rectTransform.sizeDelta)
                {
                    phase = 0;

                    updateDisplaySwitch = false;
                }
            }

        }
    }

    public void Setup(MonsterBase monster)
    {
        this.monster = monster;
        chara_Name = monster.GetNickname();
        chara_Image = monster.GetImage();

        chara_Hp = monster.GetCurrentHPValue();
        chara_Mp = monster.GetCurrentMPValue();
        chara_MaxHp = monster.GetMaxHPValue();
        chara_MaxMp = monster.GetMaxMPValue();
        chara_BeforeHp = monster.GetCurrentHPValue();
        chara_BeforeMp = monster.GetCurrentMPValue();

        imageView = gameObject.transform.Find("chara").GetComponent<Image>();
        hpGauge = gameObject.transform.Find("HpGauge").GetComponent<Image>();
        mpGauge = gameObject.transform.Find("MpGauge").GetComponent<Image>();
        hpGauge_Background = gameObject.transform.Find("HpGauge_Background").GetComponent<Image>();
        mpGauge_Background = gameObject.transform.Find("MpGauge_Background").GetComponent<Image>();

        //////

        nameText = gameObject.transform.Find("NameText").GetComponent<Text>();
        imageView.sprite = chara_Image;
        hpText = gameObject.transform.Find("Hp").GetComponent<Text>();
        mpText = gameObject.transform.Find("Mp").GetComponent<Text>();

        nameText.text = chara_Name;

        Vector2 gaugeLength = hpGauge.rectTransform.sizeDelta;
        gaugeLength.x = gauge_MaxLength * chara_Hp / chara_MaxHp;
        hpGauge.rectTransform.sizeDelta = gaugeLength;

        gaugeLength = hpGauge_Background.rectTransform.sizeDelta;
        gaugeLength.x = gauge_MaxLength * chara_Hp / chara_MaxHp;
        hpGauge_Background.rectTransform.sizeDelta = gaugeLength;

        gaugeLength = mpGauge.rectTransform.sizeDelta;
        gaugeLength.x = gauge_MaxLength * chara_Mp / chara_MaxMp;
        mpGauge.rectTransform.sizeDelta = gaugeLength;

        gaugeLength = mpGauge_Background.rectTransform.sizeDelta;
        gaugeLength.x = gauge_MaxLength * chara_Mp / chara_MaxMp;
        mpGauge_Background.rectTransform.sizeDelta = gaugeLength;

        hpText.text = "HP:" + chara_Hp + "/" + chara_MaxHp;
        mpText.text = "MP:" + chara_Mp + "/" + chara_MaxMp;




    }

    /// <summary>
    /// ステータスの更新
    /// アニメーションさせる
    /// </summary>
    public void Update_DisplayStatus()
    {


        phase = 1;

        updateDisplaySwitch = true;

    }

    public void Popup_Display(bool onOff)
    {
        if (onOff == true)
        {
            Vector3 scaleSize = gameObject.transform.localScale;
            scaleSize.x = scale_Popup;
            scaleSize.y = scale_Popup;
            gameObject.transform.localScale = scaleSize;
        }
        else
        {

            Vector3 scaleSize = gameObject.transform.localScale;
            scaleSize.x = scale_Default;
            scaleSize.y = scale_Default;
            gameObject.transform.localScale = scaleSize;

        }
    }
}
