using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operating_Systems_Project
{
    internal partial class Settings
    {
        // Use a shortened version of app version format. "1.0" instead of "1.0.0", which stand for "MAJOR.MINOR.PATCH"
        private static string _APP_VERSION = Operating_Systems.APP_VERSION;

        private static readonly (string Name, string Email)[] DEVELOPERS =
        {
            ("Belal Amr Mohamed", "belalamrofficial@gmail.com"),
            ("Mohamed Ahmed Mohamed Tawfeeq", "m.a.tawfeq@gmail.com"),
            ("Ahmed Mohamed Husaini", "ahmedmohammedhussieny@gmail.com"),
            ("Mahmoud Gad Alkareem", "mahmooodgad245@gmail.com"),
            ("Ahmed Khairy Ahmed", "akhairy975@gmail.com"),
            ("Abdulra'of Mohamed Abdulra'of", "AbdulraofMohamed@gmail.com"),
        };
    }
}