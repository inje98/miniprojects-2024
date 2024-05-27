using MQTTnet.Client;

namespace SmartHomeMonitoringApp.Logics
{
    public class Commons
    {
        // Windows에 MQTT Broker가 설치되어 있으므로 가능한 아이피, 호스트
        // localhost, 127.0.0.1, 192.168.5.2
        public static string BROKERHOST { get; set; } = "192.168.5.2"; // Window Mqtt Broker Mosquitto IP
        public static string MQTTOPIC { get; set; } = "pknu/data/"; // 대소문자구분, 마지막 /도 중요!!
        public static string DBCONNSTRING { get; set; } = "Data Source=127.0.0.1;Initial Catalog=EMS;Persist Security Info=True;User ID=sa;Encrypt=False;" +
                                                          "Password=mssql_p@ss";

        public static IMqttClient MQTT_CLIENT { get; set; } // 전체 프로젝트에서 다 사용할 공용 MQTT클라이언트
    }
}
