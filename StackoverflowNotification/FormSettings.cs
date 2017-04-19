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
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    using StackExchange.StacMan;

    using Tulpep.NotificationWindow;

    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            this.InitializeComponent();
            this.Interval = 5000;
        }

        public int Interval { get; set; }

        private void ButtonStartClick(object sender, EventArgs e)
        {
            this.button1.Enabled = false;
            this.timer.Interval = this.Interval;
            this.timer.Enabled = true;
            this.timer.Tick += new EventHandler(this.OnTimerEvent);
        }

        public StacManResponse<Question> FetchNotification()
        {
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
            var i = 0;
            var popupText = new StringBuilder();
            var questions = notificationMessage.Data.Items;
            foreach (var question in questions)
            {
                popupText.AppendLine($"{i++}. {question.Title} [Link]");
            }

            var popupNotifier = new PopupNotifier
                                    {
                                        TitleText = "Recent Stackoverflow Post",
                                        BodyColor = Color.GhostWhite,
                                        BorderColor = Color.DarkRed,
                                        HeaderColor = Color.CadetBlue,
                                        ContentText = popupText.ToString(),
                                        Delay = 5000,
                                        Size = new Size(500, questions.Count() * 22)
                                    };

            popupNotifier.Popup();
        }

        public void OnTimerEvent(object source, EventArgs e)
        {
            var fetchedMessage = this.FetchNotification();
            this.DisplayPopup(fetchedMessage);
        }

        private void ButtonStopClick(object sender, EventArgs e)
        {
            if (this.button1.Enabled)
            {
                return;
            }
            this.timer.Stop();
            this.button1.Enabled = true;
        }

    }
}