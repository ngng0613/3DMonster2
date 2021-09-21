using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{

    [SerializeField] GameObject seObject;
    [SerializeField] AudioClip audioClip;
    [SerializeField] AudioSource audioSource;

    [SerializeField] List<AudioClip> audioList = new List<AudioClip>();
    public enum AudioType
    {
        EnterButton = 1,
        CancelButton = 2,
        Cursor = 3,
        BattleCursor = 4,
        Error = 5,

        SendChar = 7,

        BattleStart = 10,

        Punch = 50,
        Magic_Fire = 51,
        Magic_Thunder = 52,
    }
    AudioType audioType;



    // Start is called before the first frame update
    void Start()
    {
        //gameManager = this.gameObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlaySound(AudioClip clip)
    {

        GameObject seInstanceObject = Instantiate(seObject);
        seInstanceObject.GetComponent<AudioSource>().clip = clip;
        seInstanceObject.GetComponent<AudioSource>().Play();


    }
    public void PlaySoundDefault(AudioType type)
    {
        if (audioList[(int)type] == null)
        {
            return;
        }
        GameObject seInstanceObject = Instantiate(seObject);
        seInstanceObject.GetComponent<AudioSource>().clip = audioList[(int)type];
        seInstanceObject.GetComponent<AudioSource>().Play();
    }
}
