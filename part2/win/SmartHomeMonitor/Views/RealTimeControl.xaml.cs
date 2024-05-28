using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.VisualElements;
using MahApps.Metro.Controls;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using SmartHomeMonitoringApp.Logics;
using System;
using System.Collections.Generic;
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
using System.Windows.Threading;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;

namespace SmartHomeMonitoringApp.Views
{
    /// <summary>
    /// RealTimeControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RealTimeControl : UserControl
    {
        public RealTimeControl()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1); // 1초마다
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            LblSensingDt.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Commons.MQTT_CLIENT != null && Commons.MQTT_CLIENT.IsConnected)
            { // 이미 다른화면에서 MQTT를 연결했다면...
                Commons.MQTT_CLIENT.ApplicationMessageReceivedAsync += MQTT_CLIENT_ApplicationMessageReceivedAsync;
            }
            else
            {   // 한번도 연결한 적이 없으면.
                var mqttFactory = new MqttFactory();
                Commons.MQTT_CLIENT = mqttFactory.CreateMqttClient();
                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(Commons.BROKERHOST).Build();
                await Commons.MQTT_CLIENT.ConnectAsync(mqttClientOptions, CancellationToken.None);
                Commons.MQTT_CLIENT.ApplicationMessageReceivedAsync += MQTT_CLIENT_ApplicationMessageReceivedAsync;

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder().WithTopicFilter(
                    f =>
                    {
                        f.WithTopic(Commons.MQTTOPIC);
                    }).Build();
                await Commons.MQTT_CLIENT.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
            }
        }

        private Task MQTT_CLIENT_ApplicationMessageReceivedAsync(MQTTnet.Client.MqttApplicationMessageReceivedEventArgs arg)
        {
            var payload = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);
            Debug.WriteLine(payload);
            UpdateChart(payload);

            return Task.CompletedTask;
        }

        // 앵귤러차트를 위한 변수, 속성 선언
        public IEnumerable<ISeries> HumidSeries { get; set; }

        public IEnumerable<VisualElement<SkiaSharpDrawingContext>> VisualElements { get; set; }

        public NeedleVisual Needle { get; set; }

        private static void SetStyle(
        double sectionsOuter, double sectionsWidth, PieSeries<ObservableValue> series)
        {
            series.OuterRadiusOffset = sectionsOuter;
            series.MaxRadialColumnWidth = sectionsWidth;
        }
        // 여기까지. 그냥 복붙. 가져다 쓴거.



        private void UpdateChart(string payload)
        {
            // 차트에 값 대입해서 차트가 나오도록
            this.Invoke(() =>
            {
                var currValue = JsonConvert.DeserializeObject<Dictionary<string, string>>(payload);

                var splitValue = currValue["VALUE"].Split('|');
                var temp = Convert.ToDouble(splitValue[0]);
                var humid = Convert.ToDouble(splitValue[1]);

                // tempVal 여기에 차트에 그릴 습도값 들어오는듯
                var tempVal = GaugeGenerator.BuildSolidGauge(new GaugeItem(
                        temp,
                        series =>
                        {
                            series.MaxRadialColumnWidth = 50;
                            series.DataLabelsSize = 50;
                        }
                    ));
                ChtLivingTemp.Series = ChtDiningTemp.Series = ChtBadTemp.Series = ChtBathTemp.Series = tempVal;

                var sectionsOuter = 130;
                var sectionsWidth = 20;

                HumidSeries = GaugeGenerator.BuildAngularGaugeSections(
                    new GaugeItem(humid,
                    s => SetStyle(sectionsOuter, sectionsWidth, s))
                );
                // 습도 색차트 부분
                ChtDiningHumid.Series = ChtLivingHumid.Series = ChtBadHumid.Series = ChtBathHumid.Series = HumidSeries;

                // 습도를 나타낼 앵귤러 차트를 초기화

                Needle = new NeedleVisual { Value = humid };

                // VisualElements 여기에 바늘에 들어갈 습도 값 들어오는듯?
                VisualElements = new VisualElement<SkiaSharpDrawingContext>[]
                {
                new AngularTicksVisual
                {
                    LabelsSize = 12,
                    LabelsOuterOffset = 10,
                    OuterOffset = 65,
                    TicksLength = 15
                },
                Needle
                };
                // 습도 바늘 부분
                ChtLivingHumid.VisualElements = ChtDiningHumid.VisualElements = ChtBadHumid.VisualElements = ChtBathHumid.VisualElements =  VisualElements; // 위에서 만든 화면 디자인을 차트에 적용

            });
        }

        private void BtnWarning_Click(object sender, RoutedEventArgs e)
        {
            Commons.MQTT_CLIENT.PublishStringAsync("pknu/rcv/", "{'control' : 'warning'}");
        }

        private void BtnNormal_Click(object sender, RoutedEventArgs e)
        {
            Commons.MQTT_CLIENT.PublishStringAsync("pknu/rcv/", "{'control' : 'normal'}");

        }

        private void BtnOff_Click(object sender, RoutedEventArgs e)
        {
            Commons.MQTT_CLIENT.PublishStringAsync("pknu/rcv/", "{'control' : 'off'}");

        }
    }
}