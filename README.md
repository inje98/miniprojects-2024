# 미니프로젝트 2024
IoT 개발자 미니프로젝트 리포지토리

## 1일차
- 조별 자리배치
- IoT 프로젝트 개요

    ![IoT프로젝트](https://raw.githubusercontent.com/inje98/miniprojects-2024/main/images/mp001.png)

    1. IoT기기 구성 - 아두이노, 라즈베리파이 등 IoT장비들과 연결
    2. 서버 구성 - IoT기기와 통신, DB구성, 데이터 수집 앱 개발
    3. 모니터링 구성 - 실시간 모니터링/제어 앱

- 조별 미니프로젝트
    - 5월 28일(40시간)
    - 구체적으로 어떤 디바이스 구성, 데이터 수집, 모니터링 계획
    - 8월말 정도에 끝나는 프로젝트 일정 계획
    - **요구사항 리스트, 기능명세, UI/UX 디자인, DB설계 문서하나**로 통합
    - 5월 28일 오후 각 조별로 파워포인트/프레젠테이션 10분 발표
    - 요구사항 리스트 문서전달
    - 기능명세 문서
    - DB설계 ERD 또는 SSMS 물리적DB설계
    - UI/UX 디자인 16일(목) 내용전달


## 2일차
- 미니프로젝트
    - 프로젝트 문서
    - Notion 팀 프로젝트 템플릿 활용
    - UI/UX 디자인 툴 설명
        - https://ovenapp.io/ 카카오
        - https://www.figma.com/ 피그마
        - https://app.moqops.com/ 목업디자인 사이트
    - 프레젠테이션
        - https://www.miricanvas.com/kr 미리캔버스 활용 추천
    - 라즈베리파이(RPi) 리셋팅, 네트워크 설정, VNC(OS UI작업)

- 스마트홈 연동 클래스 미니프로젝트
    1. 요구사항 정의, 기능명세
    2. UI/UX 디자인
        - RPi는 디자인 없음(콘솔)
        - 데이터 수신앱
        - 모니터링 앱
    3. DB설계
    4. RPi 셋팅
    5. RPio GPIO, IoT디바이스 연결(카메라, HDT센서, RGB LED, ...)
    6. RPi 데이터 전송 파이썬 프로그래밍
    7. PC(Server) 데이터 수신 C# 프로그래밍
    8. 모니터링 앱 개발(수신 및 송신)

## 3일차
- 미니프로젝트
    - 실무 프로젝트 문서
    - Jira 사용법
    - 조별로 진행

- 라즈베리파이 셋팅
    1. RPi 기본 구성 - RPi + MicroSD + Power
    2. RPi 기본 셋팅
        - 한글화
        - 키보드 변경
        - 화면사이즈 변경(RealVNC)
        - Pi Apps 앱 설치 도우미 앱
        - Gitbub Desctop, Vs Code
        - 네트워크 확인 
        - RealVNC

- 스마트홈 연동 클래스 미니프로젝트
    - RPi 셋팅... 진행 

## 4일차
- 라즈베리파이 IoT장비 설치
    - [x] 라즈베리파이 카메라
    - [x] GPIO HAT
    - [x] 브레드보드와 연결
    - [ ] DHT11 센서
    - [x] RGB LED 모듈
        - V - 5V 연결
        - R - GPIO4 연결
        - B - GPIO5 연결
        - G - GPIO6 연결
    - [ ] 서보모터

## 5일차
- 미니 프로젝트
    - 팀별 구매목록 작성
    - 프로젝트 결정사항 공유
    - 발표자료 준비

## 6, 7일차
- 네트워크 대공사
    [x] 개인 공유기, PC, 라즈베리파이

- 스마트홈 연동 클래스 미니프로젝트
    - 온습도 센서, RGB LED
    - RPi <--> Windows 통신(MQTT)
    - WPF 모니터링 앱

- IoT 기기간 통신방법
    - Modbus - 시리얼통신으로 데이터 전송(완전 구식)
    - OPC UA - ModBus통신 불편한점 개선(아주 복잡)
    - **MQTT** - 가장 편리! AWS IoT, Azure IoT 클라우드 산업계표준으로 사용

- MQTT 통신
    - [x]Mosquitto Broker 설치
        - mosquitto.conf : listener 1883 0.0.0.0, allow_anoymous true
        - 방화벽 인바운드 열기
    - [x]RPi : paho-mqtt 패키지 설치, 송신(publisher)
    - Win/C# : M2MQTT, MQTT.NET Nuget패키지 설치, 수신(subcriber)
        - M2MQTT : 가볍게 쓸 수 있음. 업데이트가 안됨
        - MQTTNet : MS에서 개발, 무겁다. 최신까지 업데이트 잘됨

## 9일차
- 스마트홈 연동 클래스 미니프로젝트
    - [ ] WPF MQTT데이터 DB로 저장
    - [ ] MQTT데이터 실시간모니터링
    - [ ] MQTT RPi 제어(LED 제어)
    - [ ] WPF MQTT 데이터 히스토리 확인


## 10일차
- 스마트홈 연동 클래스 미니프로젝트 마무리
    - [x] WPF MQTT데이터 DB로 저장
    - [x] MQTT데이터 실시간모니터링
    - [x] MQTT RPi 제어(LED 제어)
    - [x] WPF MQTT 데이터 히스토리 확인
        - LiveChart2는 차후에 다시, 현재는 OxyPlot 차트 사용
    - 실행결과
    ![스마트홈1](https://raw.githubusercontent.com/inje98/miniprojects-2024/main/images/mp002.png)
    
    ![스마트홈2](https://raw.githubusercontent.com/inje98/miniprojects-2024/main/images/mp003.png)
    
    ![스마트홈3](https://raw.githubusercontent.com/inje98/miniprojects-2024/main/images/mp004.png)
        

- 조별 미니프로젝트 발표