using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace DocumentApprovalSystem.Lib
{
    public static class LibExtensions
    {
        public static bool IsImage(this string Extension)
        {
            if (Extension.ToLower().Contains(".png")
                || Extension.ToLower().Contains(".jpg")
                || Extension.ToLower().Contains(".jpeg")
                || Extension.ToLower().Contains(".svg")
                || Extension.ToLower().Contains(".png"))
                return true;
            else return false;
        }

    }
}
