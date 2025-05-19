namespace ArcaeaPatcher
{
    internal static class Program
    {
        internal static string AssetsPath = Directory.GetParent(Application.ExecutablePath)?.FullName + "\\Assets";

        internal static string CoreFile = AssetsPath + "\\CoreFile.ipa";

        internal static void MsgInfo(string msg) => MessageBox.Show(msg, "ÏûÏ¢", MessageBoxButtons.OK, MessageBoxIcon.Information);

        internal static void MsgError(string msg) => MessageBox.Show(msg, "´íÎó", MessageBoxButtons.OK, MessageBoxIcon.Error);

        [STAThread]
        static void Main()
        {
            if (!Directory.Exists(AssetsPath))
            {
                Directory.CreateDirectory(AssetsPath);
            }
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}