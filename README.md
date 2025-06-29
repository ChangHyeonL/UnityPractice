# 🛠️ 미니 컨베이어 시뮬레이터

Unity로 제작된 미니 컨베이어 벨트 시뮬레이터입니다. UI를 통한 제어와 센서 감지 기능을 제공하며, 향후 TCP 통신을 통한 외부 제어도 가능합니다.

## 🎯 주요 기능

- **UI 제어**: 버튼과 슬라이더를 통한 컨베이어 시작/정지 및 속도 조절
- **센서 감지**: 제품이 센서에 도달하면 자동 정지 및 상태 표시
- **실시간 모니터링**: 현재 상태, 속도, 센서 상태를 실시간으로 표시
- **TCP 연동 준비**: 외부 시스템과의 통신을 위한 TCP 서버 기능 (확장 예정)

## 📁 프로젝트 구조

```
Assets/
├── Scripts/
│   ├── ConveyorController.cs    # 컨베이어 메인 제어
│   ├── ProductController.cs     # 제품 상태 관리
│   ├── SensorTrigger.cs         # 센서 감지 처리
│   ├── UIController.cs          # UI 요소 제어
│   └── TCPController.cs         # TCP 통신 (확장용)
├── Materials/
│   ├── Conveyor.mat            # 컨베이어 재질
│   └── Sensor.mat              # 센서 재질
└── Scenes/
    └── SampleScene.unity       # 메인 씬
```

## 🚀 사용법

### 1. 씬 구성
- **Plane**: 작업 바닥 (기본 생성)
- **Conveyor**: 긴 큐브 (x=5, y=0.2, z=1)
- **Product**: 작은 큐브 (x=0.5, y=0.5, z=0.5)
- **Sensor**: 얇은 큐브 (x=0.2, y=0.2, z=1), isTrigger = true

### 2. 스크립트 설정
각 오브젝트에 해당하는 스크립트를 추가하고 Inspector에서 설정:

#### ConveyorController
- `product`: 제품 오브젝트 할당
- `speed`: 기본 속도 (0.1~2.0)
- `uiController`: UI 컨트롤러 참조

#### ProductController
- `normalMaterial`: 기본 재질 (파란색)
- `detectedMaterial`: 감지 시 재질 (빨간색)

#### SensorTrigger
- `targetTag`: "Product" (기본값)
- `uiController`: UI 컨트롤러 참조
- `conveyorController`: 컨베이어 컨트롤러 참조

#### UIController
- 모든 UI 요소들 할당
- `conveyorController`: 컨베이어 컨트롤러 참조
- `sensorTrigger`: 센서 트리거 참조

### 3. UI 구성
- **Start/Stop Button**: 컨베이어 시작/정지
- **Reset Button**: 제품 위치 초기화
- **Speed Slider**: 속도 조절 (0.1~2.0)
- **Status Text**: 현재 상태 표시
- **Speed Text**: 현재 속도 표시
- **Sensor Text**: 센서 상태 표시

## 🔧 개발 일정

### Day 1: 프로젝트 설계 및 Scene 구성 ✅
- [x] Unity 프로젝트 생성
- [x] 기본 오브젝트 배치 (Plane, Conveyor, Product, Sensor)
- [x] UI 요소 생성 (Canvas, Button, Slider, Text)
- [x] 스크립트 파일 생성 및 기본 구조 작성

### Day 2: 컨베이어 움직임 및 속도 제어 구현
- [ ] Slider 값으로 컨베이어 속도 제어
- [ ] Start/Stop 버튼으로 동작 on/off
- [ ] Product가 컨베이어 위에서 일정 속도로 전진

### Day 3: 센서 감지 및 상태 표시 구현
- [ ] Product가 Sensor Trigger에 닿으면 상태 Text 업데이트
- [ ] 감지 후 Product 정지 및 색상 변경
- [ ] 실시간 상태 모니터링

### Day 4: 코드 정리 및 업로드
- [ ] 코드 정리 및 주석 작성
- [ ] GitHub Push
- [ ] Notion 문서 작성
- [ ] 시연 영상 촬영

## 🔮 확장 계획

### TCP 연동 (주말 이후)
- Unity ↔ TCP Server 송수신 구조 구현
- TCP 신호 수신 시 컨베이어 제어
- 지원 명령어:
  - `start`: 컨베이어 시작
  - `stop`: 컨베이어 정지
  - `reset`: 제품 위치 초기화
  - `speed:1.5`: 속도 설정

## 🛠️ 기술 스택

- **Unity 2022.3 LTS**
- **C#**
- **TextMesh Pro** (UI 텍스트)
- **Universal Render Pipeline** (렌더링)

## 📝 주의사항

1. **태그 설정**: Product 오브젝트에 "Product" 태그를 반드시 설정해야 합니다.
2. **Collider 설정**: Sensor 오브젝트의 Collider에서 "Is Trigger"를 체크해야 합니다.
3. **Rigidbody**: Product 오브젝트에 Rigidbody 컴포넌트를 추가해야 합니다.
4. **Material**: Product의 색상 변경을 위해 두 개의 Material을 준비해야 합니다.

## 🐛 문제 해결

### 제품이 움직이지 않는 경우
- ConveyorController의 `product` 필드에 제품 오브젝트가 할당되었는지 확인
- 제품에 "Product" 태그가 설정되었는지 확인

### 센서가 작동하지 않는 경우
- Sensor의 Collider에서 "Is Trigger"가 체크되었는지 확인
- SensorTrigger 스크립트의 `targetTag`가 "Product"로 설정되었는지 확인

### UI가 업데이트되지 않는 경우
- UIController의 모든 UI 요소가 올바르게 할당되었는지 확인
- 각 스크립트 간의 참조가 올바르게 설정되었는지 확인

## 📞 문의

프로젝트 관련 문의사항이 있으시면 언제든 연락주세요!