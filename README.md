[TO-DO]
    1. centos 로 cloud 서버 재생성
       - 1대만 생성
       - jenkins 설치
    2. 데이터 지속 적재
    3. 데이터 분석 툴 / WEB APP / vuejs
       - 업데이트 worker

[환경설정]
     1. vscode  설치
     2. git 설치
     3. dotnet core 6.0 설치 (dotnet-sdk-6.0.302-win-x64)
     4. my.race.api 에서 dotnet restore
     5. my.race.worker 에서 dotnet restore
     6. 참조 경로 (restore)
          - dotnet add package Dapper
          - dotnet add package Mysql.Data
          - dotnet add package Newtonsoft.Json
          - dotnet add package Microsoft.Extensions.Logging
          - add reference ../my.race.model/my.race.model.csproj

[서버 방화벽 오픈 - 오라클 리눅스 (centos)]
     sudo systemctl firewalld stop
     sudo vi /etc/firewalld/zones/public.xml
     sudo systemctl firewalld start

[서버 배포 - 서비스 재확인]
     serverice 만들어서 실행
     sudo cp mj-worker.service /etc/systemd/system/mj-worker.service
     sudo cp mj-service.service /etc/systemd/system/mj-service.service

     sudo systemctl daemon-reload
     sudo systemctl enable mj-worker.service;sudo systemctl enable mj-service.service

     sudo systemctl stop mj-worker.service;sudo systemctl stop mj-service.service;
     sudo systemctl start mj-worker.service;sudo systemctl start mj-service.service
    
     sudo systemctl status mj-worker.service 

[개발 완료후 배포 패키지 준비]
     sudo dotnet publish -c Release -o ../mj.publish 
     -- 이후 push 한다음 서버에서 받아서 서비스 재시작

[기능 동작]
    - RaceResult 에 결과 업로드
    - RaceSchedule 에 예정 업로드 (시점이 언제임?)
    
    1. 결과 뷰어 (vue.js / materialDesign참고)
    2. 예측 뷰어, 계산 정보 (vue.js / materialDesign참고)
    3. URL/시스템접속정보등은 configure 로 이동
    4. docker 배포


[예측모델 및 변수들]
     Master Data
     - Horse
     - Rider
     - Trainer
     - Owner

     Peridic Data
     - Race Schedule / Result

     Varient
     - Ranking
     - Competition (Horse / Rider)
     - Race Time / Number
     - Weather

     Predical Data
     - By Portion
     
     Predical Result
     
[DB 생성]     
[공공 API 변경]
결과 다른 API 로참조 : 한국마사회_경주별상세성적표
순위에 따라 적용할수 있도록 함     


[UI]

[CORS 회피]