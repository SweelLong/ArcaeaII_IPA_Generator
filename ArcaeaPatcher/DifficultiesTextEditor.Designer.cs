namespace ArcaeaPatcher
{
    partial class DifficultiesTextEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DifficultiesTextEditor));
            En_Label = new Label();
            ApplyChangeButton = new Button();
            RatingClassSet = new ListBox();
            DiffClassLabel = new Label();
            ChartDesigner_Label = new Label();
            ChartDesigner_Text = new TextBox();
            JacketDesigner_Label = new Label();
            JacketDesigner_Text = new TextBox();
            RatingNum = new NumericUpDown();
            Rating_Label = new Label();
            RatingPlus = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)RatingNum).BeginInit();
            SuspendLayout();
            // 
            // En_Label
            // 
            En_Label.AutoSize = true;
            En_Label.Location = new Point(12, 18);
            En_Label.Name = "En_Label";
            En_Label.Size = new Size(0, 17);
            En_Label.TabIndex = 2;
            // 
            // ApplyChangeButton
            // 
            ApplyChangeButton.Location = new Point(270, 156);
            ApplyChangeButton.Name = "ApplyChangeButton";
            ApplyChangeButton.Size = new Size(75, 43);
            ApplyChangeButton.TabIndex = 12;
            ApplyChangeButton.Text = "应用";
            ApplyChangeButton.UseVisualStyleBackColor = true;
            ApplyChangeButton.Click += ApplyChangeButton_Click;
            // 
            // RatingClassSet
            // 
            RatingClassSet.FormattingEnabled = true;
            RatingClassSet.Location = new Point(12, 29);
            RatingClassSet.Name = "RatingClassSet";
            RatingClassSet.Size = new Size(129, 89);
            RatingClassSet.TabIndex = 13;
            RatingClassSet.SelectedIndexChanged += RatingClassSet_SelectedIndexChanged;
            // 
            // DiffClassLabel
            // 
            DiffClassLabel.AutoSize = true;
            DiffClassLabel.Location = new Point(12, 9);
            DiffClassLabel.Name = "DiffClassLabel";
            DiffClassLabel.Size = new Size(68, 17);
            DiffClassLabel.TabIndex = 14;
            DiffClassLabel.Text = "难度分类：";
            // 
            // ChartDesigner_Label
            // 
            ChartDesigner_Label.AutoSize = true;
            ChartDesigner_Label.Location = new Point(161, 9);
            ChartDesigner_Label.Name = "ChartDesigner_Label";
            ChartDesigner_Label.Size = new Size(68, 17);
            ChartDesigner_Label.TabIndex = 15;
            ChartDesigner_Label.Text = "谱面设计：";
            // 
            // ChartDesigner_Text
            // 
            ChartDesigner_Text.Location = new Point(161, 29);
            ChartDesigner_Text.Name = "ChartDesigner_Text";
            ChartDesigner_Text.Size = new Size(150, 23);
            ChartDesigner_Text.TabIndex = 16;
            ChartDesigner_Text.TextChanged += ChartDesigner_Text_TextChanged;
            // 
            // JacketDesigner_Label
            // 
            JacketDesigner_Label.AutoSize = true;
            JacketDesigner_Label.Location = new Point(161, 75);
            JacketDesigner_Label.Name = "JacketDesigner_Label";
            JacketDesigner_Label.Size = new Size(68, 17);
            JacketDesigner_Label.TabIndex = 17;
            JacketDesigner_Label.Text = "插画设计：";
            // 
            // JacketDesigner_Text
            // 
            JacketDesigner_Text.Location = new Point(161, 95);
            JacketDesigner_Text.Name = "JacketDesigner_Text";
            JacketDesigner_Text.Size = new Size(150, 23);
            JacketDesigner_Text.TabIndex = 18;
            JacketDesigner_Text.TextChanged += JacketDesigner_Text_TextChanged;
            // 
            // RatingNum
            // 
            RatingNum.Location = new Point(12, 154);
            RatingNum.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
            RatingNum.Name = "RatingNum";
            RatingNum.Size = new Size(129, 23);
            RatingNum.TabIndex = 19;
            RatingNum.ValueChanged += RatingNum_ValueChanged;
            // 
            // Rating_Label
            // 
            Rating_Label.AutoSize = true;
            Rating_Label.Location = new Point(12, 134);
            Rating_Label.Name = "Rating_Label";
            Rating_Label.Size = new Size(44, 17);
            Rating_Label.TabIndex = 20;
            Rating_Label.Text = "定数：";
            // 
            // RatingPlus
            // 
            RatingPlus.AutoSize = true;
            RatingPlus.Location = new Point(161, 156);
            RatingPlus.Name = "RatingPlus";
            RatingPlus.Size = new Size(36, 21);
            RatingPlus.TabIndex = 21;
            RatingPlus.Text = "+";
            RatingPlus.UseVisualStyleBackColor = true;
            RatingPlus.CheckedChanged += RatingPlus_CheckedChanged;
            // 
            // DifficultiesTextEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(364, 221);
            Controls.Add(RatingPlus);
            Controls.Add(Rating_Label);
            Controls.Add(RatingNum);
            Controls.Add(JacketDesigner_Text);
            Controls.Add(JacketDesigner_Label);
            Controls.Add(ChartDesigner_Text);
            Controls.Add(ChartDesigner_Label);
            Controls.Add(DiffClassLabel);
            Controls.Add(RatingClassSet);
            Controls.Add(ApplyChangeButton);
            Controls.Add(En_Label);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "DifficultiesTextEditor";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "难度信息编辑器";
            ((System.ComponentModel.ISupportInitialize)RatingNum).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label En_Label;
        private Button ApplyChangeButton;
        private ListBox RatingClassSet;
        private Label DiffClassLabel;
        private Label ChartDesigner_Label;
        private TextBox ChartDesigner_Text;
        private Label JacketDesigner_Label;
        private TextBox JacketDesigner_Text;
        private NumericUpDown RatingNum;
        private Label Rating_Label;
        private CheckBox RatingPlus;
    }
}