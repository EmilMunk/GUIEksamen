using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace PhotoRename
{
    public class Repository
    {
        internal static void SaveFile(string fileName, List<BitmapImage> BitmapImages)
        {
            // Create an instance of the XmlSerializer class and specify the type of object to serialize.
            XmlSerializer serializer = new XmlSerializer(typeof(List<BitmapImage>));
            TextWriter writer = new StreamWriter(fileName);
            // Serialize all the BitmapImages.
            serializer.Serialize(writer, BitmapImages);
            writer.Close();
        }
    }
}
