using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineManager : MonoBehaviour
{

    /// <summary>
    /// タイムライン上を移動するオブジェクト
    /// </summary>
    [SerializeField] TimelineObject timelineObject;
    [SerializeField] GameObject objectParent;

    public float maxTime = 10000;
    [SerializeField] Vector3 positionTop;
    [SerializeField] Vector3 positionBottom;
    float timelineLength = 0;

    [SerializeField] List<MonsterBase> monsterBaseList = new List<MonsterBase>();
    [SerializeField] List<TimelineObject> timelineObjects = new List<TimelineObject>();
    [SerializeField] List<MonsterBase> actMonsterList = new List<MonsterBase>();
    [SerializeField] List<bool> deadList = new List<bool>();

    /// <summary>
    /// 受け取ったモンスターリストを元にタイムラインを作成する
    /// </summary>
    /// <param name="monsterBaseList">MonsterBaseのリスト</param>
    public void Setup(List<MonsterBase> monsterBaseList)
    {
        timelineLength = positionTop.y - positionBottom.y;
        this.monsterBaseList = monsterBaseList;
        foreach (var monster in this.monsterBaseList)
        {
            monster.coolTime = maxTime;
        }
        for (int i = 0; i < this.monsterBaseList.Count; i++)
        {
            TimelineObject addObject = Instantiate(timelineObject);
            addObject.transform.SetParent(objectParent.transform);
            addObject.transform.localPosition = positionTop;
            timelineObjects.Add(addObject);
            MonsterBase monster = this.monsterBaseList[i];
            addObject.UpdateImage(monster.GetImage());
            if (monster.charactorTag == BattleMonsterTag.CharactorTag.Enemy1 || monster.charactorTag == BattleMonsterTag.CharactorTag.Enemy2 || monster.charactorTag == BattleMonsterTag.CharactorTag.Enemy3)
            {
                addObject.ChangeSide(TimelineObject.Side.EnemySide);
            }
        }

        actMonsterList = new List<MonsterBase>();
    }

    /// <summary>
    /// タイムラインの更新。引数で時間を進めるか否かを指定。
    /// </summary>
    /// <param name="advanceTheTime">時間を進めるか否か</param>
    /// <returns></returns>
    public List<MonsterBase> UpdateTimeline(bool advanceTheTime)
    {

        actMonsterList = new List<MonsterBase>();

        for (int i = 0; i < monsterBaseList.Count; i++)
        {
            if (advanceTheTime)
            {
                //モンスターの素早さぶん、クールタイムを進める
                monsterBaseList[i].coolTime -= monsterBaseList[i].GetSpeedValue() / 20;
            }

            //クールタイムが減った分だけ、タイムラインオブジェクトを移動する

            Vector3 pos = timelineObjects[i].transform.localPosition;
            pos.y = positionBottom.y + (timelineLength * monsterBaseList[i].coolTime / maxTime);

            if (pos.y < positionBottom.y)
            {
                pos.y = positionBottom.y;
            }
            timelineObjects[i].transform.localPosition = pos;

            if (monsterBaseList[i].coolTime <= 0)
            {
                actMonsterList.Add(monsterBaseList[i]);
            }

        }

        if (actMonsterList.Count > 0)
        {
            return actMonsterList;
        }

        return null;
    }

    public void DeleteMonster(MonsterBase deadMonster)
    {
        for (int i = 0; i < monsterBaseList.Count; i++)
        {
            if (monsterBaseList[i] == deadMonster)
            {
                Destroy(timelineObjects[i].gameObject);
                timelineObjects.RemoveAt(i);
                monsterBaseList.RemoveAt(i);
                for (int k = 0; k < actMonsterList.Count; k++)
                {
                    if (actMonsterList[k] == deadMonster)
                    {
                        actMonsterList.Remove(deadMonster);
                    }
                }

            }
        }
    }

    /// <summary>
    /// 指定されたモンスターを一番先頭に表示する
    /// 
    /// </summary>
    public void MoveImageToFront(MonsterBase monster)
    {
        for (int i = 0; i < monsterBaseList.Count; i++)
        {
            if (monsterBaseList[i] == monster)
            {
                timelineObjects[i].transform.SetAsLastSibling();
            }
        }

    }
}
