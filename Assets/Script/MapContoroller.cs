using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapContoroller : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] Tilemap _tileMap;

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 tempPos = Camera.main.ScreenToWorldPoint(mousePos);
        

    }

}
