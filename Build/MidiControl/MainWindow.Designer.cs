namespace MidiControl
{
    partial class MainWindow
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.hostDeviceControl = new System.Windows.Forms.Integration.ElementHost();
            this.deviceControl = new MidiControl.DeviceControl();
            this.SuspendLayout();
            // 
            // hostDeviceControl
            // 
            this.hostDeviceControl.Location = new System.Drawing.Point(0, 0);
            this.hostDeviceControl.Name = "hostDeviceControl";
            this.hostDeviceControl.Size = new System.Drawing.Size(1400, 700);
            this.hostDeviceControl.TabIndex = 0;
            this.hostDeviceControl.Text = "hostDeviceControl";
            this.hostDeviceControl.Child = this.deviceControl;
            // 
            // MainWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1400, 700);
            this.Controls.Add(this.hostDeviceControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "DL4 MkII MIDI";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost hostDeviceControl;
        private DeviceControl deviceControl;
    }
}

