namespace ScreenRecording
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnStartRec = new Button();
            btnStopRec = new Button();
            richTextBox1 = new RichTextBox();
            SuspendLayout();
            // 
            // btnStartRec
            // 
            btnStartRec.Location = new Point(50, 320);
            btnStartRec.Name = "btnStartRec";
            btnStartRec.Size = new Size(75, 23);
            btnStartRec.TabIndex = 0;
            btnStartRec.Text = "Start";
            btnStartRec.UseVisualStyleBackColor = true;
            btnStartRec.Click += btnStartRec_Click;
            // 
            // btnStopRec
            // 
            btnStopRec.Location = new Point(182, 320);
            btnStopRec.Name = "btnStopRec";
            btnStopRec.Size = new Size(75, 23);
            btnStopRec.TabIndex = 1;
            btnStopRec.Text = "Stop";
            btnStopRec.UseVisualStyleBackColor = true;
            btnStopRec.Click += btnStopRec_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(106, 30);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(429, 191);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(richTextBox1);
            Controls.Add(btnStopRec);
            Controls.Add(btnStartRec);
            Name = "Main";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button btnStartRec;
        private Button btnStopRec;
        private RichTextBox richTextBox1;
    }
}