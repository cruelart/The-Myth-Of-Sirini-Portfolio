using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.Rendering;
#endif

[System.Serializable]
public class Edge
{
    public NavigationNode node;
    public int weight; // 가중치 간선을 위한 값
}

public class NavigationNode : MonoBehaviour
{
    [SerializeField]
    private List<Edge> edges = new();
    public IReadOnlyList<Edge> Edges => edges; // 읽기 전용


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel);
        labelStyle.fontSize = 20;
        labelStyle.normal.textColor = Color.yellow;
        labelStyle.alignment = TextAnchor.MiddleCenter;

        // 지형이나 오브젝트에 가려지지 않도록 설정
        CompareFunction previousZTest = Handles.zTest;
        Handles.zTest = CompareFunction.Always;

        // 현재 노드 표시
        Handles.color = Color.yellow;

        Handles.SphereHandleCap(
            0,
            transform.position,
            Quaternion.identity,
            1.7f,
            EventType.Repaint
        );

        foreach (Edge edge in edges)
        {
            if (edge.node == null)
                continue;

            Vector3 start = transform.position;
            Vector3 end = edge.node.transform.position;
            Vector3 direction = end - start;

            if (direction.sqrMagnitude <= Mathf.Epsilon)
                continue;

            Vector3 normalizedDirection = direction.normalized;

            // 간선 표시
            Handles.color = Color.magenta;

            Handles.DrawAAPolyLine(
                7f,
                new Vector3[] { start, end }
            );

            // 진행 방향 기준으로 간선 옆 방향 계산
            Vector3 side = Vector3.Cross(
                Vector3.up,
                normalizedDirection
            ).normalized;

            // 출발 노드에서 목적지 방향으로 30% 지점
            // 양방향 Edge는 각각 반대쪽에 표시됨
            Vector3 labelPosition =
                start
                + direction * 0.3f
                + side * 0.4f
                + Vector3.up * 0.5f;

            // 월드 X·Z축 기준 방향 문자
            string arrow = GetDirectionArrow(direction);

            Handles.Label(
                labelPosition,
                $"{arrow} 가중치 {edge.weight}",
                labelStyle
            );
        }

        // 기존 Handles 설정 복구
        Handles.zTest = previousZTest;
    }

    private string GetDirectionArrow(Vector3 direction)
    {
        // 지면에서 사용하는 X·Z 방향을 각도로 변환
        float angle =
            Mathf.Atan2(direction.z, direction.x)
            * Mathf.Rad2Deg;

        // -180~180 범위를 0~360으로 변환
        if (angle < 0f)
            angle += 360f;

        // 45도 간격의 8방향으로 변환
        int directionIndex =
            Mathf.RoundToInt(angle / 45f) % 8;

        switch (directionIndex)
        {
            case 0:
                return "→"; // +X

            case 1:
                return "↗"; // +X, +Z

            case 2:
                return "↑"; // +Z

            case 3:
                return "↖"; // -X, +Z

            case 4:
                return "←"; // -X

            case 5:
                return "↙"; // -X, -Z

            case 6:
                return "↓"; // -Z

            case 7:
                return "↘"; // +X, -Z

            default:
                return "";
        }
    }
#endif
}
