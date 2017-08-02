var a = new AsyncChromeDriver.Core.a1();
var clFields = typeof(AsyncChromeDriver.Core.a1).GetFields();

var resStr = string.Join(Environment.NewLine, clFields.Where(v => v.Name != "EXECUTE_SQL").Select(v => "public string " + v.Name + " = \"" + string.Join("", (string[])v.GetValue(a)).Replace("\"", "\"\"") + "\";"));
System.Windows.Clipboard.SetText(resStr);