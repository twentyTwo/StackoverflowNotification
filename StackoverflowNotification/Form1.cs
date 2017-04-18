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
    using System.Text;
    using System.Windows.Forms;

    using StackExchange.StacMan;

    using Tulpep.NotificationWindow;

    public partial class Form1 : Form
    {
        public Form1()
        {
            this.InitializeComponent();
            this.Interval = 10000;
        }

        public int Interval { get; set; }

        private void Button1Click(object sender, EventArgs e)
        {
            this.button1.Enabled = false;
            this.timer1.Interval = this.Interval;
            this.timer1.Enabled = true;
            this.timer1.Tick += new EventHandler(this.OnTimerEvent);
        }

        public string FetchNotification()
        {
            var client = new StacManClient();

            var response =
                client.Questions.GetAll("stackoverflow", page: 1, pagesize: 10, tagged: "sql-server", order: Order.Desc)
                    .Result;

            var content = new StringBuilder();

            var i = 0;

            foreach (var question in response.Data.Items)
            {
                content.Append($"{++i}. {question.Title}\n");
            }

            return content.ToString();
        }

        public void DisplayPopup(string popupContent)
        {
            var popupNotifier = new PopupNotifier
                                    {
                                        TitleText = "Recent Stackoverflow Post",
                                        BodyColor = Color.GhostWhite,
                                        BorderColor = Color.DarkRed,
                                        HeaderColor = Color.CadetBlue,
                                        ContentText = popupContent,
                                        IsRightToLeft = false,
                                        Size = new Size(500, 500)
                                    };

            popupNotifier.Popup();
        }

        public void OnTimerEvent(object source, EventArgs e)
        {
            Debug.WriteLine("Inside timer");
            var fetchedMessage = this.FetchNotification();
            this.DisplayPopup(fetchedMessage);
        }

        private void Button2Click(object sender, EventArgs e)
        {
            if (this.button1.Enabled)
            {
                return;
            }
            this.timer1.Stop();
            this.button1.Enabled = true;
        }
    }
}