using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebBrowser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.txtUrl.Text = "http://saishi.cnki.net/exam/Exam/Start/sj2afbb1c3-a9ac-4106-b396-010c73133b2b";
            this.txtUrl.Text = "http://saishi.cnki.net/passport/Account/Login";
            //this.txtUrl.Text = "http://127.0.0.1/abc.html";
            this.webBrowser.Navigate(this.txtUrl.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.webBrowser.Navigate(this.txtUrl.Text);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string tigan = "";
            string answer = "";
            HtmlElementCollection divs = this.webBrowser.Document.GetElementsByTagName("div");
            foreach (HtmlElement item in divs)
            {
                if (item == null || item.InnerHtml == null)
                    continue;
                var str = item.OuterHtml.Replace(item.InnerHtml, "");
                str = str.Replace("\"", "");
                var classVal = str.Substring(str.IndexOf("class=") + 6, str.IndexOf("><") - (str.IndexOf("class=") + 6));
                if (!string.IsNullOrEmpty(classVal))
                {
                    if (string.IsNullOrEmpty(answer) && classVal == "tigan")
                    {
                        tigan = item.InnerHtml;
                        answer = GetAnswer(tigan);
                    }
                    if (classVal == "iradio_square-green")
                    {
                        var inputEle = item.FirstChild;
                        var inputEleVal = inputEle.GetAttribute("value");
                        if (inputEle != null && answer.Contains(inputEleVal))
                        {
                            Thread.Sleep(1000);
                            MessageBox.Show("选" + inputEleVal);
                            inputEle.InvokeMember("click");                           
                        }

                    }
                }
            }
        }

        private string GetAnswer(string tigan)
        {
            //if (tigan == "成语“闻鸡起舞”、“中流击楫”是出自哪个古代将领的故事？")
            //{
            //    return "A";
            //}
            //else
            //{
            //    return "B";
            //}\
            string[] arr = new[] { "A", "B", "C", "D" };
            return arr[new Random().Next(0, 4)];
        }

        //http://blog.csdn.net/taoerchun/article/details/49782739
        private void InjectScript()
        {
            HtmlElement ele = this.webBrowser.Document.CreateElement("script");
            ele.SetAttribute("type", "text/javascript");
            ele.SetAttribute("text", @"
");

            // 注入.
            webBrowser.Document.Body.AppendChild(ele);
            object result = webBrowser.Document.InvokeScript("GetPictureData");
            

            //MessageBox.Show(buff.ToString());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Login()
        {
            var usernameEle = GetElementByName("input", "user-name");
            usernameEle.SetAttribute("value", "15201106926");

            var pwd = GetElementByName("input", "password");
            pwd.SetAttribute("value", "woshishen");

            var loginBtn = webBrowser.Document.Window.Frames[0].Document.GetElementById("dologin");
            //loginBtn.InvokeMember("click");

        }

        private HtmlElement GetElementByName(string type, string name)
        {
            HtmlElementCollection inputs = webBrowser.Document.Window.Frames[0].Document.GetElementsByTagName(type);
            foreach (HtmlElement item in inputs)
            {
                if (item.Name.ToLower() == name.ToLower())
                    return item;
            }
            return null;
        }

        private void btnFillData_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            var url = e.Url;
            this.txtUrl.Text = url.ToString();
            if (url.ToString() == "http://saishi.cnki.net/")
            {
                //var url2 = "http://saishi.cnki.net/exam/Exam/Answer/kjf0e12af0-9870-11e7-8221-1866dae6f470";
                //url2 = "http://127.0.0.1/abc.html";
                //this.webBrowser.Navigate(url2);
            }
            //this.webBrowser.Navigate("http://saishi.cnki.net/exam/Exam/Start/sj2afbb1c3-a9ac-4106-b396-010c73133b2b");
        }

        private void webBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;//取消打开新窗口动作
            try
            {
                //获取要打开的新窗口链接地址
                string url = this.webBrowser.Document.ActiveElement.GetAttribute("href");
                this.webBrowser.Navigate(url);
            }
            catch
            {
            }
        }

        private void webBrowser_ControlAdded(object sender, ControlEventArgs e)
        {
            
        }
    }

    //private void btnInput_Click(object sender, EventArgs e)
    //{
    //    var input = this.webBrowser1.Document.GetElementById("kw");
    //    input.SetAttribute("value", "好例子网");
    //}

    //private void btnOper_Click(object sender, EventArgs e)
    //{
    //    var button = this.webBrowser1.Document.GetElementById("su");
    //    button.InvokeMember("click");
    //}
}
