using UnityEngine;

public class ConsumerItemInstance
{
    //베이스 아이템 고유데이터
    public ConsumerItemDB setting;

    //인스턴스 아이템 데이터
    public long instanceID { get; private set; }
    public int count { get; private set; }

    public ConsumerItemInstance(ConsumerItemDB _setting)
    {
        this.setting = _setting;
        //this.data = _data;
        instanceID = ItemInstanceID.GetInstanceID();
        count = 0;
    }

    //아이템 사용했을때 효과
    public void UseConsumerItem()
    {

    }
    public bool GetIsCombine()
    {
        if(this.setting.maxNum != 1) // 최대 1개가 아니면 합치는거 가능
        {
            return true;
        }
        return false; // 최대 1개면 합치기 불가능
    }

    public void AddCount(int _num)
    {
        count += _num;
    }

    public void RemoveCount(int _num)
    {
        if (count != 0)
        {
            count -= _num;
        }
    }

}
