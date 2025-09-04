using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Windows11Calculator
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Optional CLI language override: en, ru, tk (or tk-TM)
            if (e.Args.Length > 0)
            {
                var arg = e.Args[0].ToLowerInvariant();
                var culture = arg switch
                {
                    "ru" => "ru-RU",
                    "tk" => "tk-TM",
                    "tk-tm" => "tk-TM",
                    "en" => "en-US",
                    _ => CultureInfo.CurrentUICulture.Name
                };
                SetCulture(culture);
            }
        }

        public static void SetCulture(string cultureName)
        {
            var ci = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            CultureInfo.DefaultThreadCurrentCulture = ci;
            CultureInfo.DefaultThreadCurrentUICulture = ci;
        }
    }
}