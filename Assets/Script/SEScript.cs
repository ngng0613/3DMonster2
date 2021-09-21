using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEScript : MonoBehaviour
{
    AudioSource audioSource = null;

    bool isPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayed == false)
        {
            if (audioSource.isPlaying == true)
            {
                isPlayed = true;
            }
        }
        else
        {
            if (audioSource.isPlaying == false)
            {
                Destroy(gameObject);
            }
        }


    }
}
