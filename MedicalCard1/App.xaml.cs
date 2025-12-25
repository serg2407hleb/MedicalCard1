using System.Windows;

namespace MedicalCard1
{
    public partial class App : Application
    {
        private void InitializeDatabase()
        {
            using (var context = new AppContext())
            {
                context.Database.EnsureCreated();
            }
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeDatabase();
        }
    }
}
