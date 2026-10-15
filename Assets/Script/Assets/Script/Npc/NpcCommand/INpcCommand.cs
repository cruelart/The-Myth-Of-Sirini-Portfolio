using UnityEngine;

public interface INpcCommand
{
    // void Init(); -> 이건 그냥 생성자에서 처리하자
    void Execute();
}
