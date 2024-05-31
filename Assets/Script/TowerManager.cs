using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public List<GameObject> prefabs;//炮塔预设
    public int selectIndex;//当前选择的索引
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
            //主相机屏幕点转换为射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //射线碰到了物体
            if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("TowerBase")/*,QueryTriggerInteraction.Ignore*/))
            {
                //Debug.Log(hit);
                Debug.Log($"..........{hit.collider.gameObject.name}  {hit.collider.gameObject.tag}");
                //销毁解除的游戏对象
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
                            //如果角度错误 重新摆放好参照物 塔基的角度即可
                            towerObject.transform.eulerAngles = hit.collider.transform.eulerAngles;
                            tb.tower = towerObject;
                            GameData.coins -= tower.expend;
                            //更新金币
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
