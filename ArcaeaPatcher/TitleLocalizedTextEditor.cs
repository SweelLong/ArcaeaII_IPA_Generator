using ArcSonglistMigrator;
using System.ComponentModel;

namespace ArcaeaPatcher
{
    public partial class TitleLocalizedTextEditor : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TitleLocalizedText { get; set; }

        public TitleLocalizedTextEditor(string text)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(text))
            {
                var lines = text.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    if (line.StartsWith("英语: "))
                    {
                        En_Text.Text = line.Substring(3).Trim();
                    }
                    else if (line.StartsWith("日语: "))
                    {
                        Ja_Text.Text = line.Substring(3).Trim();
                    }
                    else if (line.StartsWith("韩语: "))
                    {
                        Ko_Text.Text = line.Substring(3).Trim();
                    }
                    else if (line.StartsWith("简体中文: "))
                    {
                        Hans_Text.Text = line.Substring(5).Trim();
                    }
                    else if (line.StartsWith("繁体中文: "))
                    {
                        Hant_Text.Text = line.Substring(5).Trim();
                    }
                }
            }
        }

        private void ApplyChangeButton_Click(object sender, EventArgs e)
        {
            var languageTexts = new List<string>();
            if (!string.IsNullOrEmpty(En_Text.Text))
            {
                languageTexts.Add($"英语: {En_Text.Text}");
            }
            if (!string.IsNullOrEmpty(Ja_Text.Text))
            {
                languageTexts.Add($"日语: {Ja_Text.Text}");
            }
            if (!string.IsNullOrEmpty(Ko_Text.Text))
            {
                languageTexts.Add($"韩语: {Ko_Text.Text}");
            }
            if (!string.IsNullOrEmpty(Hans_Text.Text))
            {
                languageTexts.Add($"简体中文: {Hans_Text.Text}");
            }
            if (!string.IsNullOrEmpty(Hant_Text.Text))
            {
                languageTexts.Add($"繁体中文: {Hant_Text.Text}");
            }
            TitleLocalizedText = string.Join(Environment.NewLine, languageTexts);
            DialogResult = DialogResult.OK;
            Dispose();
        }
    }
}