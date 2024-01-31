using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HP_Txt : MonoBehaviour
{
    public TextMeshProUGUI hpTxt;
    public Image fillImg;

    private int fullHP;

    private void Start()
    {
        fullHP = PlayerInfo.Instance.playerBaseInfo.hp;
    }

    private void Update()
    {
        hpTxt.text = "HP : " + PlayerInfo.Instance.playerBaseInfo.hp + "/" + fullHP;
        fillImg.fillAmount = (float)PlayerInfo.Instance.playerBaseInfo.hp / fullHP;
    }
}
