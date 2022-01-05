using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusIconView : MonoBehaviour
{
    public List<StatusEffectBase> StatusEffectBaseList;
    public List<StateIcon> stateIconList;
    StateIcon stateIconPrefab;
    public void UpdateView()
    {
        stateIconList.Clear();
        for (int i = 0; i < StatusEffectBaseList.Count; i++)
        {
            StatusEffectBase effect = StatusEffectBaseList[i];
            StateIcon icon = Instantiate(stateIconPrefab);
            icon.Count = effect.Count;

        }
    }
}

