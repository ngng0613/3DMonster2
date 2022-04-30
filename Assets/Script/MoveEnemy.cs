using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    //一旦localEularAngle前提でやる
    Vector3 _nextDir = new Vector3();

    //動くテンポ　ここで設定した数値よりupdate回数が上回ると、ランダムで回転処理が動く。
    [SerializeField] int _moveTempo = 500;

    int _updateCount = 0;

    int _rotateSpeed = 50;

    [SerializeField] int _movePhase = 0;

    Quaternion _quaternion;

    float _timeCount = 0;
    int _walkCount = 0;

    [SerializeField] int _moveSpeed = 3;
    [SerializeField] MonsterBase _thisMonsterBase;

    // Start is called before the first frame update
    void Start()
    {
        _thisMonsterBase = gameObject.GetComponent<MonsterBase>();
    }

    // Update is called once per frame
    void Update()
    {
        _updateCount++;

        if (_updateCount >= _moveTempo)
        {
            if (_movePhase == 0)
            {
                if (Random.Range(0, 100) >= 35)
                {

                    _nextDir = transform.localEulerAngles;
                    _nextDir.y += Random.Range(30, 330);
                    if (_nextDir.y >= 360)
                    {
                        _nextDir.y -= 360;
                    }
               

                    _quaternion = Quaternion.Euler(_nextDir);
                

                    _movePhase++;
                }
                else
                {
                    _updateCount = 0;
                }
            }
            if (_movePhase == 1)
            {
                _thisMonsterBase.MotionMove();
                transform.rotation = Quaternion.Slerp(this.transform.rotation, _quaternion, _timeCount);
                _timeCount += Time.deltaTime / 5;

                //Debug.Log(transform.rotation.ToString() + "←現在 " + quaternion.ToString() + "←理想");
                if (transform.eulerAngles.y >= _quaternion.eulerAngles.y - 0.5f && transform.eulerAngles.y <= _quaternion.eulerAngles.y + 0.5f)
                {
                    _movePhase++;
                }
                if (_timeCount >= 0.8f)
                {
                    _thisMonsterBase.MotionIdle();
                    _timeCount = 0;
                    _movePhase = 0;
                    _updateCount = 0;
                }
            }
            if (_movePhase == 2)
            {
                _thisMonsterBase.MotionMove();
                transform.position += transform.forward * _moveSpeed * Time.deltaTime;
                _walkCount++;
                if (_walkCount >= 90)
                {
                    _walkCount = 0;
                    _thisMonsterBase.MotionIdle();
                    _movePhase++;
                }

            }
            if (_movePhase == 3)
            {

                _timeCount = 0;
                _movePhase = 0;
                _updateCount = 0;
            }


        }

    }
}
