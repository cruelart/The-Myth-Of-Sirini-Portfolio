using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraderStoreUI : UIBase
{
    [SerializeField]
    private List<BaseitemDB> sellItemList; // 판매할 아이템들 목록

    [SerializeField]
    private StoreUISlot storeUISlotPrefab; // 판매아이템 슬롯 프리팹 넣는 곳

    [SerializeField]
    private Transform storeUISlotTransform; // 위치설정

    [SerializeField]
    private TextMeshProUGUI timeText; // 타이머 텍스트

    private List<StoreUISlot> storeUISlots = new();

    [Header("판매 슬롯 갯수")]
    [SerializeField]
    private int slotNum = 21; // 웬만하면 7의 배수로 ㄱ 6의배수로 하면 꼬이더라

    [Header("갱신 시간 설정")]
    [SerializeField]
    private float initTime = 30.0f; // 1시간으로 일단 설정

    private int endTime;
    

    private void Awake()
    {this.transform.SetAsLastSibling(); // 맨 위로 UI올리기
        CreateSellItemSlot();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        endTime = (int)(Time.time + initTime);
        StartCoroutine(RefreshSellItemSlot()); 
    }

    // Update is called once per frame
    void Update()
    {
        float remainTime = Mathf.Max(0, endTime - Time.time); // 0초보다 작아지는건 안됨

        int minuteTime = (int)remainTime / 60;
        int secondTime = (int)remainTime % 60;

        timeText.text = minuteTime.ToString() + " : " + secondTime.ToString();
    }

    private void CreateSellItemSlot()
    {
        for(int i =0; i < slotNum; i++)
        {
            StoreUISlot storeUISlot = Instantiate(storeUISlotPrefab, storeUISlotTransform);

            int randomNum = Random.Range(0, sellItemList.Count); // 아이템 목록중에 아무 인덱스 하나 뽑고

            storeUISlot.Init(sellItemList[randomNum]); // 그거를 상점에 올리자
            storeUISlots.Add(storeUISlot);

            endTime = (int)(Time.time + initTime);
        }
    }

    //일정 기간 지나면 아이템 목록 초기화하는 함수 -> 떠돌이 상인인데 같은 아이템만 파는건 좀 그런거같네

    private IEnumerator RefreshSellItemSlot()
    {
        while (true)
        {
            yield return new WaitForSeconds(initTime);

            for (int i = 0; i < slotNum; i++)
            {
                int randomNum = Random.Range(0, sellItemList.Count);

                storeUISlots[i].Init(sellItemList[randomNum]);
            }
            endTime = (int)(Time.time + initTime);
        }
    }
}
