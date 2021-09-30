using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using SoulsFormats;

namespace ParamDefXmlSerializerDeserializer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any())
            {
                Console.WriteLine("No files were drag and drop on the program.");
                Console.WriteLine("Drag and drop paramdef files to convert them to paramdef.xml files");
                Console.WriteLine("Drag and drop paramdef.xml files to convert them to .paramdef files");
                Console.WriteLine("Press ENTER to continue...");
                Console.ReadLine();
                return;
            }
            foreach (var path in args)
            {
                string pathExtension = Path.GetExtension(path);
                string pathFileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
                if (pathExtension == ".paramdef")
                {
                    var paramDef = PARAMDEF.Read(path);
                    XDocument XDoc = new XDocument();

                    using (var xmlWriter = XDoc.CreateWriter())
                    {
                        XmlSerializer paramDefSerializer = new XmlSerializer(typeof(PARAMDEF));
                        paramDefSerializer.Serialize(xmlWriter, paramDef);
                    }

                    string newXmlPath = path + ".xml";
                    Console.WriteLine(newXmlPath);
                    XDoc.Save(newXmlPath);
                    Console.WriteLine($"{pathFileNameWithoutExtension}.paramdef converted to {pathFileNameWithoutExtension}.paramdef.xml at: {newXmlPath}");
                }
                else if (pathExtension == ".xml")
                {
                    XDocument paramDefXml = XDocument.Load(path);
                    XmlSerializer paramDefDeserializer = new XmlSerializer(typeof(PARAMDEF));
                    using (XmlReader xmlReader = paramDefXml.CreateReader())
                    {
                        PARAMDEF paramDefObject = (PARAMDEF)paramDefDeserializer.Deserialize(xmlReader);
                        string newParamDefPath = path.Substring(0, path.Length - 4);
                        paramDefObject.Write(newParamDefPath);
                        Console.WriteLine($"{pathFileNameWithoutExtension}.paramdef.xml converted to {pathFileNameWithoutExtension}.paramdef at: {newParamDefPath}");
                    }
                }
            }
        }
    }
}
