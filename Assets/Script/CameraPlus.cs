using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlus : MonoBehaviour
{
    [SerializeField] GameObject player; 
    public Vector3 offset;

    [SerializeField] bool talkMode = false;
    [SerializeField] bool talkNow = false;
    bool talkEnd = false;
    [SerializeField] int phase = 0;

    // Use this for initialization
    void Start()
    {
        transform.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (talkMode)
        {
            if (phase == 1)
            {
                //移動先のポジションを計算
                Vector3 tempPos = player.transform.position;
                Vector3 tempDir = player.transform.localEulerAngles;

                if (tempDir.y < 90 && tempDir.y > 270)
                {
                    tempPos.x += 2;

                }
                else
                {
                    tempPos.x += 2;

                }

                tempPos.y = 3;
                tempPos.z -= 2;

                tempDir.y = -45 /* + gameManager.GetPlayer().transform.localEulerAngles.y */;
                

                tempDir.x = 45;

                transform.position = tempPos;
                transform.localEulerAngles = tempDir;
                phase = 2;
            }
            if (phase == 2)
            {
                if (talkEnd)
                {

                    Vector3 tempVector = transform.localEulerAngles;
                    tempVector.x = 60;
                    tempVector.y = 0;
                    tempVector.z = 0;
                    transform.localEulerAngles = tempVector;

                    tempVector = transform.position;
                    tempVector.y = 6;
                    transform.position = tempVector;

                    talkEnd = false;
                    talkMode = false;
                    phase = 0;
                    //新しいトランスフォームの値を代入する
                    transform.position = player.transform.position + offset;

                }
            }
        }
        else
        {
            //新しいトランスフォームの値を代入する
            transform.position = player.transform.position + offset;
            
        }


    }

    public void TalkModeOn()
    {
        talkMode = true;
        phase = 1;

    }
    public void TalkModeOff()
    {
        if (!talkNow)
        {
            talkEnd = true;
        }
      
   
    }
    public void Set_Talk_Now_Flag(bool flag)
    {
        talkNow = flag;
    }
}
