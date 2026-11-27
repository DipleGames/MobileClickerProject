# 개발 변경 기록

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
