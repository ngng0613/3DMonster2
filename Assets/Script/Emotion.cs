using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Emotion : MonoBehaviour
{
    public bool activeSwitch;
    Camera camera = null;

    [SerializeField] GameObject emotion_Fukidashi;
    [SerializeField] GameObject emotion_Icon;

    [SerializeField] Sprite emotion_SavePoint;
    [SerializeField] Sprite emotion_Quest;
    [SerializeField] Sprite emotion_Bikkuri;
    

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Canvas>().worldCamera)
        {
            camera = GetComponent<Canvas>().worldCamera;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activeSwitch)
        {
            emotion_Fukidashi.SetActive(true);
            emotion_Icon.gameObject.SetActive(true);
            /*
            //transform.LookAt(camera.transform);
            Vector3 tempDir = transform.localEulerAngles;
            */
            transform.forward = transform.position - camera.transform.position;

        }
        else
        {
            emotion_Fukidashi.SetActive(false);
            emotion_Icon.gameObject.SetActive(false);
        }

    }

    public void Emotion_Quest()
    {
        transform.Find("Emotion_Icon").GetComponent<Image>().sprite = emotion_Quest;
    }
    public void Emotion_Bikkuri()
    {
        transform.Find("Emotion_Icon").GetComponent<Image>().sprite = emotion_Bikkuri;
    }
}
