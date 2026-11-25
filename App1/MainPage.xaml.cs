using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace App1
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer mytimer;
        public static readonly DependencyProperty SecondsProperty = DependencyProperty.Register("Seconds", typeof(int), typeof(MainPage), new PropertyMetadata(default(int)));
        public MainPage()
        {
            this.InitializeComponent();
            Seconds = 15;
            mytimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1d)
            };
            mytimer.Tick += Timer1_Count;
            mytimer.Start();
            MediaElement mediaElement1 = new MediaElement
            {
                Source = new Uri("http://hit.trkeurasia.ru:8000/hit128"),
                AreTransportControlsEnabled = true
            };
            mediaElement1.Volume = 0.5;
            mediaElement1.TransportControls.IsCompact = true;
            mediaElement1.TransportControls.IsFullWindowButtonVisible = false;
            mediaElement1.TransportControls.IsFullWindowEnabled = false;
            mediaElement1.AutoPlay = false;
            Grid1.Children.Add(mediaElement1);
        }

        public int Seconds
        {
            get { return (int)GetValue(SecondsProperty); }
            set { SetValue(SecondsProperty, value); }
        }

        private async void Timer1_Count(object sender, object e)
        {
            if (--Seconds <= 0)
            {
                var client = new HttpClient();
                var requestContent = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("", ""), });
                HttpResponseMessage response = await client.PostAsync("https://privateprojectminecraft.000webhostapp.com/sound.php", requestContent);
                HttpContent responseContent = response.Content;
                using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
                {
                    Textbox1.Text = await reader.ReadToEndAsync();
                }
                //getsound();
            }
        }

        /*private void getsound()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://smotri.radio/wp-content/themes/smotrihit/plugins/gettrackinfo.php");
            JArray array = new JArray();
            using (var twitpicResponse = (HttpWebResponse)request.GetResponse())
            {

                using (var reader = new StreamReader(twitpicResponse.GetResponseStream()))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var objText = reader.ReadToEnd();
                    JObject joResponse = JObject.Parse(objText);
                    JObject result = (JObject)joResponse["title"];
                    array = (JArray)result["title"];
                    string statu = array[0]["title"].ToString();
                    Console.WriteLine(statu);
                }

            }
        }*/
    }
}
