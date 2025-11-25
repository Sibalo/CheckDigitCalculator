using CheckDigitCalculator.App.Services.Implementations;
using CheckDigitCalculator.App.Services.Interfaces;
using CheckDigitCalculator.Application.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CheckDigitCalculator
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            var services = new ServiceCollection();
            services.AddSingleton<InputNavigationService>();
            services.AddSingleton<ICheckDigitService, CheckDigitService>();
            services.AddTransient<Form1>();
            var serviceProvider = services.BuildServiceProvider();
            var mainForm = serviceProvider.GetRequiredService<Form1>();
            System.Windows.Forms.Application.Run(mainForm);
        }
    }
}
