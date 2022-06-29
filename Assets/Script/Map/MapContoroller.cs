using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class MapContoroller : MonoBehaviour
{
    [SerializeField] bool IsDebug;
    [SerializeField] PlayerObject _playerObject;
    [SerializeField] CameraPlus _cameraPlus;
    [SerializeField] float _zoom = 1.0f;
    [SerializeField] GameObject _player;
    [SerializeField] float _playerMoveSpeed = 1.0f;
    [SerializeField] MapEvent[] _mapEventArray;
    [SerializeField] Fade _fade;
    Vector3 _moveToPos;
    bool _isMoving = false;
    [SerializeField] DeckManager _deckComposition;
    [SerializeField] ShopManager _shopManager;
    [SerializeField] Evolution _evolution;
    [Header("移動可能距離")]
    [SerializeField] float _moveLength = 1.0f;

    /// <summary>
    /// 次に移動可能なマスのリスト
    /// </summary>
    [SerializeField] List<MapEvent> _nextMapEventList;

    [SerializeField] MessageUi _restPointMessage;
    [SerializeField] string _battleScenenName;
    [SerializeField] ConfirmationScreen _confirmEvolve;

    [Header("ゲームクリア画面関連")]
    [SerializeField] GameResultCanvas _gameResultCanvas;
    [SerializeField] GameObject _gameOverObject;
    [SerializeField] GameObject _gameOverLogo;
    [Header("ゲームクリア表示速度")]
    [SerializeField] float _gameOverLogoChangeScaleSpeed;

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
            else if (mapEvent.GetType() == typeof(MapEventEvolve))
            {
                mapEvent.EventAction += EvolveEvent;
            }
            else if (mapEvent.GetType() == typeof(MapEventGoal))
            {
                mapEvent.EventAction += StageClear;
            }
        }
        _player.transform.position = GameManager.Instance.PlayeraPos;

        //プレイヤーが次に移動可能なマスを調べる
        StartCoroutine(SetupCoroutine());

        

    }

    IEnumerator SetupCoroutine()
    {
        yield return new WaitForSeconds(0.03f);
        UpdateNextMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (IsDebug == true)
            {
                IsDebug = false;
            }
            else
            {
                IsDebug = true;
            }
        }
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


        for (int i = 0; i < _mapEventArray.Length; i++)
        {
            _mapEventArray[i].IsActive = false;
        }


        _isMoving = true;
        _moveToPos = pos;
        StartCoroutine(MovePlayerCoroutine());
    }

    public IEnumerator MovePlayerCoroutine()
    {
        _playerObject.ListClear();
        for (int i = 0; i < _mapEventArray.Length; i++)
        {
            _mapEventArray[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < _mapEventArray.Length; i++)
        {
            _mapEventArray[i].gameObject.SetActive(true);
        }

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
        yield return new WaitForSeconds(0.01f);
        Debug.Log("hmm");
        if (IsDebug == true)
        {
            _isMoving = false;
            CanMoving();
            UpdateNextMap();
        }
        else
        {
            StartEvent();
        }
    }

    public void BattleStart()
    {
        Scene scene = SceneManager.GetActiveScene();
        GameManager.Instance.FieldMapName = scene.name;
        GameManager.Instance.PlayeraPos = _player.transform.position;
        _fade.AfterFunction += () => GameManager.Instance.ChangeScene(_battleScenenName);
        _fade.FadeOut();
    }

    public void RestPoint()
    {
        GameManager.Instance.PlayeraPos = _player.transform.position;
        GameManager.Instance.PlayerMaxHp = GameManager.Instance.PlayerMaxHp;
        _restPointMessage.Activate();
        _isMoving = false;
        UpdateNextMap();
    }

    public void EvolveEvent()
    {
        GameManager.Instance.PlayeraPos = _player.transform.position;
        _confirmEvolve.Activate();
        _isMoving = false;
        UpdateNextMap();
    }
    public void StageClear()
    {
        _gameOverObject.SetActive(true);
        //StartCoroutine(StageClearCoroutine());
        _gameResultCanvas.DisplayUpdate();
        _gameResultCanvas.gameObject.SetActive(true);
        _gameResultCanvas.AnimationStart();
    }

    public IEnumerator StageClearCoroutine()
    {
        Vector3 firstScale = _gameOverLogo.transform.localScale;

        while (true)
        {
            Vector2 tempScale = _gameOverLogo.transform.localScale;

            if (tempScale.x > 1 && firstScale.x > 1)
            {
                tempScale.x -= 1.0f * Time.deltaTime * _gameOverLogoChangeScaleSpeed;
                if (tempScale.x < 1)
                {
                    tempScale.x = 1;
                }
            }
            else if (tempScale.x < 1 && firstScale.x < 1)
            {
                tempScale.x += 1.0f * Time.deltaTime * _gameOverLogoChangeScaleSpeed;
                if (tempScale.x > 1)
                {
                    tempScale.x = 1;
                }
            }

            if (tempScale.y > 1 && firstScale.y > 1)
            {
                tempScale.y -= 1.0f * Time.deltaTime * _gameOverLogoChangeScaleSpeed;
                if (tempScale.y < 1)
                {
                    tempScale.y = 1;
                }
            }
            else if (tempScale.y < 1 && firstScale.y < 1)
            {
                tempScale.y += 1.0f * Time.deltaTime * _gameOverLogoChangeScaleSpeed;
                if (tempScale.y > 1)
                {
                    tempScale.y = 1;
                }
            }

            _gameOverLogo.transform.localScale = tempScale;
            if (tempScale == Vector2.one)
            {
                break;
            }
            yield return null;
        }
        _gameResultCanvas.DisplayUpdate();
        _gameResultCanvas.gameObject.SetActive(true);
        _gameResultCanvas.AnimationStart();

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
        StartCoroutine(UpdateNextMapCoroutine());
    }

    IEnumerator UpdateNextMapCoroutine()
    {
        yield return null;
        List<GameObject> tempList;
        tempList = _playerObject.EventsAround;
        Debug.Log($"tempList : {tempList.Count}");
        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i] == null)
            {
                continue;
            }
            MapEvent mapEvent = tempList[i].GetComponent<MapEvent>();
            mapEvent.IsActive = true;
            _nextMapEventList.Add(mapEvent);
            Debug.Log(tempList[i].gameObject.name + "の状態をアクティブにしました");
        }
        yield return null;
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene(GameManager.Instance.TitleName);
    }
}
