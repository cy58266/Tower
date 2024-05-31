using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public List<GameObject> prefabs;//����Ԥ��
    public int selectIndex;//��ǰѡ�������
    UIManager uiManager;
    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //�������Ļ��ת��Ϊ����
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //��������������
            if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("TowerBase")/*,QueryTriggerInteraction.Ignore*/))
            {
                //Debug.Log(hit);
                Debug.Log($"..........{hit.collider.gameObject.name}  {hit.collider.gameObject.tag}");
                //���ٽ������Ϸ����
                //GameObject.Destroy(hit.collider.gameObject);
                if (hit.collider.gameObject.CompareTag("TowerBase"))
                {
                    var tb = hit.collider.gameObject.GetComponent<TowerBase>();
                    if (tb != null && tb.tower == null)
                    {
                        var towerObject = GameObject.Instantiate(prefabs[selectIndex]);
                        var tower = towerObject.GetComponent<Tower>();

                        if (GameData.coins >= tower.expend)
                        {
                            towerObject.transform.position = tb.towerPosition.position;
                            //����Ƕȴ��� ���°ڷźò����� �����ĽǶȼ���
                            towerObject.transform.eulerAngles = hit.collider.transform.eulerAngles;
                            tb.tower = towerObject;
                            GameData.coins -= tower.expend;
                            //���½��
                            uiManager.UpdateBattleLevelData();

                        }
                        else
                        {
                            uiManager.CoinTips();
                        }
                    }

                }

            }
        }
    }
}
