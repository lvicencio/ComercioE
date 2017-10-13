using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ComercioE.Clases
{
    public class FilesHelper
    {
        public static bool UploadPhoto(HttpPostedFileBase file, string folder, string nombre)
        {
            if (file == null || string.IsNullOrEmpty(folder) ||string.IsNullOrEmpty(nombre))
            {
                return false;
            }
            try
            {
                string path = string.Empty;
               
                if (file != null)
                {
                    path = Path.Combine(HttpContext.Current.Server.MapPath(folder), nombre);
                    file.SaveAs(path);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }
                return true;
            }
            catch 
            {

                return false;
            }

          
        }

    }
}