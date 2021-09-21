using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Element
{
    public enum BattleElement
    {
        None,
        White,
        Black,
        Red,
        Blue,
        Yellow,
        Green,
    }


    public static float CheckAdvantage(BattleElement attackElement, BattleElement defenceElement)
    {
        switch (attackElement)
        {

            case BattleElement.White:

                if (defenceElement == BattleElement.Black)
                {
                    return 1.3f;
                }
                else if (defenceElement == BattleElement.Blue)
                {

                }
                break;
            case BattleElement.Black:
                if (defenceElement == BattleElement.White)
                {
                    return 1.3f;
                }

                break;
            case BattleElement.Red:
                if (defenceElement == BattleElement.Green)
                {
                    return 1.3f;
                }
                else if (defenceElement == BattleElement.Blue)
                {
                    return 0.8f;
                }
                break;
            case BattleElement.Blue:
                if (defenceElement == BattleElement.Red)
                {
                    return 1.3f;
                }
                else if (defenceElement == BattleElement.Yellow)
                {
                    return 0.8f;
                }
                break;
            case BattleElement.Yellow:
                if (defenceElement == BattleElement.Blue)
                {
                    return 1.3f;
                }
                else if (defenceElement == BattleElement.Green)
                {
                    return 0.8f;
                }
                break;
            case BattleElement.Green:
                if (defenceElement == BattleElement.Yellow)
                {
                    return 1.3f;
                }
                else if (defenceElement == BattleElement.Red)
                {
                    return 0.8f;
                }
                break;
            default:

                break;
        }
        return 1.0f;
    }
}
