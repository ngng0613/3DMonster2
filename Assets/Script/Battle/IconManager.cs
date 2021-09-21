using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    public Sprite iconNeutral;
    public Sprite iconRed;
    public Sprite iconBlue;
    public Sprite iconYellow;
    public Sprite iconGreen;

    public Sprite GetIconImage(Element.BattleElement battleElement)
    {
        Sprite returnSprite = iconNeutral;
        switch (battleElement)
        {
            case Element.BattleElement.White:
                break;
            case Element.BattleElement.Black:
                break;
            case Element.BattleElement.Red:
                returnSprite = iconRed;
                break;
            case Element.BattleElement.Blue:
                returnSprite = iconBlue;
                break;
            case Element.BattleElement.Yellow:
                returnSprite = iconYellow;
                break;
            case Element.BattleElement.Green:
                returnSprite = iconGreen;
                break;
            default:
                break;
        }
        return returnSprite;
    }
}
