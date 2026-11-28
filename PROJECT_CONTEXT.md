# MobileClickerProject 현재 상태

> 마지막 확인: 2026-11-28. 이 문서는 세션 시작을 돕는 요약이며, 실제 작업 전 브랜치·Git 상태와 관련 코드를 다시 확인한다.

## 프로젝트 개요

- Unity 6000.0.61f1, C#, Android 대상 2D 방치형 클리커 RPG.
- 터치 공격과 스킬로 몬스터를 처치하고 골드로 성장하며 무한 스테이지를 진행한다.
- 주요 설계는 매니저 중심 시스템, ScriptableObject 설정 데이터, JSON 로컬 저장, 오브젝트 풀링이다.
- 개요와 구현 의도는 [README.md](README.md), 개발 규칙은 [AGENTS.md](AGENTS.md)를 참고한다.

## 주요 시스템 위치

- 진행·저장: `Assets/Scripts/Manager/GameManager.cs`, `SaveManager.cs`, `StageManager.cs`, `SaveData.cs`
- 전투·성장: `AttackManager.cs`, `Monster.cs`, `Player.cs`, `UpgradeManager.cs`, `AbilityManager.cs`
- 스킬·버프: `SkillManager.cs`, `SkillManagement.cs`, `SkillRuntime.cs`, `BuffManager.cs`, `Assets/Scripts/Skill/`
- 화면·UI: `GameView.cs`, `HUDManager.cs`, `ViewManager.cs`, `Assets/Scripts/UI/`
- 데이터·풀: `Assets/Scripts/Data/`, `PoolManager.cs`, `ObjectPool.cs`

## 진행 상황 스냅샷

- 기본 전투, 스테이지, 성장, 스킬, 저장 구조가 프로젝트에 구현되어 있다. 작업할 때 현재 코드를 기준으로 동작을 확인한다.
- 모바일 해상도 대응, 몬스터 애니메이션, 공격 및 스킬 연출, 폰트 등의 개선 작업이 진행되어 왔다. 구체적인 완료 범위는 관련 코드·에셋과 Git 이력에서 확인한다.
- 현재 작업 트리에는 오프라인 보상 팝업 관련 `GameManager.cs`, `GameScene.unity`, 신규 `OfflineRewardPopup.cs` 변경과 폰트 SDF 변경이 감지되었다. 미커밋 상태이며, 세션 시작 시 실제 상태와 diff를 다시 확인한다.
- 오프라인 보상 구현은 저장 시각을 기준으로 경과 시간 및 골드를 계산해 재접속 때 팝업으로 보여주는 흐름이다. 씬 연결과 시간·보상 표시를 에디터 및 기기에서 검증했는지는 별도로 확인한다.

## 빌드 저장 및 스킬 초기화 (2026-11-29)

- 저장 파일이 없는 첫 실행에도 `GameManager.LoadData()`가 기본 상태로 매니저들을 초기화하고 스킬 목록 UI를 구성하도록 수정했다. 스테이지 기본 진입값은 1이다.
- `SaveManager`는 게임 초기화 완료 후 앱이 백그라운드로 전환될 때와 종료될 때 진행 상태를 저장한다. 스킬 해금 직후에도 저장한다.
- 확인 필요: Unity 에디터에서 컴파일 및 새 저장 데이터 흐름 확인, Android 빌드에서 첫 실행 스킬 메뉴 표시, 스킬 해금 후 백그라운드 복귀 및 재실행 시 저장 유지 확인. 이번 변경에서는 에디터/Android 실행 검증을 하지 않았다.

## 모바일 프레임 목표 (2026-11-29)

- `GameManager.Start()`에서 모바일 플랫폼일 때 `Application.targetFrameRate`를 60으로 설정한다. PC 에디터/빌드에는 이 제한을 적용하지 않는다.
- 확인 필요: Android 실기기에서 실제 FPS가 60에 가까운지 Profiler 또는 FPS 표시로 확인한다. 코드 변경 후 Unity 컴파일과 기기 실행 검증은 아직 하지 않았다.

