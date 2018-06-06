namespace P3.Model
{
    using System;
    using System.Xml.Serialization;

    /// <summary>Оболочка.</summary>
    public class Shell
    {
        /// <summary>Корневой элемент.</summary>
        [Serializable]
        [XmlRootAttribute("Settings")]
        public class RootElement
        {
            /// <summary>Обновление.</summary>
            public SoftUpdate SoftUpdate { get; set; }

            /// <summary>Контакты.</summary>
            public Contacts Contacts { get; set; }
        }

        /// <summary>Параметры обновления.</summary>
        [Serializable]
        public class SoftUpdate
        {
            /// <summary>Путь к настройкам обновления.</summary>
            [XmlAttribute]
            public string UpdPath { get; set; }
        }

        /// <summary>Параметры контактов.</summary>
        [Serializable]
        public class Contacts
        {
            /// <summary>Путь к файлу контактов.</summary>
            [XmlAttribute]
            public string FilePath { get; set; }            
        }
    }
}