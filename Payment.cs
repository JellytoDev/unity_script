using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DataInfo;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Storage;
using System;
using UnityEngine.Networking;

public class Payment : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject DialogBox;
    public GameObject BasketList;

    void Start()
    {
        string userName = "정종현"; // DataInfo.name;
        int total_price = 0;

        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("Payment Btn click");

            GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

            for(int i = 0; i < items.Length; i++)
            {
                total_price += int.Parse(items[i].transform.Find("price").GetComponent<Text>().text);
            
            }
            string name = "손민승";

            NHWrapper.instance.Transfer("손민승", "3020000005165", total_price.ToString(), (result, data) =>
            {
                Debug.Log(result);
                Debug.Log(data);

                BasketList.SetActive(false);

                DialogChange("결제 성공", "총 "+total_price.ToString()+" 원의 구매가 완료되었습니다.\n\b즐거운 쇼핑 바랍니다.");
                DialogBox.GetComponent<CanvasGroup>().alpha = 1;
            });
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DialogChange(string title, string content)
    {
        DialogBox.transform.Find("Header").Find("Title").GetComponent<Text>().text = title;
        DialogBox.transform.Find("Content").Find("Text").GetComponent<Text>().text = content;

    }
}
