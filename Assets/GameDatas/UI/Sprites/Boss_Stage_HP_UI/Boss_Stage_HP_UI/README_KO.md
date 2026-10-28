# 보스 스테이지 · 체력바 UI 리소스

참고 이미지의 3번 ‘보스전 강조형’ 상단을 참고해 제작했습니다.
해골·뿔 장식과 적갈색 금속 테두리를 재현한 별도 PNG 3종입니다.

| 파일 | 크기 | 용도 |
| --- | --- | --- |
| PNG/boss_stage_plate.png | 768 × 248 | 스테이지 명판. 해골·뿔 장식과 어두운 내부 포함 |
| PNG/boss_hp_frame_empty.png | 768 × 120 | 체력바 테두리와 검은 빈 체력 영역 |
| PNG/boss_hp_fill_red.png | 586 × 26 | 체력에 따라 줄어드는 빨간 채움 영역 |

명판과 체력바 프레임의 외부는 투명합니다. 빨간 채움은 여백 없는 직사각형입니다.
BOSS, STAGE 10, 5.3K 같은 글자·숫자는 PNG에 포함하지 않았습니다. TextMeshPro로 표시하고 게임 데이터에 맞춰 갱신하세요.

## Unity 구성

StagePlate(Image) 아래에 BossLabel(TMP)과 StageLabel(TMP)을 둡니다.
HPFrame(Image) 아래에 HPFill(Image)을 두고, 체력 수치를 표시할 HPLabel(TMP)을 별도로 체력바 위에 배치합니다.
HPFill은 부모 HPFrame보다 나중에 그려지도록 자식으로 두세요.

PNG는 Texture Type: Sprite (2D and UI), Sprite Mode: Single로 가져옵니다.
Sprite Mesh Type은 Full Rect, Compression은 None을 시작 설정으로 사용하세요.
Filter Mode는 Point이면 도트 윤곽이 선명하고 Bilinear이면 부드럽게 보입니다.
장식 Image의 Raycast Target은 꺼서 클릭 입력을 가로채지 않게 합니다.

## 체력바 배치

HPFrame의 RectTransform 크기를 768 × 120으로 놓았을 때:

- HPFill Anchor Min / Max: (0.5, 0.5)
- HPFill Pivot: (0.5, 0.5)
- HPFill Pos X / Y: (0, 0)
- HPFill Width / Height: 586 × 26
- HPFill Image Type: Filled
- Fill Method: Horizontal
- Fill Origin: Left
- Preserve Aspect: Off

이 배치는 프레임 이미지 왼쪽 위에서 x=91, y=47에 빨간 채움을 놓는 것과 같습니다.
프레임과 채움 전체를 동일한 배율로 축소하면 장식과 채움 위치가 함께 유지됩니다.

체력 비율만 변경합니다:

```csharp
hpFill.fillAmount = maxHP > 0
    ? Mathf.Clamp01((float)(currentHP / (double)maxHP))
    : 0f;
```

빈 체력 영역은 프레임에 포함되어 있어 별도 배경 PNG가 필요 없습니다.

## 명판 글자 배치

StagePlate가 768 × 248일 때, 자식 글자의 Anchor와 Pivot을 모두 중앙으로 둡니다.

| 글자 | Pos X | Pos Y | Width | Height | 글자 크기 예시 |
| --- | ---: | ---: | ---: | ---: | ---: |
| BOSS | 0 | 14 | 360 | 24 | 20 |
| STAGE 10 | 0 | -13 | 440 | 40 | 32 |

가운데 정렬, BOSS는 #E9A477, 스테이지는 #FFE3A0을 사용하면 예시와 비슷합니다.
실제 글꼴에 따라 크기는 조금 조절하세요. UI에 쓰는 기존 글꼴을 그대로 사용할 수 있습니다.
해골·뿔이 찌그러지지 않게 명판의 가로세로 비율을 유지해 배치하세요.

layout.json에도 크기와 배치값을 넣었습니다. PNG 및 배치 시안은 검수했으며 Unity 프로젝트 실행 검증은 포함하지 않습니다.

제작: 내장 ImageGen으로 참고 디자인을 재제작하고 PNG를 자르기·크기 정리했습니다. 원본 게임 리소스의 직접 추출본은 아닙니다.
프롬프트는 generation_prompts.txt에 있습니다.

Unity 공식 참고:
- https://docs.unity3d.com/kr/2018.4/ScriptReference/UI.Image-fillAmount.html
- https://docs.unity3d.com/kr/2017.1/ScriptReference/UI.Image.Type.Filled.html
