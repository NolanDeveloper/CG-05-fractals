namespace BezierCurves
{
    partial class Form1
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
            this.bezierPlotter1 = new BezierCurves.BezierPlotter();
            this.SuspendLayout();
            // 
            // bezierPlotter1
            // 
            this.bezierPlotter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bezierPlotter1.Location = new System.Drawing.Point(0, 0);
            this.bezierPlotter1.Name = "bezierPlotter1";
            this.bezierPlotter1.Size = new System.Drawing.Size(928, 586);
            this.bezierPlotter1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(928, 586);
            this.Controls.Add(this.bezierPlotter1);
            this.Name = "Form1";
            this.Text = "Bezier splines";
            this.ResumeLayout(false);

        }

        #endregion

        private BezierPlotter bezierPlotter1;
    }
}

