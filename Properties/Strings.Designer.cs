namespace Windows11Calculator.Properties
{
    using System;
    using System.Resources;
    using System.Globalization;

    public class Strings
    {
        private static ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        public Strings() { }

        public static ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    resourceMan = new ResourceManager("Windows11Calculator.Properties.Strings", typeof(Strings).Assembly);
                }
                return resourceMan;
            }
        }

        public static CultureInfo Culture
        {
            get { return resourceCulture; }
            set { resourceCulture = value; }
        }

      
        public static string ButtonEquals => ResourceManager.GetString("ButtonEquals", resourceCulture);
        public static string ButtonPlus => ResourceManager.GetString("ButtonPlus", resourceCulture);
        public static string ButtonMinus => ResourceManager.GetString("ButtonMinus", resourceCulture);
        public static string ButtonMultiply => ResourceManager.GetString("ButtonMultiply", resourceCulture);
        public static string ButtonDivide => ResourceManager.GetString("ButtonDivide", resourceCulture);
        public static string DisplayZero => ResourceManager.GetString("DisplayZero", resourceCulture);
        public static string DisplayError => ResourceManager.GetString("DisplayError", resourceCulture);
   
    }
}
