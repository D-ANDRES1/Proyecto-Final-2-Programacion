namespace JutiapaCommunityApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {

            splitContainer = new SplitContainer();
            mapWebView = new Microsoft.Web.WebView2.WinForms.WebView2();
            controlPanel = new Panel();
            resultsTabControl = new TabControl();
            problemsTab = new TabPage();
            problemsListBox = new ListBox();
            investmentsTab = new TabPage();
            investmentsListBox = new ListBox();
            projectsTab = new TabPage();
            projectsListBox = new ListBox();
            aboutButton = new Button();
            loadingIndicator = new ProgressBar();
            analyzeButton = new Button();
            geoJsonTextBox = new TextBox();
            groupBox = new GroupBox();
            businessCheckBox = new CheckBox();
            educationCheckBox = new CheckBox();
            healthCheckBox = new CheckBox();
            infraCheckBox = new CheckBox();
            urbanRadioButton = new RadioButton();
            ruralRadioButton = new RadioButton();
            label1 = new Label();
            populationNumeric = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mapWebView).BeginInit();
            controlPanel.SuspendLayout();
            resultsTabControl.SuspendLayout();
            problemsTab.SuspendLayout();
            investmentsTab.SuspendLayout();
            projectsTab.SuspendLayout();
            groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)populationNumeric).BeginInit();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 0);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(mapWebView);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(controlPanel);
            splitContainer.Size = new Size(1184, 661);
            splitContainer.SplitterDistance = 788;
            splitContainer.TabIndex = 0;
            // 
            // mapWebView
            // 
            mapWebView.AllowExternalDrop = true;
            mapWebView.CreationProperties = null;
            mapWebView.DefaultBackgroundColor = Color.White;
            mapWebView.Dock = DockStyle.Fill;
            mapWebView.Location = new Point(0, 0);
            mapWebView.Name = "mapWebView";
            mapWebView.Size = new Size(788, 661);
            mapWebView.TabIndex = 0;
            mapWebView.ZoomFactor = 1D;
            // 
            // controlPanel
            // 
            controlPanel.Controls.Add(resultsTabControl);
            controlPanel.Controls.Add(aboutButton);
            controlPanel.Controls.Add(loadingIndicator);
            controlPanel.Controls.Add(analyzeButton);
            controlPanel.Controls.Add(geoJsonTextBox);
            controlPanel.Controls.Add(groupBox);
            controlPanel.Dock = DockStyle.Fill;
            controlPanel.Location = new Point(0, 0);
            controlPanel.Name = "controlPanel";
            controlPanel.Size = new Size(392, 661);
            controlPanel.TabIndex = 0;
            // 
            // resultsTabControl
            // 
            resultsTabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            resultsTabControl.Controls.Add(problemsTab);
            resultsTabControl.Controls.Add(investmentsTab);
            resultsTabControl.Controls.Add(projectsTab);
            resultsTabControl.Location = new Point(3, 354);
            resultsTabControl.Name = "resultsTabControl";
            resultsTabControl.SelectedIndex = 0;
            resultsTabControl.Size = new Size(386, 266);
            resultsTabControl.TabIndex = 7;
            // 
            // problemsTab
            // 
            problemsTab.Controls.Add(problemsListBox);
            problemsTab.Location = new Point(4, 24);
            problemsTab.Name = "problemsTab";
            problemsTab.Padding = new Padding(3);
            problemsTab.Size = new Size(378, 238);
            problemsTab.TabIndex = 0;
            problemsTab.Text = "Problemas Comunes";
            problemsTab.UseVisualStyleBackColor = true;
            // 
            // problemsListBox
            // 
            problemsListBox.Dock = DockStyle.Fill;
            problemsListBox.FormattingEnabled = true;
            problemsListBox.ItemHeight = 15;
            problemsListBox.Location = new Point(3, 3);
            problemsListBox.Name = "problemsListBox";
            problemsListBox.Size = new Size(372, 232);
            problemsListBox.TabIndex = 0;
            // 
            // investmentsTab
            // 
            investmentsTab.Controls.Add(investmentsListBox);
            investmentsTab.Location = new Point(4, 24);
            investmentsTab.Name = "investmentsTab";
            investmentsTab.Padding = new Padding(3);
            investmentsTab.Size = new Size(378, 238);
            investmentsTab.TabIndex = 1;
            investmentsTab.Text = "Sugerencias de Inversión";
            investmentsTab.UseVisualStyleBackColor = true;
            // 
            // investmentsListBox
            // 
            investmentsListBox.Dock = DockStyle.Fill;
            investmentsListBox.FormattingEnabled = true;
            investmentsListBox.ItemHeight = 15;
            investmentsListBox.Location = new Point(3, 3);
            investmentsListBox.Name = "investmentsListBox";
            investmentsListBox.Size = new Size(372, 232);
            investmentsListBox.TabIndex = 0;
            // 
            // projectsTab
            // 
            projectsTab.Controls.Add(projectsListBox);
            projectsTab.Location = new Point(4, 24);
            projectsTab.Name = "projectsTab";
            projectsTab.Size = new Size(378, 238);
            projectsTab.TabIndex = 2;
            projectsTab.Text = "Ideas de Proyectos";
            projectsTab.UseVisualStyleBackColor = true;
            // 
            // projectsListBox
            // 
            projectsListBox.Dock = DockStyle.Fill;
            projectsListBox.FormattingEnabled = true;
            projectsListBox.ItemHeight = 15;
            projectsListBox.Location = new Point(0, 0);
            projectsListBox.Name = "projectsListBox";
            projectsListBox.Size = new Size(378, 238);
            projectsListBox.TabIndex = 0;
            // 
            // aboutButton
            // 
            aboutButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            aboutButton.Location = new Point(3, 626);
            aboutButton.Name = "aboutButton";
            aboutButton.Size = new Size(386, 30);
            aboutButton.TabIndex = 6;
            aboutButton.Text = "Acerca de...";
            aboutButton.UseVisualStyleBackColor = true;
            aboutButton.Click += aboutButton_Click;
            // 
            // loadingIndicator
            // 
            loadingIndicator.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            loadingIndicator.Location = new Point(3, 338);
            loadingIndicator.Name = "loadingIndicator";
            loadingIndicator.Size = new Size(386, 10);
            loadingIndicator.Style = ProgressBarStyle.Marquee;
            loadingIndicator.TabIndex = 5;
            loadingIndicator.Visible = false;
            // 
            // analyzeButton
            // 
            analyzeButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            analyzeButton.Location = new Point(3, 302);
            analyzeButton.Name = "analyzeButton";
            analyzeButton.Size = new Size(386, 30);
            analyzeButton.TabIndex = 4;
            analyzeButton.Text = "Analizar Zona";
            analyzeButton.UseVisualStyleBackColor = true;
            analyzeButton.Click += analyzeButton_Click;
            // 
            // geoJsonTextBox
            // 
            geoJsonTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            geoJsonTextBox.Location = new Point(3, 196);
            geoJsonTextBox.Multiline = true;
            geoJsonTextBox.Name = "geoJsonTextBox";
            geoJsonTextBox.ReadOnly = true;
            geoJsonTextBox.ScrollBars = ScrollBars.Vertical;
            geoJsonTextBox.Size = new Size(386, 100);
            geoJsonTextBox.TabIndex = 3;
            // 
            // groupBox
            // 
            groupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox.Controls.Add(businessCheckBox);
            groupBox.Controls.Add(educationCheckBox);
            groupBox.Controls.Add(healthCheckBox);
            groupBox.Controls.Add(infraCheckBox);
            groupBox.Controls.Add(urbanRadioButton);
            groupBox.Controls.Add(ruralRadioButton);
            groupBox.Controls.Add(label1);
            groupBox.Controls.Add(populationNumeric);
            groupBox.Location = new Point(3, 3);
            groupBox.Name = "groupBox";
            groupBox.Size = new Size(386, 187);
            groupBox.TabIndex = 2;
            groupBox.TabStop = false;
            groupBox.Text = "Contexto y Análisis";
            // 
            // businessCheckBox
            // 
            businessCheckBox.AutoSize = true;
            businessCheckBox.Checked = true;
            businessCheckBox.CheckState = CheckState.Checked;
            businessCheckBox.Location = new Point(200, 151);
            businessCheckBox.Name = "businessCheckBox";
            businessCheckBox.Size = new Size(115, 19);
            businessCheckBox.TabIndex = 7;
            businessCheckBox.Text = "Emprendimiento";
            businessCheckBox.UseVisualStyleBackColor = true;
            // 
            // educationCheckBox
            // 
            educationCheckBox.AutoSize = true;
            educationCheckBox.Checked = true;
            educationCheckBox.CheckState = CheckState.Checked;
            educationCheckBox.Location = new Point(200, 126);
            educationCheckBox.Name = "educationCheckBox";
            educationCheckBox.Size = new Size(81, 19);
            educationCheckBox.TabIndex = 6;
            educationCheckBox.Text = "Educación";
            educationCheckBox.UseVisualStyleBackColor = true;
            // 
            // healthCheckBox
            // 
            healthCheckBox.AutoSize = true;
            healthCheckBox.Checked = true;
            healthCheckBox.CheckState = CheckState.Checked;
            healthCheckBox.Location = new Point(200, 101);
            healthCheckBox.Name = "healthCheckBox";
            healthCheckBox.Size = new Size(55, 19);
            healthCheckBox.TabIndex = 5;
            healthCheckBox.Text = "Salud";
            healthCheckBox.UseVisualStyleBackColor = true;
            // 
            // infraCheckBox
            // 
            infraCheckBox.AutoSize = true;
            infraCheckBox.Checked = true;
            infraCheckBox.CheckState = CheckState.Checked;
            infraCheckBox.Location = new Point(200, 76);
            infraCheckBox.Name = "infraCheckBox";
            infraCheckBox.Size = new Size(103, 19);
            infraCheckBox.TabIndex = 4;
            infraCheckBox.Text = "Infraestructura";
            infraCheckBox.UseVisualStyleBackColor = true;
            // 
            // urbanRadioButton
            // 
            urbanRadioButton.AutoSize = true;
            urbanRadioButton.Location = new Point(200, 46);
            urbanRadioButton.Name = "urbanRadioButton";
            urbanRadioButton.Size = new Size(64, 19);
            urbanRadioButton.TabIndex = 3;
            urbanRadioButton.Text = "Urbano";
            urbanRadioButton.UseVisualStyleBackColor = true;
            // 
            // ruralRadioButton
            // 
            ruralRadioButton.AutoSize = true;
            ruralRadioButton.Checked = true;
            ruralRadioButton.Location = new Point(200, 21);
            ruralRadioButton.Name = "ruralRadioButton";
            ruralRadioButton.Size = new Size(52, 19);
            ruralRadioButton.TabIndex = 2;
            ruralRadioButton.TabStop = true;
            ruralRadioButton.Text = "Rural";
            ruralRadioButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 23);
            label1.Name = "label1";
            label1.Size = new Size(129, 15);
            label1.TabIndex = 1;
            label1.Text = "Población aproximada:";
            // 
            // populationNumeric
            // 
            populationNumeric.Location = new Point(6, 41);
            populationNumeric.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            populationNumeric.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            populationNumeric.Name = "populationNumeric";
            populationNumeric.Size = new Size(120, 23);
            populationNumeric.TabIndex = 0;
            populationNumeric.Value = new decimal(new int[] { 5000, 0, 0, 0 });
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 661);
            Controls.Add(splitContainer);
            MinimumSize = new Size(1200, 700);
            Name = "MainForm";
            Text = "Jutiapa Community App";
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mapWebView).EndInit();
            controlPanel.ResumeLayout(false);
            controlPanel.PerformLayout();
            resultsTabControl.ResumeLayout(false);
            problemsTab.ResumeLayout(false);
            investmentsTab.ResumeLayout(false);
            projectsTab.ResumeLayout(false);
            groupBox.ResumeLayout(false);
            groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)populationNumeric).EndInit();
            ResumeLayout(false);

        }

        private void AnalyzeButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private SplitContainer splitContainer;
        private Microsoft.Web.WebView2.WinForms.WebView2 mapWebView;
        private Panel controlPanel;
        private GroupBox groupBox;
        private NumericUpDown populationNumeric;
        private Label label1;
        private RadioButton urbanRadioButton;
        private RadioButton ruralRadioButton;
        private CheckBox businessCheckBox;
        private CheckBox educationCheckBox;
        private CheckBox healthCheckBox;
        private CheckBox infraCheckBox;
        private TextBox geoJsonTextBox;
        private Button analyzeButton;
        private ProgressBar loadingIndicator;
        private Button aboutButton;
        private TabControl resultsTabControl;
        private TabPage problemsTab;
        private ListBox problemsListBox;
        private TabPage investmentsTab;
        private ListBox investmentsListBox;
        private TabPage projectsTab;
        private ListBox projectsListBox;
    }
}