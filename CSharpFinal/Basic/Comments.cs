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
using System.Drawing;

namespace Basic
{
    public partial class Comments: Form
    {
          private Poem poem;
          private HttpClient client;
          private string account;
          private int poemId;
    public class newComment
    {

      public string Account { get; set; }
      public int poemId { get; set; }
      public DateTime created { get; set; }
      public newComment(string account, int poemId)
      {

        this.Account = account;
        this.poemId = poemId;
        this.created = DateTime.Now;
      }
      public override string ToString()

      {

        return $" {Account}:{poemId}:{created}";

      }
    }
    public Comments(string account,int id)
        {
            InitializeComponent();
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, err) => true;
            client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            this.account = account;
            poemId = id;
            getComments(id);
    }

  

    private void uiButton1_Click(object sender, EventArgs e)
    {
      int i=0;  
      if(uiButton1.FillColor == Color.White)
      {
        i++;
        uiButton1.FillColor = Color.Red;
        uiButton1.Text = "赞" + i;
      }
      else
      {
        i--;
        uiButton1.FillColor = Color.White;
        uiButton1.Text = "赞" + i;
      }
    }

    public void getComments(int id)
    {
      var task = client.GetStringAsync("https://localhost:5001/api/comment?poemId=" + id);
      commentBindingSource.DataSource =
          JsonConvert.DeserializeObject<List<Comment>>(task.Result);
      commentGridView.DataSource = commentBindingSource;
      //MessageBox.Show(task.Result);
    }
    private void Comments_Load(object sender, EventArgs e)
    {

    }

    private void uiButton2_Click(object sender, EventArgs e)
    {
      var myComment = new newComment(account, poemId);
      var json = JsonConvert.SerializeObject(myComment);
      HttpContent data = new StringContent(json);
      data.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
      var task = client.PostAsync("https://localhost:5001/api/comment", data);
      getComments(poemId);
    }

    private void commentGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
      
    }

    private void uiButton3_Click(object sender, EventArgs e)
    {
      Comment comment = (Comment)commentBindingSource.Current;
      Replys replys = new Replys(account, comment.Id, comment.UserAccount,poemId);
      replys.Show();
    }
  }
}
