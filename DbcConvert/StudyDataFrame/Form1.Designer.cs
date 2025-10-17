namespace StudyDataFrame
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.tb_InputFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDbcTest = new System.Windows.Forms.Button();
            this.tb_OutputFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FileProgressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // tb_InputFile
            // 
            this.tb_InputFile.Font = new System.Drawing.Font("굴림", 8F);
            this.tb_InputFile.Location = new System.Drawing.Point(83, 6);
            this.tb_InputFile.Name = "tb_InputFile";
            this.tb_InputFile.Size = new System.Drawing.Size(398, 20);
            this.tb_InputFile.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Input File";
            // 
            // btnDbcTest
            // 
            this.btnDbcTest.Location = new System.Drawing.Point(487, 6);
            this.btnDbcTest.Name = "btnDbcTest";
            this.btnDbcTest.Size = new System.Drawing.Size(75, 77);
            this.btnDbcTest.TabIndex = 3;
            this.btnDbcTest.Text = "Convert";
            this.btnDbcTest.UseVisualStyleBackColor = true;
            this.btnDbcTest.Click += new System.EventHandler(this.btnDbcTest_Click);
            // 
            // tb_OutputFile
            // 
            this.tb_OutputFile.Font = new System.Drawing.Font("굴림", 8F);
            this.tb_OutputFile.Location = new System.Drawing.Point(83, 33);
            this.tb_OutputFile.Name = "tb_OutputFile";
            this.tb_OutputFile.Size = new System.Drawing.Size(398, 20);
            this.tb_OutputFile.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Output File";
            // 
            // FileProgressBar
            // 
            this.FileProgressBar.Location = new System.Drawing.Point(14, 60);
            this.FileProgressBar.Name = "FileProgressBar";
            this.FileProgressBar.Size = new System.Drawing.Size(467, 23);
            this.FileProgressBar.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 90);
            this.Controls.Add(this.FileProgressBar);
            this.Controls.Add(this.btnDbcTest);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_OutputFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_InputFile);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tb_InputFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDbcTest;
        private System.Windows.Forms.TextBox tb_OutputFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar FileProgressBar;
    }
}

