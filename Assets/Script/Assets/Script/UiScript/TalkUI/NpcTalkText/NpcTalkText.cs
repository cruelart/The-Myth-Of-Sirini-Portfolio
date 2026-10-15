using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcTalkText : MonoBehaviour
{
    [TextArea]
    private List<NpcTextData> npcTextList;

    [SerializeField]
    private TextMeshProUGUI npcText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowNpcTalkText(List<NpcTextData> _npcTextList, int _currentIndex) // 얕은 복사 -> 주소참조(복사할 필요없으니까 참조만)
    {
        npcTextList = _npcTextList;
        npcText.text = npcTextList[_currentIndex].npcText;

    }

}
