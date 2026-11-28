# 개발 변경 기록

## 2026-11-29 빌드 초기화·저장 및 모바일 프레임 설정

- **목적** — 새 빌드에서 첫 실행 스킬 메뉴가 비고 모바일에서 PC와 프레임 체감이 다른 문제를 점검하고, 초기화·저장 흐름과 모바일 목표 프레임을 설정한다.
- **초기화와 스킬 UI** ([GameManager.cs](Assets/Scripts/Manager/GameManager.cs)) — 저장 파일이 없어도 기본 저장 데이터로 진행 상태와 스킬 데이터를 초기화하고 스킬 메뉴 UI를 구성하도록 수정했다. 기본 스테이지는 1로 보정한다.
- **저장** ([SaveManager.cs](Assets/Scripts/Manager/SaveManager.cs), [SkillBtn.cs](Assets/Scripts/Object/SkillBtn.cs)) — 게임 초기화 완료 후 앱이 백그라운드로 가거나 종료될 때 저장하며, 스킬 해금 직후에도 저장한다. Android에서 종료 콜백만으로 저장이 보장되지 않는 점을 보완했다.
- **모바일 프레임 목표** ([GameManager.cs](Assets/Scripts/Manager/GameManager.cs)) — 모바일 플랫폼에서 `Application.targetFrameRate`를 60으로 설정하고 PC에는 적용하지 않는다. Android는 Unity 기본 목표가 30 FPS이며 모바일에서는 `vSyncCount`가 무시되는 점을 근거로 설정했다.
- **검증 결과** — `git diff --check` 통과. Unity 컴파일, Android 빌드, 실기기 FPS 및 저장 복귀 동작은 확인하지 않았다.
- **다음 확인** — Android에서 첫 실행 스킬 목록, 해금 후 백그라운드 복귀·재실행 시 저장 유지, 실제 FPS와 발열 상태를 확인한다.

## 2026-11-28 분노의 광클 시스템과 게이지 UI

- **목적** — 빠르게 터치할수록 피해량이 증가하는 동기를 제공하고 현재 분노 상태와 피해 배율을 시각화한다.
- **분노 로직** — [RageClickManager.cs](Assets/Scripts/Manager/RageClickManager.cs)에 게이지 0~1, 피해 배율 1~3배를 구현했다. 첫 터치는 간격 판정에서 제외한다. 기본값은 빠른 터치 기준 0.25초, 증가 0.08/회, 느린 터치 감소 0.05/회, 지속 감소 0.1/초이며 Inspector에서 조정한다. 지속 감소는 빠른 터치 중에도 적용된다.
- **피해 연결** — 사용자가 수정한 [ClickHandler.cs](Assets/Scripts/Core/ClickHandler.cs)와 [DamageCalcurator.cs](Assets/Scripts/DamageCalcurator.cs)를 확인했다. 피해 계산에서 분노 배율을 곱하며 현재 터치의 게이지 갱신 전에 피해를 계산한다. 사용자는 피해가 잘 반영되는 것으로 확인했다.
- **씬과 View** — [RageClickView.cs](Assets/Scripts/View/RageClickView.cs)와 [GameScene.unity](Assets/Scenes/GameScene.unity)에 UI를 구성했다. 최종 구조는 `ViewManager/RageClickView`가 `Top_Panel/RageGauge_Panel` 하위 UI를 Inspector 참조로 연결하는 방식이다. 초기 실행 시 생성 방식은 제거했고 [GameView.cs](Assets/Scripts/View/GameView.cs)는 기존 역할을 유지한다. 위치·크기는 씬에서 직접 편집한다.
- **최종 배치** — 탑패널 왼쪽의 세로 중앙에 배치했다. 왼쪽 가장자리에서 중심까지 68, 패널 88×540, 막대 36×352이며 아래에서 위로 차오른다. 기존 `MobileHudScaler`의 안전 영역을 따른다. Slider 상호작용·내비게이션과 이미지·텍스트 Raycast Target을 꺼서 공격 입력을 가리지 않는다.
- **표시 변경 근거** — 배율과 불꽃 색상 강조를 표시한다. 최대치 직후 감소하며 `MAX` 글자가 반복 점멸해 산만하다는 피드백에 따라 해당 표시 로직과 코드 필드를 제거했다. 씬에는 빈 `RageState_Text`와 이전 직렬화 데이터가 남아 있으나 갱신하지 않는다.
- **에셋** — 내장 imagegen으로 투명 배경 픽셀풍 [불꽃 이미지](Assets/Resources/UI/RageClick/rage_flame.png)를 생성하고 [생성 프롬프트](Assets/Resources/UI/RageClick/rage_flame.prompt.txt)를 보관했다. 최종 UI는 씬에서 스프라이트를 직접 참조한다.
- **개발 규칙** — [AGENTS.md](AGENTS.md)에 추가·수정하는 필드 선언 끝에 `//`로 역할을 설명하는 규칙을 추가하고 분노 매니저에도 적용했다.
- **검증 결과** — Unity 프로젝트 참조를 사용하는 외부 C# 컴파일 오류 없음. 신규 씬 오브젝트 ID 중복·참조 누락을 검사했고 왼쪽 재배치 시 관련 RectTransform만 변경했음을 확인했다. Unity 에디터 Play 검증은 수행하지 않았다.
- **미검증 범위와 다음 확인** — 에디터 씬 재로드 후 참조·글꼴·배치·Play 동작, Device Simulator의 16:9·19.5:9·태블릿·안전 영역, Android 실기기 가독성·터치·밸런스를 확인한다. 감소 관련 주석과 실제 지속 감소 동작의 차이, Inspector 시작 게이지의 초기 범위 제한도 후속 검토 사항이다.
- **Git** — `dev_Codex`에서 작업했고 커밋·푸시는 하지 않았다. 세션 이전 오프라인 보상·폰트 등의 변경은 보존했다.

