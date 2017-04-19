// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="noor.alam.shuvo@gmail.com" company="">
// //   Copyright @ 2017
// // </copyright>
// <summary>
// // </summary>
// // --------------------------------------------------------------------------------------------------------------------

namespace StackoverflowNotification
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    using StackExchange.StacMan;

    using Tulpep.NotificationWindow;

    public partial class FormSettings : Form
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Tag { get; set; }
        public int PopupDelay { get; set; }
        public int TimerInterval { get; set; }


        PopupNotifier popupNotifier = new PopupNotifier();


        public FormSettings()
        {

            this.InitializeComponent();
            //this.InitializeTracBar();
            this.InitializePopupNotifierBasicSettings();

            // Setting default values
            this.TimerInterval = 5000;
            this.PopupDelay = 2000;
            Debug.WriteLine("Initializing formSetting with timeInterval = 5 and popupdelay = 2");
        }


        private void ButtonStartClick(object sender, EventArgs e)
        {
            this.buttonStart.Enabled = false;
            this.timer.Enabled = true;
            this.timer.Interval = this.TimerInterval;
            Debug.WriteLine("ButtonStart click before timer tick");
            this.timer.Tick += new EventHandler(this.OnTimerEvent);
            Debug.WriteLine("ButtonStart click after timer tick");
        }

        public StacManResponse<Question> FetchNotification()
        {
            Debug.WriteLine("FetchNotification method called");
            return
                new StacManClient().Questions.GetAll(
                    "stackoverflow",
                    page: 1,
                    pagesize: 10,
                    tagged: "sql-server",
                    order: Order.Desc).Result;
        }

        public void DisplayPopup(StacManResponse<Question> notificationMessage)
        {
            Debug.WriteLine("Popup delay methosd called");
            var i = 0;
            var popupText = new StringBuilder();
            var questions = notificationMessage.Data.Items;
            foreach (var question in questions)
            {
                popupText.AppendLine($"{i++}. {question.Title} [Link]");
            }

            //var popupNotifier = new PopupNotifier
            //                        {
            //                            TitleText = "Recent Stackoverflow Post",
            //                            BodyColor = Color.GhostWhite,
            //                            BorderColor = Color.DarkRed,
            //                            HeaderColor = Color.CadetBlue,
            //                            ContentText = popupText.ToString(),
            //                            Delay = 5000,
            //                            Size = new Size(500, questions.Count() * 22)
            //                        };
            popupNotifier.TitleText = "Recent Stackoverflow Post";
            popupNotifier.ContentText = popupText.ToString();
            popupNotifier.Delay = this.PopupDelay;
            popupNotifier.Size = new Size(500, questions.Count() * 22);

            popupNotifier.Popup();
        }

        public void InitializePopupNotifierBasicSettings()
        {
            this.popupNotifier.BodyColor = Color.GhostWhite;
            this.popupNotifier.BorderColor = Color.DarkRed;
            this.popupNotifier.HeaderColor = Color.CadetBlue;
        }

        public void OnTimerEvent(object source, EventArgs e)
        {
            Debug.WriteLine("Ontimer event method called");
            var fetchedMessage = this.FetchNotification();
            this.DisplayPopup(fetchedMessage);
        }

        private void ButtonStopClick(object sender, EventArgs e)
        {
            if (this.buttonStart.Enabled)
            {
                return;
            }
            this.timer.Stop();
            this.buttonStart.Enabled = true;
        }

        private void ButtonSearchClick(object sender, EventArgs e)
        {

        }

        private void TrackBarScroll(object sender, EventArgs e)
        {
            Debug.WriteLine($"tracbar value set to {trackBar.Value}");
            this.PopupDelay = this.trackBar.Value;
        }

        public void InitializeTracBar()
        {
            Debug.WriteLine($"Initialize tracbar called");
            this.trackBar.Maximum = 300;
            this.trackBar.Minimum = 1;
            this.trackBar.TickFrequency = 150;

        }
    }
}