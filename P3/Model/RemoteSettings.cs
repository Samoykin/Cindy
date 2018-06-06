namespace P3.Model
{
    using System;
    using System.Xml.Serialization;

    /// <summary>Оболочка.</summary>
    public class ShellRemoteSettings
    {
        /// <summary>Корневой элемент.</summary>
        [Serializable]
        [XmlRootAttribute("Settings")]
        public class RootElementRemoteSettings
        {
            /// <summary>Информационный справочник.</summary>
            public Phonebook Phonebook { get; set; }

            /// <summary>Программа обновления.</summary>
            public PhonebookUpd PhonebookUpd { get; set; }
        }

        /// <summary>Параметры информационного справочника.</summary>
        [Serializable]
        public class Phonebook
        {
            /// <summary>Версия.</summary>
            [XmlAttribute]
            public string Version { get; set; }

            /// <summary>Путь к файлам.</summary>
            [XmlAttribute]
            public string Path { get; set; }
        }

        /// <summary>Параметры программы обновления.</summary>
        [Serializable]
        public class PhonebookUpd
        {
            /// <summary>Версия.</summary>
            [XmlAttribute]
            public string Version { get; set; }

            /// <summary>Путь к файлам.</summary>
            [XmlAttribute]
            public string Path { get; set; }
        }
    }
}