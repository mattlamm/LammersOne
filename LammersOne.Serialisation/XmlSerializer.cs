using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace MattLamm.Common.Xml
{
    public class XmlSerializer
    {
        //-------------------------------------------------------------------------------------------------------------------------

        #region Public

        public static T CloneObject<T>(T sourceObject)
        {
            var serializedObject = XmlSerializer.Serialize(sourceObject);
            return XmlSerializer.Deserialize<T>(serializedObject, sourceObject.GetType());
        }

        public static T Deserialize<T>(Type type, string filePath)
        {
            if (!File.Exists(filePath))
            {
                return default(T);
            }

            var cereal = new System.Xml.Serialization.XmlSerializer(type);
            object deserializedObject = null;

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                deserializedObject = cereal.Deserialize(fileStream);
                cereal = null;
            }

            if (deserializedObject == null)
            {
                throw new InvalidDataException("The file is invalid or corrupted!");
            }

            return (T)deserializedObject;
        }

        public static T Deserialize<T>(string data, Type type)
        {
            if (data.Trim() == String.Empty)
            { return default(T); }

            var cereal = new System.Xml.Serialization.XmlSerializer(type);
            object deserializedObject = null;

            using (var reader = new StringReader(data))
            {
                deserializedObject = cereal.Deserialize(reader);
                cereal = null;
            }

            if (deserializedObject == null)
            {
                throw new InvalidDataException("The data is invalid or corrupted!");
            }

            return (T)deserializedObject;
        }

        public static void Serialize(Type type, string filePath, object serializableObject)
        {
            var cereal = new System.Xml.Serialization.XmlSerializer(type);
            var xmlNameSpaces = new XmlSerializerNamespaces();
            xmlNameSpaces.Add("", "");

            var xmlWriteSettings = new XmlWriterSettings();
            xmlWriteSettings.OmitXmlDeclaration = true;
            xmlWriteSettings.Indent = true;

            var oFile = new FileInfo(filePath);
            if (!oFile.Directory.Exists)
            {
                Directory.CreateDirectory(oFile.Directory.FullName);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                using (var xmlWriter = XmlWriter.Create(fileStream, xmlWriteSettings))
                {
                    cereal.Serialize(xmlWriter, serializableObject, xmlNameSpaces);
                    cereal = null;
                }
            }
        }

        public static string Serialize(object obj)
        {
            var cereal = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            var xmlNameSpaces = new XmlSerializerNamespaces();
            xmlNameSpaces.Add("", "");

            var xmlWriteSettings = new XmlWriterSettings();
            xmlWriteSettings.OmitXmlDeclaration = true;
            xmlWriteSettings.Indent = true;

            using (var writer = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(writer, xmlWriteSettings))
                {
                    cereal.Serialize(xmlWriter, obj, xmlNameSpaces);
                }

                return writer.ToString();
            }
        }

        #endregion

        //-------------------------------------------------------------------------------------------------------------------------
    }
}
