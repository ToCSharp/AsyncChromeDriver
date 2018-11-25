using System;
using System.IO;
using Newtonsoft.Json.Linq;

public partial class CreatePage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        StreamReader reader = new StreamReader(Request.InputStream);
        string requestFromPost = reader.ReadToEnd();
        var json = JToken.Parse(requestFromPost);
        var dir = json["dir"].ToString();
        var fileName = "temp.html";
        File.WriteAllText(Path.Combine(dir, fileName), json["content"].ToString());
        Response.ContentType = "text/html";

        Response.Output.Write(fileName);
    }
}
