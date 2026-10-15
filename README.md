# The Myth of Sirini — C# Source Portfolio

Unity 6와 C#으로 개발 중인 1인 개발 RPG 프로젝트의 대표 게임플레이 코드입니다. 플레이어 입력과 상태 전환, 몬스터 AI, 퀘스트 진행, 아이템 관리, 풀링 구현을 중심으로 구성했습니다.

## 살펴볼 코드

|기능|구현 내용|대표 파일|
|-|-|-|
|입력 버퍼와 상태 머신|선입력 수집, 유효 시간이 있는 요청 보관, 상태별 전환 허용 및 우선순위 처리|[PlayerInputBuffer](Assets/Script/PlayerScript/PlayerInput/PlayerInputBuffer/PlayerInputBuffer.cs), [PlayerStateMachine](Assets/Script/PlayerScript/PlayerStateMachine/PlayerStateMachine.cs)|
|콤보와 무기 전환|애니메이션 이벤트에 따른 콤보 예약과 무기 전환|[NormalAttackState](Assets/Script/PlayerScript/PlayerStateMachine/PlayerState/NormalAttackState.cs), [PlayerWeaponController](Assets/Script/PlayerScript/PlayerController/PlayerWeaponController.cs)|
|몬스터 AI|직접 구성한 행동 트리와 슬라임 행동, 드래곤의 전투 상태 및 리깅 연동|[Behavior\_Tree](Assets/Script/AiScript/Behavior_Tree.cs), [SlimeAIScript](Assets/Script/MonsterScript/Slime/Refacoring_Slime/SlimeAIScript.cs), [DragonGroundCombatState](Assets/Script/MonsterScript/Boss/Dragon/StateMachine/State/DragonGroundCombatState.cs)|
|퀘스트 진행|퀘스트 정의와 플레이어 진행도 분리, 타깃별 요구사항 인덱스, 진행 이벤트|[PlayerQuestData](Assets/Script/PlayerScript/PlayerData/PlayerQuestData.cs), [QuestManager](Assets/Script/Quest/QuestManager/QuestManager.cs)|
|인벤토리와 장비|아이템 정의와 인스턴스 분리, 장비 슬롯과 이벤트 연동|[Re\_Inventory](Assets/Script/PlayerScript/Inventory/Re_Inventory.cs), [EquipmentItemInstance](Assets/Script/Item/ItemData/new_itemForder/ItemDataBase/Instance/EquipmentItemInstance.cs)|
|이펙트 재사용|종류별 Queue 보관, 재생과 반환, 파티클 종료 확인|[EffectPoolManager](Assets/Script/EffectPool/EffectPoolManager.cs), [PooledEffect](Assets/Script/EffectPool/PooledEffect.cs)|
|아이템 드롭|몬스터 사망 이벤트와 드롭 테이블을 연동해 확률·수량을 결정하고 아이템 풀에서 배치|[DropManager](Assets/Script/Item/DropTable/Manager/DropManager.cs), [DropItemPoolManager](Assets/Script/Item/DropItem/DropItemPoolManager.cs)|
|NPC 가중치 그래프 기반 이동|노드 기반 순찰 경로와 NavMesh 이동을 구성하고, 대화 상태에 따라 이동을 정지·재개|[WanderingTraderNav](Assets/Script/NpcScript/WanderingTraderNav.cs), [WanderingTraderAIController](Assets/Script/Npc/NpcAIController/WanderingTraderAIController.cs), [NavigationNode](Assets/Script/NaviScript/NavigationNode.cs)|

## 공개 범위

플레이어 상태 머신, 몬스터 AI, 퀘스트, 인벤토리 등 주요 게임플레이 구현과 관련 C# 코드를 공개합니다.



모델, 텍스처, 애니메이션 등 외부 에셋은 포함하지 않은 코드 열람용 저장소이며, 이 저장소만으로 게임을 실행할 수는 없습니다.



## 열람 안내

주요 게임플레이 구현을 살펴볼 수 있는 C# 소스 코드 저장소입니다.

씬과 에셋 등 실행에 필요한 파일은 포함하지 않아, 이 저장소만으로 게임을 실행할 수는 없습니다.



현재 개발 중인 프로젝트로, 드래곤 AI 등 일부 기능은 구현을 진행하고 있습니다.