## 분노의 광클 UI (2026-11-28)

- 세션 작업은 [개발 변경 기록](DEVELOPMENT_LOG.md)에 정리했다. `dev_Codex`의 미커밋 변경에는 기존 오프라인 보상·폰트 작업과 분노 로직·씬 UI·에셋·문서가 포함된다. 커밋·푸시는 하지 않았다.
- `RageClickManager` 기본값은 빠른 터치 기준 0.25초, 증가 0.08/회, 느린 터치 감소 0.05/회, 지속 감소 0.1/초다. 빠른 터치 중에도 지속 감소한다. 사용자가 `DamageCalcurator`에 배율을 연결했으며 현재 터치의 게이지 갱신 전에 피해를 계산한다. `MAX` 코드 삭제 후 빈 `RageState_Text`와 이전 직렬화 데이터는 씬에 남아 있다.

- `Assets/Scripts/View/RageClickView.cs`가 분노 게이지(0~1), 현재 피해 배율(1~3), 불꽃 색상 강조를 표시한다. 최대치 직후 자연 감소로 글자가 반복 점멸하는 것을 막기 위해 `MAX` 텍스트 갱신과 코드 참조를 제거했다. `GameScene.unity`의 `ViewManager/RageClickView` 컴포넌트가 `Top_Panel/RageGauge_Panel` 하위 UI를 직렬화 참조로 연결한다. UI는 씬에 미리 배치되어 에디터에서 위치와 크기를 편집할 수 있다. `GameView`의 실행 시 생성·제거 코드는 제거했다.
- 전투 영역 왼쪽의 세로 중앙에 아래에서 위로 차오르는 표시 전용 Slider를 배치하며, `MobileHudScaler`의 안전 영역과 화면 비율 처리를 상속한다. 패널은 88×540, 막대는 36×352이며 왼쪽 가장자리에서 중심까지 68만큼 띄웠다. Slider의 상호작용·내비게이션과 이미지·텍스트의 Raycast Target을 꺼서 공격 입력을 가리지 않는다. 표시값은 `LateUpdate()`에서 읽으며 위치·크기는 코드에서 덮어쓰지 않는다. 분노·피해 계산 로직은 변경하지 않았다.
- 불꽃 이미지는 내장 imagegen으로 생성했고 `Assets/Resources/UI/RageClick/rage_flame.png`에 저장했다. 생성 프롬프트는 같은 폴더의 `rage_flame.prompt.txt`에 보관한다. 최종 UI는 씬에서 스프라이트를 직접 참조하며 실행 시 Resources 로딩은 제거했다.
- 확인: 사용자에게 씬 저장 완료를 확인한 뒤 수정했다. 새 씬 오브젝트 ID 중복 및 새 참조 누락 없음, Unity 프로젝트 참조를 사용하는 C# 컴파일 오류 없음. 다음 확인: Unity 에디터에서 씬 재로드 후 배치·글꼴·레이어 순서와 Play 동작 확인, Device Simulator의 16:9·19.5:9·태블릿 및 안전 영역, 실기기의 크기·가독성·터치 확인. 에디터 화면·Simulator·실기기 시각 검증은 아직 수행하지 않았다.

## 다음 세션 시작 순서

1. `AGENTS.md`와 이 문서를 읽는다.
2. 현재 브랜치와 변경 사항을 확인하고, 기존 사용자 작업을 보존한다.
3. 이어서 할 기능과 직접 관련된 코드·씬만 살펴 문서의 상태를 확인한다.
4. 기능을 마친 뒤 진행 상태, 다음 확인 항목, 검증 여부를 이 문서에 짧게 갱신한다. 세부 기록은 사용자가 요청한 경우에만 `DEVELOPMENT_LOG.md`에 남긴다.
