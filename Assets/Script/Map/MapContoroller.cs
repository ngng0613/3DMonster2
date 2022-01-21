using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MapContoroller : MonoBehaviour
{
    [SerializeField] CameraPlus _cameraPlus;
    [SerializeField] float _zoom = 1.0f;
    [SerializeField] GameObject _player;
    [SerializeField] float _playerMoveSpeed = 1.0f;
    [SerializeField] MapEvent[] _mapEventArray;
    Vector3 _moveToPos;
    bool _isMoving = false;
    private void Start()
    {
        for (int i = 0; i < _mapEventArray.Length; i++)
        {
            MapEvent mEvent = _mapEventArray[i];
            mEvent.MovePlayer = MovePlayer;
            //型チェック
            //もしバトルイベントなら
            if (mEvent.GetType() == typeof(MapEventBattle))
            {
                mEvent.EventAction += BattleEvent;
            }
        }
    }

    private void Update()
    {
        if (Input.mouseScrollDelta != new Vector2(0.0f, 0.0f))
        {
            if (Input.mouseScrollDelta.y == 1.0f)
            {
                _zoom -= 0.1f;
            }
            if (Input.mouseScrollDelta.y == -1.0f)
            {
                _zoom += 0.1f;
            }
            if (_zoom < 0.3f)
            {
                _zoom = 0.3f;
            }
            if (_zoom > 3.0f)
            {
                _zoom = 3.0f;
            }
            _cameraPlus.offset.z = -3 * _zoom;
            _cameraPlus.offset.y = -2 * _zoom;
        }
    }

    /// <summary>
    /// マップイベントボタンが押された際に、プレイヤーの場所から該当するイベントを探す
    /// </summary>
    public void EventButton()
    {
        MapEvent mEvent = _mapEventArray.Where(mapEvent => mapEvent.transform.position == _player.transform.position) as MapEvent;

    }

    public void BattleEvent()
    {
        Debug.Log("バトル開始");
    }

    public void MovePlayer(Vector3 pos)
    {
        if (_isMoving == true || (pos.x == _player.transform.position.x && pos.y == _player.transform.position.y))
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
            _player.transform.position = new Vector3(movePos.x, movePos.y, Mathf.Cos(t) * -1 + 0.3f);
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
