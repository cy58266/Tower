using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{

    public InputField coins;
    public InputField emenyCount;
    public InputField hp;

    public bool co;
    public bool em;
    public bool hpl;


    public GameObject mainPage;
    public GameObject settingPage;

    // Start is called before the first frame update
    void Start()
    {
        Cionschange();
        EnemyChange();
        Hpchange();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Cionschange()
    {

        //Debug.Log("0" + coins);
        //Debug.Log("1" + GameData.coins);

        float.TryParse(coins.text,out GameData.coins);
        co = true;

        //Debug.Log("@" + GameData.coins);

    }

    public void EnemyChange()
    {
        int.TryParse(emenyCount.text, out GameData.enemyCount);
        em = true;
    }

    public void Hpchange()
    {
        float.TryParse(hp.text, out GameData.HP);
        hpl = true;
    }


    public void returnMainPage()
    {
        settingPage.SetActive(false);
        mainPage.SetActive(true);
    }


}
