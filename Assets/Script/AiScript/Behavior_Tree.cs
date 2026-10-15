using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    //노드의 상태
    public enum tasks { Success, Failure, Running } // 성공, 실패, 실행중

    //오브젝트하나에서 노드가 참고할 수 있는 데이터(블랙보드)
    public class Blackboard // 언리얼에서 사용하는 블랙보드처럼 사용할 예정
    {
        private Dictionary<string, object> data = new Dictionary<string, object>(); // 데이터(해시기반)

        //데이터 저장
        public void SetData(string key, object value) => data[key] = value; // 람다 사용

        //데이터 Get(자동 형변환)
        public T GetData<T>(string key)
        {
            if (data.TryGetValue(key, out object value)) // 일치하는 키값이 존재하면
                return (T)value; // 형변환후 반환
            return default; // 없으면 디폴트
        }

    }
    abstract public class Node
    {
        abstract public tasks Evaluate(); // 노드의 상태를 나타내는 것은 무조건 구현해야됨(추상 클래스로 설정하자)
    }

    //조건 따지는 데코레이터 클래스
    public abstract class DecoratorNode : Node
    {
        protected Node child; // 실행시킬 노드
        public DecoratorNode(Node child) => this.child = child;
    }

    //특정 조건이 참이면 실행하도록 하는 데코레이터
    public class ConditionNode : DecoratorNode
    {
        private Func<bool> condition; // 델리게이트 선언
        public ConditionNode(Func<bool> _condition, Node child) // 조건함수, 실행시킬 노드
            : base(child) => condition = _condition;
        public override tasks Evaluate()
        {
            if (condition == null || !condition())
                return tasks.Failure;

            return child.Evaluate();
        }
    }

    //Sequence, Selector와 같이 하위 자식노드를 많이 가질 수 있는 노드
    public abstract class CompositeNode : Node
    {
        protected List<Node> childlist = new List<Node>();
        public void ListAdd(Node node) => childlist.Add(node);
    }

    public class SelectorNode : CompositeNode
    {
        public override tasks Evaluate()
        {
            foreach (Node node in childlist) // 자식노드를 탐색
            {
                switch (node.Evaluate()) // 자식노드가 반환한 값이
                {
                    case tasks.Success: // 성공일 경우
                        return tasks.Success; // 자식중 성공이 나오면 바로 return
                    case tasks.Running:
                        return tasks.Running; // 자식중 실행중이면 바로 return
                }
            }
            return tasks.Failure; // 성공도 아니고 실행도 아닌 상태라면 실패 return
        }
    }

    public class SequenceNode : CompositeNode
    {
        public override tasks Evaluate()
        {
            foreach (Node node in childlist)
            {
                switch (node.Evaluate())
                {
                    case tasks.Success:
                        continue;
                    case tasks.Failure: // 자식노드중 1개라도 실패시 Failure반환 
                        return tasks.Failure;
                    case tasks.Running:
                        return tasks.Running;
                }
            }
            return tasks.Success; // 모든 자식노드를 순회하여 자식노드가 존재하지 않는다면 Success반환 
        }
    }

    public class ActionNode : Node
    {
        private Func<tasks> action; // states를 반환하는 delegate 선언
        public ActionNode(Func<tasks> _action) => action = _action;

        public override tasks Evaluate() => action?.Invoke() ?? tasks.Failure;
        // ?.invoke() : null이면 그냥 null이라고 해라
        // ?? 왼쪽이 null이면 task.Failure다로 표현

    }
}
