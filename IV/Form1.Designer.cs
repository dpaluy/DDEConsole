namespace IV
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.zgCall1 = new ZedGraph.ZedGraphControl();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(769, 316);
            this.panel1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 294);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(769, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.zgCall1);
            this.splitContainer1.Size = new System.Drawing.Size(769, 294);
            this.splitContainer1.SplitterDistance = 383;
            this.splitContainer1.TabIndex = 1;
            // 
            // zgCall1
            // 
            this.zgCall1.AutoScroll = true;
            this.zgCall1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zgCall1.Location = new System.Drawing.Point(0, 0);
            this.zgCall1.Name = "zgCall1";
            this.zgCall1.ScrollGrace = 0;
            this.zgCall1.ScrollMaxX = 0;
            this.zgCall1.ScrollMaxY = 0;
            this.zgCall1.ScrollMaxY2 = 0;
            this.zgCall1.ScrollMinX = 0;
            this.zgCall1.ScrollMinY = 0;
            this.zgCall1.ScrollMinY2 = 0;
            this.zgCall1.Size = new System.Drawing.Size(383, 294);
            this.zgCall1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 316);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "IV Graphs";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private ZedGraph.ZedGraphControl zgCall1;
    }
}

