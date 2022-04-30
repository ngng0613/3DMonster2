using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationScreen : MonoBehaviour
{
    public void Activate()
    {
        this.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }
}
