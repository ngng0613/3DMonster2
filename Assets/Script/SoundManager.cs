using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;




public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] float fadeTime = 0.3f;

    ///
    /// SE一覧
    ///
    [SerializeField] AudioClip gameStart;
    [SerializeField] AudioClip battleStart;
    [SerializeField] AudioClip select;
    [SerializeField] AudioClip decided;
    [SerializeField] AudioClip cancel;
    [SerializeField] AudioClip openMenu;
    [SerializeField] AudioClip closeMenu;

    public enum SeList
    {
        GameStart,
        BattleStart,
        Select,
        Decided,
        Cancel,
        OpenMenu,
        CloseMenu,

    }

    public void PlayBgm()
    {
        audioSource.Play();
    }

    public void StopBgm()
    {
        audioSource.DOFade(0.0f, fadeTime);
    }

    public void PlaySe(SeList seList)
    {
        switch (seList)
        {
            case SeList.GameStart:
                audioSource.PlayOneShot(gameStart);
                break;
            case SeList.BattleStart:
                audioSource.PlayOneShot(battleStart);
                break;
            case SeList.Select:
                audioSource.PlayOneShot(select);
                break;
            case SeList.Decided:
                audioSource.PlayOneShot(decided);
                break;
            case SeList.Cancel:
                audioSource.PlayOneShot(cancel);
                break;
            case SeList.OpenMenu:
                audioSource.PlayOneShot(openMenu);
                break;
            case SeList.CloseMenu:
                audioSource.PlayOneShot(closeMenu);
                break;
            default:
                break;
        }

    }

    public void PlaySe(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
