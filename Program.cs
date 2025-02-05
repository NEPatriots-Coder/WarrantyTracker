using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using WarrantyTracker.Forms;
using WarrantyTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace WarrantyTracker
{
    public static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        [STAThread]
        public static void Main()
        {
            // Attach to console
            AttachConsole(ATTACH_PARENT_PROCESS);
            Console.WriteLine("Application starting...");

            ApplicationConfiguration.Initialize();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Test database connection on startup
            using (var context = new WarrantyContext())
            {
                try
                {
                    var canConnect = context.Database.CanConnect();
                    Console.WriteLine($"Database connection test: {(canConnect ? "SUCCESS" : "FAILED")}");
                    
                    if (canConnect)
                    {
                        context.Database.EnsureCreated();
                        Console.WriteLine("Database and tables created if they didn't exist");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database initialization error: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                }
            }

            Application.Run(new MainForm());
        }
    }
}