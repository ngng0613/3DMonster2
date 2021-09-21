using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraWork : MonoBehaviour
{
    Vector3 cameraDefaultPos;
    Vector3 cameraDefaultDir;

    //Player1ズーム
    Vector3 player1FixPos;
    Vector3 player1FixDir;

    Vector3 enemy1FixPos;
    Vector3 enemy1FixDir;

    [SerializeField] Transform player1_Pos;
    [SerializeField] Transform player2_Pos;
    [SerializeField] Transform player3_Pos;

    [SerializeField] Transform enemy1_Pos;
    [SerializeField] Transform enemy2_Pos;
    [SerializeField] Transform enemy3_Pos;

    [SerializeField] Transform defaultPos;


    [SerializeField] CinemachineVirtualCamera camera_V;
    


    // Start is called before the first frame update
    void Start()
    {

        //初期設定

        camera_V = GetComponent<CinemachineVirtualCamera>();
        Debug.Log(camera_V);

        cameraDefaultPos.x = 50;
        cameraDefaultPos.y = 12;
        cameraDefaultPos.z = 25;

        cameraDefaultDir.x = 45;
        cameraDefaultDir.y = 0;
        cameraDefaultDir.z = 0;

        player1FixPos.x = 53;
        player1FixPos.y = 3;
        player1FixPos.z = 40;

        player1FixDir.x = 30;
        player1FixDir.y = 225;
        player1FixDir.z = 0;

        enemy1FixPos.x = 53;
        enemy1FixPos.y = 3;
        enemy1FixPos.z = 45;

        enemy1FixDir.x = 30;
        enemy1FixDir.y = 315;
        enemy1FixDir.z = 0;




        //初期設定の定位置に移動
        ResetPos();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetPos()
    {
        camera_V.Follow = defaultPos;
        FastMove();
     
        gameObject.transform.localEulerAngles = cameraDefaultDir;
    }

    public void MoveCamera_EnemyMonster1()
    {
        DefaultSpeedMove();
        camera_V.Follow = enemy1_Pos;
        Debug.Log("1へ移動");

    }

    public void MoveCamera_EnemyMonster2()
    {
        DefaultSpeedMove();
        camera_V.Follow = enemy2_Pos;
        Debug.Log("2へ移動");

    }

    public void MoveCamera_EnemyMonster3()
    {
        DefaultSpeedMove();
        camera_V.Follow = enemy3_Pos;
        Debug.Log("3へ移動");
    }

    public void zoomPlayerMonster1()
    {
        FastMove();
        camera_V.Follow = player1_Pos;
        gameObject.transform.localEulerAngles = player1FixDir;
        /*
        gameObject.transform.localPosition = player1FixPos;
        gameObject.transform.localEulerAngles = player1FixDir;
        */
    }

    public void zoomPlayerMonster2()
    {
        FastMove();
        camera_V.Follow = player2_Pos;
        gameObject.transform.localEulerAngles = player1FixDir;
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
        camera_V.Follow = player3_Pos;
        gameObject.transform.localEulerAngles = player1FixDir;
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
        camera_V.Follow = enemy1_Pos;
        gameObject.transform.localEulerAngles = enemy1FixDir;
        /*
        gameObject.transform.localPosition = enemy1FixPos;
        gameObject.transform.localEulerAngles = enemy1FixDir;
        */
    }

    public void zoomEnemyMonster2()
    {
        FastMove();
        camera_V.Follow = enemy2_Pos;
        gameObject.transform.localEulerAngles = enemy1FixDir;

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
        camera_V.Follow = enemy3_Pos;
        gameObject.transform.localEulerAngles = enemy1FixDir;

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
            return enemy1FixDir;
        }
        else
        {
            return enemy1FixDir;
        }

    }

    public Vector3 GetPlayerDir(int playerNumber)
    {
        if (playerNumber == 1)
        {
            return player1FixPos;
        }
        else
        {
            return player1FixDir;
        }

    }

    public void DefaultSpeedMove()
    {
        camera_V.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0.5f;
        camera_V.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0.5f;
        camera_V.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0.5f;

    }

    public void FastMove()
    {
        camera_V.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0.1f;
        camera_V.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0.1f;
        camera_V.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0.1f;

    }
}
