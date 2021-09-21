using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    //一旦localEularAngle前提でやる
    Vector3 nextDir = new Vector3();

    //動くテンポ　ここで設定した数値よりupdate回数が上回ると、ランダムで回転処理が動く。
    [SerializeField] int moveTempo = 500;

    int updateCount = 0;

    int rotateSpeed = 50;

    [SerializeField] int movePhase = 0;

    Quaternion quaternion;

    float timeCount = 0;
    int walkCount = 0;

    [SerializeField] int moveSpeed = 3;
    [SerializeField] MonsterBase thisMonsterBase;

    // Start is called before the first frame update
    void Start()
    {
        thisMonsterBase = gameObject.GetComponent<MonsterBase>();
    }

    // Update is called once per frame
    void Update()
    {
        updateCount++;

        if (updateCount >= moveTempo)
        {
            if (movePhase == 0)
            {
                if (Random.Range(0, 100) >= 35)
                {

                    nextDir = transform.localEulerAngles;
                    nextDir.y += Random.Range(30, 330);
                    if (nextDir.y >= 360)
                    {
                        nextDir.y -= 360;
                    }
               

                    quaternion = Quaternion.Euler(nextDir);
                

                    movePhase++;
                }
                else
                {
                    updateCount = 0;
                }
            }
            if (movePhase == 1)
            {
                thisMonsterBase.MotionMove();
                transform.rotation = Quaternion.Slerp(this.transform.rotation, quaternion, timeCount);
                timeCount += Time.deltaTime / 5;

                //Debug.Log(transform.rotation.ToString() + "←現在 " + quaternion.ToString() + "←理想");
                if (transform.eulerAngles.y >= quaternion.eulerAngles.y - 0.5f && transform.eulerAngles.y <= quaternion.eulerAngles.y + 0.5f)
                {
                    movePhase++;
                }
                if (timeCount >= 0.8f)
                {
                    thisMonsterBase.MotionIdle();
                    timeCount = 0;
                    movePhase = 0;
                    updateCount = 0;
                }
            }
            if (movePhase == 2)
            {
                thisMonsterBase.MotionMove();
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
                walkCount++;
                if (walkCount >= 90)
                {
                    walkCount = 0;
                    thisMonsterBase.MotionIdle();
                    movePhase++;
                }

            }
            if (movePhase == 3)
            {

                timeCount = 0;
                movePhase = 0;
                updateCount = 0;
            }


        }

    }
}