## 2026-11-27 전투 화면 개선

- **피해 숫자** (`DamageText.cs`, `GameView.cs`, `GameScene.unity`) — 몬스터 주변에 자연스럽게 퍼지며 체력바 아래에서 사라진다. 일반 피해는 따뜻한 흰색, 스킬 피해는 연한 하늘색으로 구분했다.
- **골드 숫자** (`AdditionScoreText.cs`) — 금색 `+숫자`로 표시하고 상승 거리와 표시 시간을 줄여 피해량과 쉽게 구분할 수 있게 했다.
- **스킬 슬롯** (`SkillExecutor.cs`, `GameScene.unity`) — 장착 후에도 금테를 유지한다. 쿨타임 중에는 아이콘을 어둡게 하고 남은 초와 금색 진행바를 표시한다.
- **스킬 이펙트** (`SkillEffectPlayer.cs`, `SkillEffect.cs`, `SkillManager.cs`, `Critical Hit.asset`, `Thunder Bolt.asset`) — 기존 애니메이션을 활용해 급소 강타는 몬스터 중앙에서 짧게, 썬더 볼트는 더 높은 위치에서 재생한다. 피해 숫자가 이펙트보다 앞에 표시된다.

**확인** — Unity Play 모드에서 피해 숫자, 쿨타임, 두 이펙트의 재생을 확인했다. 이펙트의 화면 연출과 Android 실기기 가시성은 추가 확인이 필요하다.

## 2026-11-27 Android 화면 비율 대응

- **HUD 배치** (`MobileHudScaler.cs`, `HUDManager.cs`, `GameScene.unity`) — 1080 × 1920을 기준으로 기기별 안전 영역에 맞춰 크기를 조절한다. 상단 전투 영역과 하단 메뉴 영역은 55:45 비율로 배치한다.
- **넓은 화면과 방향** (`MobileHudScaler.cs`, `ProjectSettings.asset`) — 태블릿과 폴더블에서 플레이 영역을 가운데에 배치한다. 기본 방향은 기존 전투 화면에 맞춰 세로로 설정했다.
- **미리보기** — Unity의 **Window → General → Device Simulator**에서 Android 기기를 선택하고 **Fit to Screen**과 **Safe Area**를 켠 뒤 Play로 확인할 수 있다.

**확인** — Unity 컴파일과 5가지 화면 조건의 배치 계산을 확인했다. 시뮬레이터의 기기별 버튼·텍스트 겹침과 Android 실기기 동작은 추가 확인이 필요하다.
