using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapContoroller : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] float _playerMoveSpeed = 1.0f;
    [SerializeField] MapEvent[] _mapEventArray;
    Vector3 _moveToPos;
    bool _isMoving = false;
    private void Start()
    {
        for (int i = 0; i < _mapEventArray.Length; i++)
        {
            _mapEventArray[i].MovePlayer = MovePlayer;
        }

    }

    public void MovePlayer(Vector3 pos)
    {
        if (_isMoving == true)
        {
            return;
        }
        _isMoving = true;
        _moveToPos = pos;
        StartCoroutine(MovePlayerCoroutine());
    }

    public IEnumerator MovePlayerCoroutine()
    {
        Vector3 startPos = _player.transform.position;
        float distance = Vector3.Distance(_player.transform.position, _moveToPos);
        float startTime = Time.time;
   

        while (true)
        {
            float t = (Time.time - startTime) / distance * _playerMoveSpeed;
            Vector3 movePos = Vector3.Lerp(startPos, _moveToPos, t);
            _player.transform.position = new Vector3 (movePos.x, movePos.y , Mathf.Cos(t) * -1 + 0.3f);
            if (t >= 1)
            {
                break;
            }

            yield return null;
        }
        _player.transform.position = new Vector3(_moveToPos.x, _moveToPos.y, startPos.z);
        _isMoving = false;
    }


}
