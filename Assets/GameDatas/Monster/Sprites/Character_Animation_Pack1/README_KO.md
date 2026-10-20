# 캐릭터 애니메이션 PNG 패키지

제공된 스크린샷을 참고해 새로 제작한 2D 캐릭터 애니메이션입니다.
스크린샷에는 없는 동작과 가려진 신체를 보완했으며, 원본 게임 리소스 추출본은 아닙니다.

## 포함 파일

- 몬스터 8종: idle(대기) 6프레임 + hit(피격) 6프레임
- 플레이어 1종: idle(대기) 6프레임 + attack(공격) 6프레임
- 개별 투명 PNG 108개, 모션별 투명 PNG 시트 18개 — 총 PNG 126개
- 모션별 GIF 18개 및 전체 동작 미리보기 GIF 1개
- 캐릭터 목록 JPG 1개, 애니메이션 정보 JSON, 제작 프롬프트

| 폴더 | 캐릭터 | 모션 |
| --- | --- | --- |
| `fire_golem` | 화염 골렘 | idle + hit |
| `ice_dragon` | 얼음 드래곤 | idle + hit |
| `lich_king` | 리치 왕 | idle + hit |
| `anubis_guardian` | 아누비스 수호자 | idle + hit |
| `forest_treant` | 숲의 수호자 | idle + hit |
| `celestial_knight` | 천상의 기사 | idle + hit |
| `abyss_kraken` | 심연의 크라켄 | idle + hit |
| `inferno_dragon` | 멸망의 드래곤 | idle + hit |
| `player_swordsman` | 흰머리 검사 | idle + attack |

## 파일 구조

각 캐릭터 폴더의 `idle/`, `hit/` 또는 `attack/`에 개별 프레임이 있습니다.
예: `player_swordsman/attack/player_swordsman_attack_01.png`
캐릭터 폴더 바로 아래의 `*_sheet.png`는 해당 6프레임을 왼쪽부터 배열한 시트입니다.
`previews/`의 GIF는 확인용이며 배경색이 포함되어 있습니다. 실제 게임에는 투명 PNG를 사용하세요.

## 이미지 및 재생 설정

- 개별 프레임: 512 × 512 px, RGBA, 투명 배경
- 스프라이트 시트: 3072 × 512 px, 6열 × 1행, 간격 0
- 파일 순서: 01 → 02 → 03 → 04 → 05 → 06
- 권장 idle: 6 fps, 반복
- 권장 hit / attack: 12 fps, 1회 재생 후 idle로 복귀
- 기준점: 이미지 왼쪽 위 기준 (256, 448), 아래에서 위로 정규화한 Pivot (0.5, 0.125)
- 같은 캐릭터의 프레임은 같은 배율로 정리했습니다. 캐릭터 간 화면상 크기는 게임에서 조절하세요.
- hit / attack의 처음과 마지막은 idle 01과 동일해 전환 시 자세가 맞습니다.
- GIF는 확인을 위해 hit / attack도 반복하고 마지막 자세에 잠시 머뭅니다. 게임의 반복 설정은 JSON의 loop 값을 따르세요.

## Unity에서 사용

1. 사용할 캐릭터 폴더를 프로젝트의 Assets 안에 복사합니다.
2. 개별 PNG를 사용할 경우 Texture Type을 Sprite (2D and UI), Sprite Mode를 Single로 설정합니다.
3. 시트를 사용할 경우 Sprite Mode를 Multiple로 지정하고 Sprite Editor에서 Grid By Cell Size, 512 × 512로 자릅니다. 시트의 Max Size는 4096 이상으로 설정해 축소를 방지합니다.
4. 모든 프레임의 Pivot을 Custom (0.5, 0.125)로 맞춥니다. Pixels Per Unit도 동일하게 사용합니다.
5. 번호 순서로 프레임을 배치하고 idle은 6 fps + Loop Time, hit / attack은 12 fps + 반복 해제로 설정합니다.
6. hit / attack 완료 후 idle로 돌아가도록 전환을 연결합니다. 실제 타격 판정과 이펙트는 게임 로직에서 연결하세요.

개별 프레임과 시트는 같은 이미지의 두 제공 방식입니다. 프로젝트에는 편한 쪽을 선택해 사용하면 됩니다.
PNG 크기·투명도·파일 수·압축 무결성을 검수했습니다. Unity 프로젝트에서의 실제 실행 검증은 포함하지 않습니다.

Unity 설정 참고: [공식 Sprite Editor 설명](https://docs.unity.com/en-us/engine/6000.0/manual/unity2d/sprite/sprite-editor-window-reference/reference)
