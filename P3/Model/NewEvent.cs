namespace P3.Model
{
    using System;

    /// <summary>Новый.</summary>
    public class NewEvent
    {
        /// <summary>Дата.</summary>
        public DateTime Date { get; set; }

        /// <summary>Полное имя.</summary>
        public string FullName { get; set; }

        /// <summary>Количество лет.</summary>
        public int YearCount { get; set; }

        /// <summary>Префикс.</summary>
        public string Prefix { get; set; }

        /// <summary>Постфикс.</summary>
        public string Postfix { get; set; }        
    }
}