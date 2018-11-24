using System.Collections.Generic;
using System.Text;

namespace Zu.AsyncChromeDriver.Tests.Environment
{
    public class InlinePage
    {
        private string _title = string.Empty;
        private List<string> _scripts = new List<string>();
        private List<string> _styles = new List<string>();
        private List<string> _bodyParts = new List<string>();
        private string _onLoad;
        private string _onBeforeUnload;

        public InlinePage WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public InlinePage WithScripts(params string[] scripts)
        {
            _scripts.AddRange(scripts);
            return this;
        }

        public InlinePage WithStyles(params string[] styles)
        {
            _styles.AddRange(styles);
            return this;
        }

        public InlinePage WithBody(params string[] bodyParts)
        {
            _bodyParts.AddRange(bodyParts);
            return this;
        }

        public InlinePage WithOnLoad(string onLoad)
        {
            _onLoad = onLoad;
            return this;
        }

        public InlinePage WithOnBeforeUnload(string onBeforeUnload)
        {
            _onBeforeUnload = onBeforeUnload;
            return this;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("<html>");
            builder.Append("<head>");
            builder.AppendFormat("<title>{0}</title>", _title);
            builder.Append("</head>");
            builder.Append("<script type='text/javascript'>");
            foreach (string script in _scripts)
            {
                builder.Append(script).Append("\n");
            }

            builder.Append("</script>");
            builder.Append("<style>");
            foreach (string style in _styles)
            {
                builder.Append(style).Append("\n");
            }

            builder.Append("</style>");
            builder.Append("<body");
            if (!string.IsNullOrEmpty(_onLoad))
            {
                builder.AppendFormat(" onload='{0}'", _onLoad);
            }

            if (!string.IsNullOrEmpty(_onBeforeUnload))
            {
                builder.AppendFormat(" onbeforeunload='{0}'", _onBeforeUnload);
            }

            builder.Append(">");
            foreach (string bodyPart in _bodyParts)
            {
                builder.Append(bodyPart).Append("\n");
            }

            builder.Append("</body>");
            builder.Append("</html>");
            return builder.ToString();
        }
    }
}
