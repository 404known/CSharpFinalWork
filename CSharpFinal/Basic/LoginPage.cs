﻿using ImportData;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Basic
{
    public partial class LoginPage : Form
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            if (judgeLogin())
            {
                MainPage main = new MainPage(AccountText.Text);
                main.FormClosed += (x, y) => this.Close();
                main.Show();
                this.Visible=false;
            }
            else
            {
                MessageBox.Show("用户名或密码错误!", "登录失败", MessageBoxButtons.OK);
            }
        }

        private bool judgeLogin()
        {
            String account = AccountText.Text;
            String password = PasswordText.Text;
            XmlDocument serverDoc = new XmlDocument();
            serverDoc.Load("../../../serverIp.xml");
            XmlNode node=serverDoc.SelectSingleNode("serverIp");
            string baseUrl = "https://"+node.InnerText+":5001/api/userinfo";

            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, err) => true;
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var task= client.GetStringAsync(baseUrl + "?account=" + account+"&password="+password);
            bool  flag = JsonConvert.DeserializeObject<Boolean>(task.Result);
            return flag;
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            RegisterPage register = new RegisterPage();
            register.Show();
        }
    }
}
