using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorEffectGenerator : MonoBehaviour
{
    [SerializeField] FloorEffect[] _floorEffect;
    [SerializeField] Canvas _generateCanvas;
    [SerializeField] float _generateSpeed = 3f;
    float _timeCount = 0;
    Transform _generatePos;
    int _generateCount = 0;

    private void Start()
    {
        _generateCanvas = GameObject.FindWithTag("FloorCanvas").GetComponent<Canvas>();
    }

    public void OnUpdate(Transform generateTransform)
    {
        _generatePos = generateTransform;
        _timeCount += Time.deltaTime;
        if (_timeCount >= 1.0f / _generateSpeed)
        {
            _timeCount = 0;
            Generate(this._generatePos.position);
        }
    }

    // Update is called once per frame
    void Generate(Vector3 generatePos)
    {
        FloorEffect newEffect = Instantiate(_floorEffect[_generateCount]);
        newEffect.transform.position = generatePos;
        newEffect.transform.position += new Vector3(0, 0.15f, 0);
        newEffect.transform.SetParent(_generateCanvas.transform);
        _generateCount++;
        if (_floorEffect.Length <= _generateCount)
        {
            _generateCount = 0;
        }

    }
}
