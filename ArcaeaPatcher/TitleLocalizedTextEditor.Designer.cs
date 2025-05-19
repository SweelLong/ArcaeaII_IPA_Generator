namespace ArcaeaPatcher
{
    partial class TitleLocalizedTextEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TitleLocalizedTextEditor));
            En_Label = new Label();
            Ja_Label = new Label();
            Ko_Label = new Label();
            Hans_Label = new Label();
            Hant_Label = new Label();
            En_Text = new TextBox();
            Ja_Text = new TextBox();
            Ko_Text = new TextBox();
            Hans_Text = new TextBox();
            Hant_Text = new TextBox();
            ApplyChangeButton = new Button();
            SuspendLayout();
            // 
            // En_Label
            // 
            En_Label.AutoSize = true;
            En_Label.Location = new Point(12, 18);
            En_Label.Name = "En_Label";
            En_Label.Size = new Size(44, 17);
            En_Label.TabIndex = 2;
            En_Label.Text = "英文：";
            // 
            // Ja_Label
            // 
            Ja_Label.AutoSize = true;
            Ja_Label.Location = new Point(12, 63);
            Ja_Label.Name = "Ja_Label";
            Ja_Label.Size = new Size(44, 17);
            Ja_Label.TabIndex = 3;
            Ja_Label.Text = "日文：";
            // 
            // Ko_Label
            // 
            Ko_Label.AutoSize = true;
            Ko_Label.Location = new Point(12, 110);
            Ko_Label.Name = "Ko_Label";
            Ko_Label.Size = new Size(44, 17);
            Ko_Label.TabIndex = 4;
            Ko_Label.Text = "韩文：";
            // 
            // Hans_Label
            // 
            Hans_Label.AutoSize = true;
            Hans_Label.Location = new Point(12, 152);
            Hans_Label.Name = "Hans_Label";
            Hans_Label.Size = new Size(44, 17);
            Hans_Label.TabIndex = 5;
            Hans_Label.Text = "简中：";
            // 
            // Hant_Label
            // 
            Hant_Label.AutoSize = true;
            Hant_Label.Location = new Point(12, 199);
            Hant_Label.Name = "Hant_Label";
            Hant_Label.Size = new Size(44, 17);
            Hant_Label.TabIndex = 6;
            Hant_Label.Text = "繁中：";
            // 
            // En_Text
            // 
            En_Text.Location = new Point(62, 18);
            En_Text.Name = "En_Text";
            En_Text.Size = new Size(242, 23);
            En_Text.TabIndex = 7;
            // 
            // Ja_Text
            // 
            Ja_Text.Location = new Point(62, 60);
            Ja_Text.Name = "Ja_Text";
            Ja_Text.Size = new Size(242, 23);
            Ja_Text.TabIndex = 8;
            // 
            // Ko_Text
            // 
            Ko_Text.Location = new Point(62, 104);
            Ko_Text.Name = "Ko_Text";
            Ko_Text.Size = new Size(242, 23);
            Ko_Text.TabIndex = 9;
            // 
            // Hans_Text
            // 
            Hans_Text.Location = new Point(62, 149);
            Hans_Text.Name = "Hans_Text";
            Hans_Text.Size = new Size(242, 23);
            Hans_Text.TabIndex = 10;
            // 
            // Hant_Text
            // 
            Hant_Text.Location = new Point(62, 199);
            Hant_Text.Name = "Hant_Text";
            Hant_Text.Size = new Size(242, 23);
            Hant_Text.TabIndex = 11;
            // 
            // ApplyChangeButton
            // 
            ApplyChangeButton.Location = new Point(321, 179);
            ApplyChangeButton.Name = "ApplyChangeButton";
            ApplyChangeButton.Size = new Size(75, 43);
            ApplyChangeButton.TabIndex = 12;
            ApplyChangeButton.Text = "应用";
            ApplyChangeButton.UseVisualStyleBackColor = true;
            ApplyChangeButton.Click += ApplyChangeButton_Click;
            // 
            // TitleLocalizedTextEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(408, 251);
            Controls.Add(ApplyChangeButton);
            Controls.Add(Hant_Text);
            Controls.Add(Hans_Text);
            Controls.Add(Ko_Text);
            Controls.Add(Ja_Text);
            Controls.Add(En_Text);
            Controls.Add(Hant_Label);
            Controls.Add(Hans_Label);
            Controls.Add(Ko_Label);
            Controls.Add(Ja_Label);
            Controls.Add(En_Label);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "TitleLocalizedTextEditor";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "歌名本地化信息编辑器";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label En_Label;
        private Label Ja_Label;
        private Label Ko_Label;
        private Label Hans_Label;
        private Label Hant_Label;
        private TextBox En_Text;
        private TextBox Ja_Text;
        private TextBox Ko_Text;
        private TextBox Hans_Text;
        private TextBox Hant_Text;
        private Button ApplyChangeButton;
    }
}