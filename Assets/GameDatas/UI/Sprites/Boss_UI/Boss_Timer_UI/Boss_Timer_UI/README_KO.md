# 보스 모래시계 · 남은 시간 UI

첨부 이미지의 체력바 오른쪽 타이머를 참고해 재제작한 UI 리소스입니다.
원본 게임 파일을 직접 추출한 것은 아닙니다.

## PNG 3종

| 파일 | 크기 | 용도 |
| --- | --- | --- |
| PNG/timer_frame.png | 512 × 224 | 금색 테두리 + 빈 모래시계 칸 + 빈 시간 표시 칸 |
| PNG/hourglass_icon.png | 128 × 192 | 금색 모래시계와 붉은 모래 정지 아이콘 |
| PNG/timer_warning_glow.png | 768 × 448 | 뒤에 겹치는 붉은 경고 발광 효과 |

바깥 배경은 모두 투명합니다. 프레임 내부의 어두운 면은 유지했습니다.
`4.8s` 같은 글자는 이미지에 포함하지 않았습니다. TextMeshPro에서 남은 시간에 맞춰 변경하세요.
Preview.jpg의 숫자는 조립 예시입니다.

## Unity 배치 예시

빈 RectTransform인 BossTimerRoot를 만들고 Width 768 / Height 448로 둡니다.
그 아래에 다음 순서로 자식을 생성합니다. 모두 Anchor Min/Max와 Pivot을 (0.5, 0.5)로 맞춥니다.

| 자식 / 렌더 순서 | 컴포넌트 | 크기 | Pos X | Pos Y |
| --- | --- | --- | ---: | ---: |
| WarningGlow | Image | 722 × 394 | 0 | -10 |
| TimerFrame | Image | 512 × 224 | 0 | 0 |
| Hourglass | Image | 96 × 160 | -178 | 0 |
| TimeText | TextMeshProUGUI | 300 × 116 | 76 | -8 |

Image에는 각 PNG를 연결하고 Type을 Simple로 설정합니다.
Hourglass는 Preserve Aspect를 켭니다. WarningGlow는 프레임 뒤에서 그려져야 합니다.
TimeText는 가로·세로 가운데 정렬, Bold, 글자 크기 약 66, 흰색을 시작 설정으로 사용하세요.
글꼴에 따라 크기는 조금 조정하면 됩니다. 글꼴 파일은 포함하지 않았습니다.

실제 화면에서는 전체를 같은 비율로 축소하세요. 예를 들어 0.4배로 배치하면 프레임이 약 205 × 90 크기로 보입니다.
WarningGlow를 끄면 기본 표시, 켜면 붉은 경고 표시입니다. 경고가 필요한 시점은 게임 로직에서 결정하세요.
전체 루트는 발광 여백을 포함하므로, 체력바에 붙일 때 실제 금색 프레임의 왼쪽 끝을 기준으로 배치하세요.

## 가져오기

- Texture Type: Sprite (2D and UI)
- Sprite Mode: Single
- Mesh Type: Full Rect
- Alpha Is Transparency: On
- 프레임/아이콘 Filter Mode: Point를 시작값으로 사용
- 발광 효과 Filter Mode: Bilinear
- Compression: None을 시작값으로 사용
- 장식 이미지의 Raycast Target: Off

## 시간 표시

기존 타이머의 남은 시간 값을 표시하는 예시입니다.

```csharp
timeText.text = $"{Mathf.Max(0f, remainingTime):0.0}s";
```

숫자는 하나의 TextMeshPro 오브젝트로 표현하면 됩니다. 숫자별 PNG는 필요하지 않습니다.
시간 감소, 보스 클리어 판정, 제한 시간 종료 처리는 기존 게임 로직에 연결하세요.

이 패키지는 정지 UI 이미지입니다. 모래시계 애니메이션, 실행용 프리팹, 카운트다운 스크립트는 포함하지 않습니다.
PNG 투명도와 조립 배치를 확인했으며 Unity 프로젝트 내 실행 검증은 하지 않았습니다.

제작 방식: 내장 ImageGen으로 이미지 제작 후 PNG 크기와 여백 정리.
제작 프롬프트는 generation_prompts.txt, 기계 판독용 배치값은 layout.json에 있습니다.
