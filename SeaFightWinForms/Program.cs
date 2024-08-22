using SeaFightWinForms.Media;
using SeaFightWinForms.Presenter;
using System.Configuration;

namespace SeaFightWinForms
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var habUrl = ConfigurationManager.AppSettings["hub_url"];
            var startForm = new StartForm(new MediaPlayerSound(), new Animation());
            new StartPresenter(startForm, habUrl);
            Application.Run(startForm);
        }
    }
}