namespace Processa.Host
{
  partial class WebHostInstaller
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

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.Installer = new System.ServiceProcess.ServiceInstaller();
      this.ProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
      // 
      // Installer
      // 
      this.Installer.Description = "Serviço de hospedagem de aplicativos WCF.";
      this.Installer.DisplayName = "Processa - Host (Serviço de Hospedagem de Aplicativos WCF)";
      this.Installer.ServiceName = "Processa.Host";
      this.Installer.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
      // 
      // ProcessInstaller
      // 
      this.ProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
      this.ProcessInstaller.Password = null;
      this.ProcessInstaller.Username = null;
      // 
      // WebHostInstaller
      // 
      this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.Installer,
            this.ProcessInstaller});

    }

    #endregion

    public System.ServiceProcess.ServiceInstaller Installer;
    public System.ServiceProcess.ServiceProcessInstaller ProcessInstaller;
  }
}