namespace P3.Model
{
    using System;
    using System.Xml.Serialization;

    /// <summary>Оболочка.</summary>
    public class ShellUpdaterSettings
    {
        /// <summary>Корневой элемент.</summary>
        [Serializable]
        [XmlRootAttribute("Settings")]
        public class RootElementUpdaterSettings
        {
            /// <summary>Программа обновления.</summary>
            public Updater Updater { get; set; }            
        }

        /// <summary>Параметры программы обновления.</summary>
        [Serializable]
        public class Updater
        {
            /// <summary>Версия.</summary>
            [XmlAttribute]
            public string Version { get; set; }            
        }        
    }
}