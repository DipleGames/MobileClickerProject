# 개발 변경 기록

## 2026-11-27 Android 화면 비율 및 안전 영역 대응

**목적**  
Android 휴대폰, 태블릿, 폴더블에서 세로형 전투 화면과 HUD를 유지하고, Unity 에디터에서 기기별 화면을 확인한다.

| 변경 파일 | 수정한 부분 | 수정 근거 |
|---|---|---|
| [MobileHudScaler.cs](Assets/Scripts/UI/MobileHudScaler.cs) | 안전 영역과 1080 × 1920 기준 해상도로 Canvas 배율을 계산한다. 상·하단 패널에 안전 영역 기준 55%와 45%를 할당하고, 넓은 화면에서는 플레이 영역을 가운데에 배치한다. | 기존 HUD는 화면 픽셀 높이를 Canvas 좌표계에 적용했다. 화면 크기와 배율이 바뀌어도 두 패널이 안전 영역 안에서 같은 비율을 유지하도록 계산 기준을 통일했다. |
| [GameScene.unity](Assets/Scenes/GameScene.unity) | HUDCanvas에 새 스케일러를 연결하고 상·하단 패널, 콘텐츠, 스킬, 메뉴 컨테이너의 앵커를 씬에 저장했다. | 씬이 열릴 때부터 기본 배치가 보이고 각 컨테이너가 부모 패널 크기에 맞춰 늘어나도록 했다. |
| [HUDManager.cs](Assets/Scripts/Manager/HUDManager.cs) | 초기화 시 패널 높이와 앵커를 설정하던 코드를 제거했다. | 화면 배율과 패널 배치는 스케일러 및 씬 설정에서 관리한다. |
| [ProjectSettings.asset](ProjectSettings/ProjectSettings.asset) | 기본 화면 방향을 세로로 설정했다. | 기존 전투·메뉴 화면의 세로 구성을 제품의 기본 방향으로 삼았다. |

**검증 결과**  
Unity 에디터에서 스크립트가 컴파일됐다. 16:9, 노치가 있는 20:9, 태블릿, 가로로 펼친 폴더블, 작은 16:9 화면에 대한 배치 계산을 확인했다. Device Simulator에서 기기별 버튼·텍스트 겹침과 Android 실기기 동작은 아직 확인하지 않았다.

**미리보기 방법**  
Unity에서 **Window → General → Device Simulator**를 열고 Android 기기를 선택한다. **Fit to Screen**과 **Safe Area**를 켠 뒤 **Play**로 확인한다. [Unity Device Simulator 문서](https://docs.unity3d.com/kr/6000.0/Manual/device-simulator-view.html)
