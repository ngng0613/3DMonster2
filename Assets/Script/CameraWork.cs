using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraWork : MonoBehaviour
{
    Vector3 _cameraDefaultPos;
    Vector3 _cameraDefaultDir;

    //Player1ズーム
    Vector3 _player1FixPos;
    Vector3 _player1FixDir;

    Vector3 _enemy1FixPos;
    Vector3 _enemy1FixDir;

    [SerializeField] Transform _player1Pos;
    [SerializeField] Transform _player2Pos;
    [SerializeField] Transform _player3Pos;

    [SerializeField] Transform _enemy1Pos;
    [SerializeField] Transform _enemy2Pos;
    [SerializeField] Transform _enemy3Pos;

    [SerializeField] Transform _defaultPos;


    [SerializeField] CinemachineVirtualCamera _cameraV;
    


    // Start is called before the first frame update
    void Start()
    {

        //初期設定

        _cameraV = GetComponent<CinemachineVirtualCamera>();
        Debug.Log(_cameraV);

        _cameraDefaultPos.x = 50;
        _cameraDefaultPos.y = 12;
        _cameraDefaultPos.z = 25;

        _cameraDefaultDir.x = 45;
        _cameraDefaultDir.y = 0;
        _cameraDefaultDir.z = 0;

        _player1FixPos.x = 53;
        _player1FixPos.y = 3;
        _player1FixPos.z = 40;

        _player1FixDir.x = 30;
        _player1FixDir.y = 225;
        _player1FixDir.z = 0;

        _enemy1FixPos.x = 53;
        _enemy1FixPos.y = 3;
        _enemy1FixPos.z = 45;

        _enemy1FixDir.x = 30;
        _enemy1FixDir.y = 315;
        _enemy1FixDir.z = 0;




        //初期設定の定位置に移動
        ResetPos();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetPos()
    {
        _cameraV.Follow = _defaultPos;
        FastMove();
     
        gameObject.transform.localEulerAngles = _cameraDefaultDir;
    }

    public void MoveCamera_EnemyMonster1()
    {
        DefaultSpeedMove();
        _cameraV.Follow = _enemy1Pos;
        Debug.Log("1へ移動");

    }

    public void MoveCamera_EnemyMonster2()
    {
        DefaultSpeedMove();
        _cameraV.Follow = _enemy2Pos;
        Debug.Log("2へ移動");

    }

    public void MoveCamera_EnemyMonster3()
    {
        DefaultSpeedMove();
        _cameraV.Follow = _enemy3Pos;
        Debug.Log("3へ移動");
    }

    public void zoomPlayerMonster1()
    {
        FastMove();
        _cameraV.Follow = _player1Pos;
        gameObject.transform.localEulerAngles = _player1FixDir;
        /*
        gameObject.transform.localPosition = player1FixPos;
        gameObject.transform.localEulerAngles = player1FixDir;
        */
    }

    public void zoomPlayerMonster2()
    {
        FastMove();
        _cameraV.Follow = _player2Pos;
        gameObject.transform.localEulerAngles = _player1FixDir;
        /*
        Vector3 v_temp = player1FixPos;
        v_temp.x -= 6;
        gameObject.transform.localPosition = v_temp;
        ga0meObject.transform.localEulerAngles = player1FixDir;
        */
    }

    public void zoomPlayerMonster3()
    {
        FastMove();
        _cameraV.Follow = _player3Pos;
        gameObject.transform.localEulerAngles = _player1FixDir;
        /*
        Vector3 v_temp = player1FixPos;
        v_temp.x += 6;
        gameObject.transform.localPosition = v_temp;
        gameObject.transform.localEulerAngles = player1FixDir;
        */
    }

    public void zoomEnemyMonster1()
    {
        FastMove();
        _cameraV.Follow = _enemy1Pos;
        gameObject.transform.localEulerAngles = _enemy1FixDir;
        /*
        gameObject.transform.localPosition = enemy1FixPos;
        gameObject.transform.localEulerAngles = enemy1FixDir;
        */
    }

    public void zoomEnemyMonster2()
    {
        FastMove();
        _cameraV.Follow = _enemy2Pos;
        gameObject.transform.localEulerAngles = _enemy1FixDir;

        /*
        Vector3 v_temp = enemy1FixPos;
        v_temp.x += 6;
        gameObject.transform.localPosition = v_temp;
        gameObject.transform.localEulerAngles = enemy1FixDir;
        */
    }

    public void zoomEnemyMonster3()
    {
        FastMove();
        _cameraV.Follow = _enemy3Pos;
        gameObject.transform.localEulerAngles = _enemy1FixDir;

        /*
        Vector3 v_temp = enemy1FixPos;
        v_temp.x -= 6;
        gameObject.transform.localPosition = v_temp;
        gameObject.transform.localEulerAngles = enemy1FixDir;
        */

    }

    public Vector3 GetEnemyDir(int enemyNumber)
    {
        if (enemyNumber == 1)
        {
            return _enemy1FixDir;
        }
        else
        {
            return _enemy1FixDir;
        }

    }

    public Vector3 GetPlayerDir(int playerNumber)
    {
        if (playerNumber == 1)
        {
            return _player1FixPos;
        }
        else
        {
            return _player1FixDir;
        }

    }

    public void DefaultSpeedMove()
    {
        _cameraV.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0.5f;
        _cameraV.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0.5f;
        _cameraV.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0.5f;

    }

    public void FastMove()
    {
        _cameraV.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0.1f;
        _cameraV.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0.1f;
        _cameraV.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0.1f;

    }
}
