using ArcSonglistMigrator;
using System.ComponentModel;

namespace ArcaeaPatcher
{
    public partial class DifficultiesTextEditor : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DifficultiesText { get; set; }

        List<Difficulty> Diffs;

        public DifficultiesTextEditor(string text)
        {
            InitializeComponent();
            Diffs = new List<Difficulty>();
            if (!string.IsNullOrEmpty(text))
            {
                var lines = text.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var parts = line.Split([", "], StringSplitOptions.RemoveEmptyEntries);
                    var difficulty = new Difficulty();
                    foreach (var part in parts)
                    {
                        if (part.StartsWith("难度等级: "))
                        {
                            int ratingClass;
                            if (int.TryParse(part.Substring(6).Trim(), out ratingClass))
                            {
                                difficulty.RatingClass = ratingClass;
                            }
                        }
                        else if (part.StartsWith("谱面设计师: "))
                        {
                            difficulty.ChartDesigner = part.Substring(7).Trim();
                        }
                        else if (part.StartsWith("封面设计师: "))
                        {
                            difficulty.JacketDesigner = part.Substring(7).Trim();
                        }
                        else if (part.StartsWith("评级: "))
                        {
                            int rating;
                            if (int.TryParse(part.Substring(4).Trim(), out rating))
                            {
                                difficulty.Rating = rating;
                            }
                        }
                        else if (part.StartsWith("评级附加: "))
                        {
                            bool ratingPlus;
                            if (bool.TryParse(part.Substring(6).Trim(), out ratingPlus))
                            {
                                difficulty.RatingPlus = ratingPlus;
                            }
                        }
                    }
                    Diffs.Add(difficulty);
                }
            }
            foreach (var difficulty in Diffs)
            {
                RatingClassSet.Items.Add(difficulty.RatingClass);
            }
        }

        private void ApplyChangeButton_Click(object sender, EventArgs e)
        {
            DifficultiesText = string.Join(Environment.NewLine, Diffs?.Select(d => $"难度等级: {d.RatingClass}, 谱面设计师: {d.ChartDesigner}, 封面设计师: {d.JacketDesigner}, 评级: {d.Rating}, 评级附加: {d.RatingPlus}"));
            DialogResult = DialogResult.OK;
            Dispose();
        }

        private void RatingClassSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var difficulty in Diffs)
            {
                if (difficulty.RatingClass.ToString() == RatingClassSet.SelectedItem.ToString())
                {
                    ChartDesigner_Text.Text = difficulty.ChartDesigner.ToString();
                    JacketDesigner_Text.Text = difficulty.JacketDesigner.ToString();
                    RatingNum.Value = difficulty.Rating;
                    RatingPlus.Checked = difficulty.RatingPlus;
                }
            }
        }

        private void RatingPlus_CheckedChanged(object sender, EventArgs e)
        {
            if(RatingClassSet.SelectedItem == null)
            {
                return;
            }
            foreach (var difficulty in Diffs)
            {
                if (difficulty.RatingClass.ToString() == RatingClassSet.SelectedItem.ToString())
                {
                    difficulty.RatingPlus = RatingPlus.Checked;
                }
            }
        }

        private void RatingNum_ValueChanged(object sender, EventArgs e)
        {
            if (RatingClassSet.SelectedItem == null)
            {
                return;
            }
            foreach (var difficulty in Diffs)
            {
                if (difficulty.RatingClass.ToString() == RatingClassSet.SelectedItem.ToString())
                {
                    difficulty.Rating = (int)RatingNum.Value;
                }
            }
        }

        private void JacketDesigner_Text_TextChanged(object sender, EventArgs e)
        {
            if (RatingClassSet.SelectedItem == null)
            {
                return;
            }
            foreach (var difficulty in Diffs)
            {
                if (difficulty.RatingClass.ToString() == RatingClassSet.SelectedItem.ToString())
                {
                    difficulty.JacketDesigner = JacketDesigner_Text.Text;
                }
            }
        }

        private void ChartDesigner_Text_TextChanged(object sender, EventArgs e)
        {
            if (RatingClassSet.SelectedItem == null)
            {
                return;
            }
            foreach (var difficulty in Diffs)
            {
                if (difficulty.RatingClass.ToString() == RatingClassSet.SelectedItem.ToString())
                {
                    difficulty.ChartDesigner = ChartDesigner_Text.Text;
                }
            }
        }
    }
}