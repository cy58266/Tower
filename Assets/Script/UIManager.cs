using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Reflection;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    TowerManager towerManager;
    //��һ�� �ȶ���
    //������
    public Transform mainPage;
    Transform settingPage;
    Button startGameButton;//��ʼ��Ϸ��ť
    Button settingButton;//���ð�ť
    Button quitButton;//�˳���ť
    //ս������
    Transform battlePage;
    Transform gameTips;//��Ϸ��ʾ�Ľ���
    Text gameTipsText;//��Ϸ��ʾ�ı���
    Button pauseButton;//��ͣ��ť
    Button lastClick;//�ϴε����ť
    public Sprite pauseSprite;//��ͣ��ť��ͼƬ
    public Sprite continueSprite;//������ť��ͼƬ
    Button[] towerButtons;//������ť�б�

    Text HP;//ˮ��Ѫ��
    Text killCount;//��ɱ��������
    Text coinsText;//�����
    Text enemyCount;//ʣ������
    Text timeText;//ʱ��

    //ʧ�ܽ���
    public Transform failPage;//ʧ�ܽ���
    Button returnButton;//���ذ�ť

    //�ɹ�����
    public Transform winPage;

    public AudioSource starmusic;
    EnemyManager enemyManger;

    //������Ϸ����
    private void OnMouseUpAsButton()
    {


    }
    //���Ĳ� �����������
    // Start is called before the first frame update
    void Start()
    {
        towerManager = GameObject.Find("TowerManager").GetComponent<TowerManager>();
        towerManager.enabled = false;
        //�ڶ��� ��ʼ��
        //������
        mainPage = transform.Find("MainPage");
        /*startGameButton = transform,Find("MainPage/btn Start").GetComponent<Button>().*/
        startGameButton = mainPage.Find("Btn_Start").GetComponent<Button>();
        settingButton = mainPage.Find("Btn_Settings").GetComponent<Button>();
        quitButton = mainPage.Find("Btn_Quit").GetComponent<Button>();

        //ս������
        battlePage = transform.Find("BattlePage");
        gameTips = transform.Find("BattlePage/GameTips");
        gameTipsText = gameTips.Find("Text").GetComponent<Text>();
        pauseButton = battlePage.Find("Btn_Pause").GetComponent<Button>();
        HP = transform.Find("BattlePage/HomeHP/Text").GetComponent<Text>();
        killCount = transform.Find("BattlePage/Kill/Text").GetComponent<Text>();
        coinsText = battlePage.Find("Coins/Text").GetComponent<Text>();
        enemyCount = battlePage.Find("EnemyCount/Text").GetComponent<Text>();
        timeText = battlePage.Find("Time").GetComponent<Text>();

        //���ý���
        settingPage = transform.Find("SettingsPage");


        //������ ����¼���
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
            //��ȡ��ť ͨ������������
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

        //���°�ť�Ľ����
        for (int i = 0; i < towerManager.prefabs.Count; i++)
        {
            var icon = towerManager.prefabs[i].GetComponent<Text>().text = coinsText.ToString();
        }

        //ʧ�ܽ���
        failPage = transform.Find("FailPage");
        returnButton = transform.Find("FailPage/ReturnButton").GetComponent<Button>();

        //�ɹ�����
        winPage = transform.Find("WinPage");
        returnButton = transform.Find("FailPage/ReturnButton").GetComponent<Button>();
    }

    //�˳���Ϸ��Ӧ����
    private void onQuitGameButtonClick()
    {
        Debug.Log("onQuitGameButtonClick");
        Application.Quit();//��Ϸ�����󣬲Ż���Ч �Զ��ر� �˳�APP
    }


    //��Ϸ���ð�ť
    private void onSettingGameButtonClick()
    {

        Debug.Log("onSettingGameButtonClick");

        //GameData.coins = 1;
        //Debug.Log(GameData.coins);

        mainPage.gameObject.SetActive(false);

        settingPage.gameObject.SetActive(true);

        //battlePage.gameObject.SetActive(true);

    }

    //�������˵�
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
            gameTipsText.text = $"���޼�������{5 - i}";
            yield return new WaitForSeconds(1);
        }
        gameTipsText.text = "";
        gameTips.gameObject.SetActive(false);
        GameObject.Find("EnemyManager").GetComponent<EnemyManager>().enabled = true;
        while (GameData.HP > 0)
        {
            //yield return ���߼��ݿˣ�Э��Ҫ�ȴ��Ĳ���
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
        txt.text = "��Ҳ���!";

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
            Debug.Log("��Ϸʤ������");
            winPage.gameObject.SetActive(true);
        }
    }
    private void onStartGameButtonClick()
    {
        Debug.Log("onStartGameButtonClick");

        mainPage.gameObject.SetActive(false);

        battlePage.gameObject.SetActive(true);

        //GameData.SetLevelData(1);
        //����UI����
        UpdateBattleLevelData();
        timeText.text = "00:00";
        //����һ��Э�̣�����Ϸ��ʱ�Ĺ���
        //��ʼ��Ϸ����ʱ
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
            // ��Ҳ�����ʾ��ԭ��ʧ����ʾ���棬����ѡ��������һ����ȫ��
            CoinTips();
            // ShowGameResult(false);
        }

        //if (enemyManger.enemys.Count==0)
        //{
        //    ShowGameResult(true);
        //}
        
    }
}