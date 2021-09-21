using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineObject : MonoBehaviour
{
    [SerializeField] Image monsterImage;
    public enum Side
    {
        PlayerSide,
        EnemySide
    }
    [SerializeField] Image backgroundImage;
    [SerializeField] Color playerSideColor;
    [SerializeField] Color enemySideColor;


    /// <summary>
    /// モンスター画像の更新
    /// </summary>
    /// <param name="sprite">変更後のモンスター画像</param>
    public void UpdateImage(Sprite sprite)
    {
        monsterImage.sprite = sprite;
    }

    /// <summary>
    /// 味方側か敵側か設定する
    /// </summary>
    /// <param name="side">味方 or 敵</param>
    public void ChangeSide(Side side)
    {
        if (side == Side.PlayerSide)
        {
            backgroundImage.color = playerSideColor;
        }
        else if (side == Side.EnemySide)
        {
            backgroundImage.color = enemySideColor;
        }
    }
}
