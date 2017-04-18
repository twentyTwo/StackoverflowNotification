using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StackoverflowNotification
{
    using StackExchange.StacMan;

    using Tulpep.NotificationWindow;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {



            var popupNotifier = new PopupNotifier();
            var client  = new StacManClient();


            var response = client.Questions.GetAll("stackoverflow",
            page: 1,
            pagesize: 10,
            tagged:"sql-server",
            order: Order.Desc).Result;

            StringBuilder content = new StringBuilder();

            int i = 0;

            foreach (var question in response.Data.Items)
            {
                content.Append($"{++i}. {question.Title}\n");
            }


            popupNotifier.TitleText = "Recent Stackoverflow Post";
            popupNotifier.BodyColor = Color.GhostWhite;
            popupNotifier.BorderColor = Color.DarkRed;
            popupNotifier.HeaderColor = Color.CadetBlue;

            popupNotifier.ContentText = content.ToString();
            popupNotifier.IsRightToLeft = false;
            popupNotifier.Size = new Size(500, 20*i);
            popupNotifier.Popup();
        }
    }
}
