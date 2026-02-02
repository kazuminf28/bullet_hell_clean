using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditUI : MonoBehaviour
{
    [SerializeField] GameObject creditPanel;

    public void ShowCredit()
    {
        creditPanel.SetActive(true);
    }

    public void HideCredit()
    {
        creditPanel.SetActive(false);
    }
}
