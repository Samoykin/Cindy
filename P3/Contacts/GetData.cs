namespace P3.Contacts
{
    using System.Collections.Generic;

    /// <summary>Получить данные.</summary>
    public class GetData
    {
        /*
        public List<string> name = new List<string>();
        public List<string> position = new List<string>();
        public List<string> tel = new List<string>();
        public List<string> workTel = new List<string>();
        public List<string> email = new List<string>();
        public List<string> company = new List<string>();
        */

        /// <summary>Имя.</summary>
        public List<string> Name { get; set; }

        /// <summary>Должность.</summary>
        public List<string> Position { get; set; }

        /// <summary>Телефон.</summary>
        public List<string> Tel { get; set; }

        /// <summary>Рабочий телефон.</summary>
        public List<string> WorkTel { get; set; }

        /// <summary>Email.</summary>
        public List<string> Email { get; set; }

        /// <summary>Компания.</summary>
        public List<string> Company { get; set; }
    }
}