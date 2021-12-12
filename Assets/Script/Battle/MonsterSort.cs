using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSort : MonoBehaviour
{
    public List<MonsterBase> turnListMonsterBase { get; set; }
    List<BattleMonsterTag.CharactorTag> turnListCharactorNumber;

    public void QuickSort(int left, int right)
    {
        if (left == right)
        {
            return;
        }

        int i = 0;
        int k = 0;

        while (true)
        {
            //もし条件を満たす添え字があった場合、ここに保存
            int changeA = -1;
            int changeB = -1;

            if (SpeedAndDelayCalculation(turnListMonsterBase[left + i]) <= SpeedAndDelayCalculation(turnListMonsterBase[left]))
            {
                changeA = left + i;
            }
            else
            {
                i++;
            }

            //調べている箇所が同じ場所を指していないなら
            if (left + i < right - k)
            {
                //未満
                if (SpeedAndDelayCalculation(turnListMonsterBase[left]) > SpeedAndDelayCalculation(turnListMonsterBase[right - k]))
                {
                    changeB = right - k;
                }
                else
                {
                    k++;
                }
            }
            //それぞれピポット以上とピポット未満が見つかったら入れ替え
            if (changeA != -1 && changeB != -1)
            {
                MonsterBase temp = turnListMonsterBase[changeA];
                turnListMonsterBase[changeA] = turnListMonsterBase[changeB];
                turnListMonsterBase[changeB] = temp;

                //差が0より大きい場合、iとk、それぞれの値を+1する。
                if ((right - k) - (left + i) > 0)
                {
                    i++;
                }
                if ((right - k) - (left + i) > 0)
                {
                    k++;
                }
            }
            if (left + i == right - k)
            {
                if (left + i == left)
                {
                    //もしピポットが一番左にあるなら、そこから右の範囲だけ再帰
                    QuickSort(left + i + 1, right);
                    break;
                }
                else
                {
                    //もしピポットがleftより大きいならピポットより左部分で再帰
                    if (left + i > left)
                    {
                        QuickSort(left, left + i - 1);
                    }
                    //ピポットを含む右の範囲で再帰
                    QuickSort(left + i, right);
                }
                break;
            }
        }
    }



    /// <summary>
    /// モンスターのスピード値とディレイ値をかけ合わせた際の計算を行う
    /// </summary>
    /// <param name="monster">対象モンスターのモンスターベース</param>
    /// <returns></returns>
    float SpeedAndDelayCalculation(MonsterBase monster)
    {
        float speed = monster.GetSpeedValue();
        float delay = monster._attackDelay;
        return speed * delay;
    }
}
