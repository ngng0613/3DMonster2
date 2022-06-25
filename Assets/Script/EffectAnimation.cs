using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectAnimation : MonoBehaviour
{
    [SerializeField] Image _animationView;
    [SerializeField] List<AnimationData> _animationList = new List<AnimationData>();

    public void Start()
    {
        _animationView.color = Color.clear;
    }

    /// <summary>
    /// アニメーションの再生
    /// </summary>
    /// <param name="playId">再生するアニメのID</param>
    public void PlayAnimation(int playId)
    {
        if (playId >= _animationList.Count)
        {
            Debug.LogError($"指定されたPlayID{playId}はアニメーションリストの範囲外です");
            return;
        }

        StartCoroutine(PlayAnimationImpl(_animationList[playId]));
    }

    IEnumerator PlayAnimationImpl(AnimationData animationData)
    {
        _animationView.color = Color.white;
        for (int i = 0; i < animationData.SpriteAnimationArray.Length; i++)
        {
            _animationView.sprite = animationData.SpriteAnimationArray[i];
            _animationView.SetNativeSize();

            yield return new WaitForSeconds(animationData.AnimationDelay);
        }
        _animationView.color = Color.clear;

    }
}

[System.Serializable]
public class AnimationData
{
    public string Name;
    public Sprite[] SpriteAnimationArray;
    public float AnimationDelay = 1.0f;
}