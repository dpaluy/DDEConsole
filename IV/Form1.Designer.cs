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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.zgCall1 = new ZedGraph.ZedGraphControl();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.labelAsset = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelAssetValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Controls.Add(this.statusStrip);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(769, 316);
            this.panel1.TabIndex = 0;
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
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label1);
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
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelAsset,
            this.labelAssetValue});
            this.statusStrip.Location = new System.Drawing.Point(0, 294);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(769, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // labelAsset
            // 
            this.labelAsset.Name = "labelAsset";
            this.labelAsset.Size = new System.Drawing.Size(34, 17);
            this.labelAsset.Text = "Asset";
            // 
            // labelAssetValue
            // 
            this.labelAssetValue.Name = "labelAssetValue";
            this.labelAssetValue.Size = new System.Drawing.Size(13, 17);
            this.labelAssetValue.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(87, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "100";
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
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.StatusStrip statusStrip;
        private ZedGraph.ZedGraphControl zgCall1;
        private System.Windows.Forms.ToolStripStatusLabel labelAsset;
        private System.Windows.Forms.ToolStripStatusLabel labelAssetValue;
        private System.Windows.Forms.Label label1;
    }
}

