using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.VisualElements;
using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;
using SmartHomeMonitoringApp.Logics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
    /// VisualizationControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class VisualizationControl : UserControl
    {
        public VisualizationControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DtpStart.Text = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
            DtpEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        // 검색버튼 클릭 이벤트 핸들러
        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(Commons.DBCONNSTRING)) 
            {
                conn.Open();
                var selQuery = @"SELECT [Idx]
                                          ,[DEV_ID]
                                          ,[CURR_DT]
                                         , CONVERT(CHAR(10), CURR_DT, 23) AS CURR_D
                                          ,[TEMP]
                                          ,[HUMID]
                                      FROM [dbo].[SmartHomeData]
                                     WHERE CONVERT(CHAR(10), CURR_DT, 23) BETWEEN @StartDt AND @EndDt";
                SqlCommand cmd = new SqlCommand(selQuery, conn);
                cmd.Parameters.AddWithValue("@StartDt", DtpStart.Text);
                cmd.Parameters.AddWithValue("@EndDt", DtpEnd.Text);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds, "SmartHomeData");
            }

            // MessageBox.Show("TotalData", ds.Tables["SmartHomeData"].Rows.Count.ToString());
            LblTotalCount.Content = $"검색데이터 : {ds.Tables["SmartHomeData"].Rows.Count} 개";

            var tmp = new PlotModel { Title = "SmartHome Visualization", DefaultFont = "NanumGothic" };
            var legend = new Legend
            {
                LegendBorder = OxyColors.DarkGray,
                LegendBackground = OxyColor.FromArgb(150, 255, 255, 255),
                LegendPosition = LegendPosition.TopRight,
                LegendPlacement = LegendPlacement.Inside,
            };
            tmp.Legends.Add(legend);

            var tempSeries = new LineSeries
            {
                Title = "Temp(℃)",
                MarkerType = MarkerType.Circle,
                Color = OxyColors.DarkOrange, // 라인색 오렌지
            };

            var humidSeries = new LineSeries
            {
                Title = "Temp(%)",
                MarkerType = MarkerType.Square,
                Color = OxyColors.Aqua, // 아쿠아
            };

            if (ds.Tables["SmartHomeData"].Rows.Count > 0 )
            {
                var count = 0;
                foreach (DataRow row in ds.Tables["SmartHomeData"].Rows)
                {
                    tempSeries.Points.Add(new DataPoint(count, Convert.ToDouble(row[4])));
                    humidSeries.Points.Add(new DataPoint(count, Convert.ToDouble(row[5])));

                    count++;
                }
            }

            tmp.Series.Add(tempSeries); //온도값 시리즈 할당
            tmp.Series.Add(humidSeries); // 습도 시리즈 할당

            // OxyPlot 차트에 할당!
            ChtResult.Model = tmp;
        }

    }
}
