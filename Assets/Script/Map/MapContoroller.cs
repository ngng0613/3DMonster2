using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class MapContoroller : MonoBehaviour
{
    [SerializeField] PlayerObject _playerObject;
    [SerializeField] CameraPlus _cameraPlus;
    [SerializeField] float _zoom = 1.0f;
    [SerializeField] GameObject _player;
    [SerializeField] float _playerMoveSpeed = 1.0f;
    [SerializeField] MapEvent[] _mapEventArray;
    [SerializeField] Fade _fade;
    Vector3 _moveToPos;
    bool _isMoving = false;
    [SerializeField] DeckComposition _deckComposition;
    [SerializeField] ShopManager _shopManager;
    [Header("移動可能距離")]
    [SerializeField] float _moveLength = 1.0f;

    /// <summary>
    /// 次に移動可能なマスのリスト
    /// </summary>
    [SerializeField] List<MapEvent> _nextMapEventList;

    [SerializeField] MessageUi _restPointMessage;


    bool _dontMove = false;

    private void Start()
    {
        _shopManager.AfterFunc = CanMoving;
        _deckComposition.AfterFunc = CanMoving;

        for (int i = 0; i < _mapEventArray.Length; i++)
        {
            MapEvent mapEvent = _mapEventArray[i];
            mapEvent.MovePlayer = MovePlayer;
            //型チェック
            //もしバトルイベントなら
            if (mapEvent.GetType() == typeof(MapEventBattle))
            {
                mapEvent.EventAction += BattleStart;
            }
            else if (mapEvent.GetType() == typeof(MapEventRestPoint))
            {
                mapEvent.EventAction += RestPoint; 
            }
        }
        _player.transform.position = GameManager.Instance.PlayeraPos;

        //プレイヤーが次に移動可能なマスを調べる
        StartCoroutine(SetupCoroutine());

    }

    IEnumerator SetupCoroutine()
    {
        yield return null;
        UpdateNextMap();
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
    /// マップイベント開始
    /// </summary>
    public void StartEvent()
    {
        Debug.Log(_mapEventArray[1].transform.position + "    " + _player.transform.position);
        MapEvent mapEvent = _mapEventArray.Where(mEvent => (mEvent.transform.position.x == _player.transform.position.x) && (mEvent.transform.position.y == _player.transform.position.y)).FirstOrDefault();
        if (mapEvent != null)
        {
            mapEvent.StartEvent();
        }
    }

    public void MovePlayer(Vector3 pos)
    {
        if (_dontMove)
        {
            return;
        }
        if (_isMoving == true || (pos.x == _player.transform.position.x && pos.y == _player.transform.position.y))
        {
            return;
        }
        _playerObject.ListClear();
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

        StartEvent();
    }

    public void BattleStart()
    {
        GameManager.Instance.PlayeraPos = _player.transform.position;
        _fade.AfterFunction += () => GameManager.Instance.ChangeScene("CardBattle");
        _fade.StartAnimation();
    }

    public void RestPoint()
    {
        GameManager.Instance.PlayeraPos = _player.transform.position;
        _restPointMessage.Activate();
        _isMoving = false;
    }

    public void DeckCompositionActivate()
    {
        _deckComposition.gameObject.SetActive(true);
        _dontMove = true;
        _deckComposition.Activate();
    }

    public void ShopActivate()
    {
        _shopManager.gameObject.SetActive(true);
        _dontMove = true;
        _shopManager.Activate();
    }

    public void CanMoving()
    {
        _dontMove = false;
    }

    public void UpdateNextMap()
    {
        List<GameObject> tempList = _playerObject.EventsAround;
       
        for (int i = 0; i < tempList.Count; i++)
        {
            MapEvent mapEvent = tempList[i].GetComponent<MapEvent>();
            mapEvent.IsActive = true;
            _nextMapEventList.Add(mapEvent);
            Debug.Log(tempList[i].gameObject.name + "の状態をアクティブにしました");
        }
    }
}
