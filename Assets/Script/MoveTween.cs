using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTween : MonoBehaviour
{
    public float speed = 100;
    public float destoryTime = 2;
    // Start is called before the first frame update
    void Start()
    {

        GameObject.Destroy(this.gameObject, destoryTime);

    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<RectTransform>().anchoredPosition +=
            new Vector2(0, speed * Time.deltaTime);
    }
}
