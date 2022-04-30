using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;




public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] float _fadeTime = 0.3f;

    ///
    /// SE一覧
    ///
    [SerializeField] AudioClip _gameStart;
    [SerializeField] AudioClip _battleStart;
    [SerializeField] AudioClip _select;
    [SerializeField] AudioClip _decided;
    [SerializeField] AudioClip _cancel;
    [SerializeField] AudioClip _openMenu;
    [SerializeField] AudioClip _closeMenu;
    [SerializeField] AudioClip _attack;
    [SerializeField] AudioClip _defence;


    public enum SeList
    {
        GameStart,
        BattleStart,
        Select,
        Decided,
        Cancel,
        OpenMenu,
        CloseMenu,
        Attack,
        Defence,

    }

    public void PlayBgm()
    {
        _audioSource.Play();
    }

    public void StopBgm()
    {
        _audioSource.DOFade(0.0f, _fadeTime);
    }

    public void PlaySe(SeList seList)
    {
        switch (seList)
        {
            case SeList.GameStart:
                _audioSource.PlayOneShot(_gameStart);
                break;
            case SeList.BattleStart:
                _audioSource.PlayOneShot(_battleStart);
                break;
            case SeList.Select:
                _audioSource.PlayOneShot(_select);
                break;
            case SeList.Decided:
                _audioSource.PlayOneShot(_decided);
                break;
            case SeList.Cancel:
                _audioSource.PlayOneShot(_cancel);
                break;
            case SeList.OpenMenu:
                _audioSource.PlayOneShot(_openMenu);
                break;
            case SeList.CloseMenu:
                _audioSource.PlayOneShot(_closeMenu);
                break;

            case SeList.Attack:
                _audioSource.PlayOneShot(_attack);
                break;
            case SeList.Defence:
                _audioSource.PlayOneShot(_defence);
                break;

            default:
                break;
        }

    }

    public void PlaySe(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

}
