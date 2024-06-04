using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Reflection;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    TowerManager towerManager;
    //第一步 先定义
    //主界面
    public Transform mainPage;
    Transform settingPage;
    Button startGameButton;//开始游戏按钮
    Button settingButton;//设置按钮
    Button quitButton;//退出按钮
    //战斗界面
    Transform battlePage;
    Transform gameTips;//游戏提示的界面
    Text gameTipsText;//游戏提示文本框
    Button pauseButton;//暂停按钮
    Button lastClick;//上次点击按钮
    public Sprite pauseSprite;//暂停按钮的图片
    public Sprite continueSprite;//继续按钮的图片
    Button[] towerButtons;//炮塔按钮列表

    Text HP;//水晶血量
    Text killCount;//击杀敌人数量
    Text coinsText;//金币数
    Text enemyCount;//剩余数量
    Text timeText;//时间

    //失败界面
    public Transform failPage;//失败界面
    Button returnButton;//返回按钮

    //成功界面
    public Transform winPage;

    public AudioSource starmusic;
    EnemyManager enemyManger;

    //关于游戏界面
    private void OnMouseUpAsButton()
    {


    }
    //第四步 创建点击方法
    // Start is called before the first frame update
    void Start()
    {
        towerManager = GameObject.Find("TowerManager").GetComponent<TowerManager>();
        towerManager.enabled = false;
        //第二步 初始化
        //主界面
        mainPage = transform.Find("MainPage");
        /*startGameButton = transform,Find("MainPage/btn Start").GetComponent<Button>().*/
        startGameButton = mainPage.Find("Btn_Start").GetComponent<Button>();
        settingButton = mainPage.Find("Btn_Settings").GetComponent<Button>();
        quitButton = mainPage.Find("Btn_Quit").GetComponent<Button>();

        //战斗界面
        battlePage = transform.Find("BattlePage");
        gameTips = transform.Find("BattlePage/GameTips");
        gameTipsText = gameTips.Find("Text").GetComponent<Text>();
        pauseButton = battlePage.Find("Btn_Pause").GetComponent<Button>();
        HP = transform.Find("BattlePage/HomeHP/Text").GetComponent<Text>();
        killCount = transform.Find("BattlePage/Kill/Text").GetComponent<Text>();
        coinsText = battlePage.Find("Coins/Text").GetComponent<Text>();
        enemyCount = battlePage.Find("EnemyCount/Text").GetComponent<Text>();
        timeText = battlePage.Find("Time").GetComponent<Text>();

        //设置界面
        settingPage = transform.Find("SettingsPage");


        //第三步 点击事件的
        startGameButton.onClick.AddListener(onStartGameButtonClick);
        settingButton.onClick.AddListener(onSettingGameButtonClick);
        quitButton.onClick.AddListener(onQuitGameButtonClick);
        pauseButton.onClick.AddListener(() =>
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                pauseButton.transform.GetComponent<Image>().sprite = continueSprite;
            }
            else
            {
                Time.timeScale = 1;
                pauseButton.transform.GetComponent<Image>().sprite = pauseSprite;
            }
        });
        Transform towerButtonParent = transform.Find("BattlePage/towerButtons");
        towerButtons = new Button[5];
        for (int i = 0; i < 5; i++)
        {
            //获取按钮 通过查找子物体
            towerButtons[i] = towerButtonParent.GetChild(i).GetComponent<Button>();
        }
        for (int i = 0; i < towerButtons.Length; i++)
        {
            var index = i;
            var btn = towerButtons[i];
            btn.onClick.AddListener(() =>
            {
                towerManager.selectIndex = index;
                if (lastClick != null)
                {
                    lastClick.GetComponent<Image>().color = new Color(0, 0, 0, 0.78f);
                }
                btn.gameObject.GetComponent<Image>().color = Color.yellow;
                lastClick = btn;
            });
        }

        towerManager.selectIndex = 0;
        towerButtons[0].gameObject.GetComponent<Image>().color = Color.yellow;

        //更新按钮的金币数
        for (int i = 0; i < towerManager.prefabs.Count; i++)
        {
            var icon = towerManager.prefabs[i].GetComponent<Text>().text = coinsText.ToString();
        }

        //失败界面
        failPage = transform.Find("FailPage");
        returnButton = transform.Find("FailPage/ReturnButton").GetComponent<Button>();

        //成功界面
        winPage = transform.Find("WinPage");
        returnButton = transform.Find("FailPage/ReturnButton").GetComponent<Button>();
    }

    //退出游戏响应方法
    private void onQuitGameButtonClick()
    {
        Debug.Log("onQuitGameButtonClick");
        Application.Quit();//游戏发布后，才会生效 自动关闭 退出APP
    }


    //游戏设置按钮
    private void onSettingGameButtonClick()
    {

        Debug.Log("onSettingGameButtonClick");

        //GameData.coins = 1;
        //Debug.Log(GameData.coins);

        mainPage.gameObject.SetActive(false);

        settingPage.gameObject.SetActive(true);

        //battlePage.gameObject.SetActive(true);

    }

    //返回主菜单
    public void returnStartPage()
    {
        //Debug.Log("asd");
        failPage.gameObject.SetActive(false);
        mainPage.gameObject.SetActive(true);
        GameData.SetLevelData(1);
        SceneManager.LoadScene(0);

    }

    public void returnStar()
    {

        winPage.gameObject.SetActive(false);
        mainPage.gameObject.SetActive(true);
        GameData.SetLevelData(1);
        SceneManager.LoadScene(0);

    }

    IEnumerator CountDown()
    {
        gameTips.gameObject.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            gameTipsText.text = $"怪兽即将到来{5 - i}";
            yield return new WaitForSeconds(1);
        }
        gameTipsText.text = "";
        gameTips.gameObject.SetActive(false);
        GameObject.Find("EnemyManager").GetComponent<EnemyManager>().enabled = true;
        while (GameData.HP > 0)
        {
            //yield return 的逻辑休克，协程要等待的操作
            yield return new WaitForSeconds(1);
            GameData.time += 1;
            float second = GameData.time % 60;
            timeText.text = $"{(int)(GameData.time / 60)}:" +
                $"{second.ToString("00")}";
        }
    }

    public void CoinTips()
    {
        var tips = GameObject.Instantiate(gameTips.gameObject);
        tips.transform.SetParent(gameTips.transform.parent);
        var txt = tips.transform.Find("Text").GetComponent<Text>();
        txt.text = "金币不足!";

        tips.transform.position = gameTips.transform.position;
        tips.gameObject.AddComponent<MoveTween>();
        tips.gameObject.SetActive(true);
    }
    public void UpdateBattleLevelData()
    {
        HP.text = GameData.HP.ToString();
        killCount.text = GameData.killCount.ToString();
        coinsText.text = GameData.coins.ToString();
        enemyCount.text = GameData.enemyCount.ToString();
        timeText.text = GameData.time.ToString();
        Debug.Log("hp" + HP.text + "coins" + coinsText.text + "enemyCount" + enemyCount.text);
    }
    public void ShowGameResult(bool win)
    {
        if (win == false)
        {
            failPage.gameObject.SetActive(true);
            Camera.main.GetComponent<CameraManager>().GameStop();
        }
        else
        {
            Debug.Log("游戏胜利！！");
            winPage.gameObject.SetActive(true);
        }
    }
    private void onStartGameButtonClick()
    {
        Debug.Log("onStartGameButtonClick");

        mainPage.gameObject.SetActive(false);

        battlePage.gameObject.SetActive(true);

        //GameData.SetLevelData(1);
        //更新UI界面
        UpdateBattleLevelData();
        timeText.text = "00:00";
        //启动一个协程，做游戏计时的工作
        //开始游戏倒计时
        StartCoroutine(CountDown());
        Camera.main.GetComponent<CameraManager>().GameStart();
        towerManager.enabled = true;

    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ShowGameResult(false);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            // 金币不足提示与原有失败提示共存，按需选择保留其中一个或全部
            CoinTips();
            // ShowGameResult(false);
        }

        //if (enemyManger.enemys.Count==0)
        //{
        //    ShowGameResult(true);
        //}
        
    }
}