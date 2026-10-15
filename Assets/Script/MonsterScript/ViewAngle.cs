using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ViewAngle
{
    public static bool isFindPlayer(float _viewDistance, float _viewAngle,Transform _playerTrans, Transform _monsterTransform)
    {
        Vector3 playerMonsterDirection = (_playerTrans.position - _monsterTransform.position).normalized;
        float playerMonsterAngle = Vector3.Angle(playerMonsterDirection, _monsterTransform.forward);// 플레이와 몬스터가 바라보는 사이의 각도
        float playerMonsterDistance = (_playerTrans.position - _monsterTransform.position).magnitude;

        Debug.DrawRay(_monsterTransform.position + _monsterTransform.up, playerMonsterDirection, Color.red);

        //Debug.Log("플레이어와 몬스터의 거리는 " + playerMonsterDistance);

        if (playerMonsterDistance < _viewDistance) // 몬스터가 시야거리 안에 들어왔고
        {
            //Debug.Log("플레이어가 시야거리안에 들어왔다");
            if (playerMonsterAngle < _viewAngle * 0.5f) // 몬스터가 시야각 내로 들어 왔을 경우
            {
                Debug.Log("플레이어가 시야각안에 들어왔다");
                RaycastHit Rayhit;
                if (Physics.Raycast(_monsterTransform.position, playerMonsterDirection, out Rayhit, _viewDistance))
                {
                    if (Rayhit.transform.tag == "Wall") // 플레이어가 장애물에 막혀 보이지않다면
                    {
                        Debug.Log("몬스터가 플레이어를 발견못했습니다.");
                        return false; // 보이지 않는다는 false 반환
                    }
                    else // 장애물에 막혀있지 않을 경우
                    {
                        Debug.Log("몬스터가 플레이어를 발견했습니다.");
                        return true;
                    }
                }

                Debug.Log("예외발생");

                Debug.Log("예외발생");
                Debug.Log(_monsterTransform.position + Vector3.up * 2);
            }
        }
        //Debug.Log(playerMonsterDirection);
        return false;
    }
    private static Vector3 BoundaryAngle(float _angle, Transform _monsterTransform)
    {
        _angle += _monsterTransform.eulerAngles.y; //몸이 회전하는 각도조정
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    public static void View(float _viewAngle, Transform _monsterTransform) // 몬스터의 시야각도표시함수
    {
        Vector3 _leftBoundary = BoundaryAngle(-_viewAngle * 0.5f, _monsterTransform);
        Vector3 _rightBoundary = BoundaryAngle(_viewAngle * 0.5f, _monsterTransform);

        Debug.DrawRay(_monsterTransform.position + _monsterTransform.up, _leftBoundary * 10, Color.red);
        Debug.DrawRay(_monsterTransform.position + _monsterTransform.up, _rightBoundary * 10, Color.red);
        Debug.DrawRay(_monsterTransform.position + _monsterTransform.up, _monsterTransform.forward.normalized * 10, Color.red);
    }
}
