using MahApps.Metro.Controls;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using SmartHomeMonitoringApp.Logics;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartHomeMonitoringApp.Views
{
    /// <summary>
    /// DataBaseControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DataBaseControl : UserControl
    {
        // 변수 또는 속성 선언
        public Boolean IsConnected { get; set; }
        Thread MqttThread { get; set; } // 없으면 UI컨트롤과 충돌나서 Log를 못찍음(응답없음)!
        int MaxCount { get; set; } = 10;// MQTT로그 과적으로 속도저하를 방지하기 위해
        public DataBaseControl()
        {
            InitializeComponent();
        }
        // 유저컨트롤 화면 로드된 이후 초기화
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TxtBrokerUrl.Text = Commons.BROKERHOST;
            TxtMqttTopic.Text = Commons.MQTTOPIC;
            TxtConnString.Text = Commons.DBCONNSTRING;

            IsConnected = false;
            BtnConnect.IsChecked = false;
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            ConnectSystem(); // DB, MQTT 연결
        }

        private async void ConnectSystem()
        {
            if (IsConnected == false) // 연결이 안됐으면 처리
            {
                // 한번도 접속을 안했으면 모두 연결
                var mqttFactory = new MqttFactory();
                Commons.MQTT_CLIENT = mqttFactory.CreateMqttClient();
                // MQTT 브로커 아이피 연결속성
                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(TxtBrokerUrl.Text).Build();

                await Commons.MQTT_CLIENT.ConnectAsync(mqttClientOptions, CancellationToken.None); // MQTT연결
                Commons.MQTT_CLIENT.ApplicationMessageReceivedAsync += MQTT_CLIENT_ApplicationMessageReceivedAsync;

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder().WithTopicFilter(
                    f =>
                    {
                        f.WithTopic(Commons.MQTTOPIC);
                    }).Build();

                await Commons.MQTT_CLIENT.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);


                IsConnected = true;
                BtnConnect.IsChecked = true;
                BtnConnect.Content = "MQTT 연결중";
            } else
            {
                // 연결했다가 다시 버튼 누르면 정지
                if (Commons.MQTT_CLIENT.IsConnected)
                {
                    Commons.MQTT_CLIENT.ApplicationMessageReceivedAsync -= MQTT_CLIENT_ApplicationMessageReceivedAsync;
                    await Commons.MQTT_CLIENT.DisconnectAsync();

                    IsConnected = false;
                    BtnConnect.IsChecked = false;
                    BtnConnect.Content = "Connect";
                }
            }
        }

        private Task MQTT_CLIENT_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            var payload = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);
            // Debug.WriteLine(payload);
            UpdateLog(payload); // TextBox에 추가
            InsertData(payload); // DB에 저장

            return Task.CompletedTask; // Async에서 Task값을 넘겨줄려면 이렇게...
        }

        private void InsertData(string payload)
        {
            this.Invoke(() =>
            {
                var currValue = JsonConvert.DeserializeObject<Dictionary<string, string>>(payload);

                // Debug.WriteLine("insertData : " + currValue["CURR_DT"]);
                // currValue["Dev_ID"], currValue["VALUE"]
                if (currValue != null)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(TxtConnString.Text))
                        {
                            conn.Open();
                            var insQuery = @"INSERT INTO [dbo].[SmartHomeData]
                                                    ([DEV_ID]
                                                    ,[CURR_DT]
                                                    ,[TEMP]
                                                    ,[HUMID])
                                              VALUES
                                                    (@DEV_ID
                                                    ,@CURR_DT
                                                    ,@TEMP
                                                    ,@HUMID)";
                            SqlCommand cmd = new SqlCommand(insQuery, conn);
                            cmd.Parameters.AddWithValue("@DEV_ID", currValue["DEV_ID"]);
                            cmd.Parameters.AddWithValue("@CURR_DT", currValue["CURR_DT"]); // string --> DateTime 자동 변환됨
                                                                                           // cmd.Parameters.AddWithValue("@TYPE", currValue["TYPE"]);
                            var spliteValue = currValue["VALUE"].Split('|'); // spliteValue[0] = 온도, splitValue[1] = 습도
                            cmd.Parameters.AddWithValue("@TEMP", spliteValue[0]);
                            cmd.Parameters.AddWithValue("@HUMID", spliteValue[1]);

                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                UpdateLog(">>> DB Insert succeed");
                            }
                            else
                            {
                                UpdateLog(">>> Insert failed");
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        UpdateLog($"DB 에러발생 : {ex.Message}");
                    }
                }
            });
            
        }

        private void UpdateLog(string payload)
        {
            this.Invoke(() =>
            {
                TxtLog.Text += $"{payload}\n";
                TxtLog.ScrollToEnd();
            });
        }
    }
}
