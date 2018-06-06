namespace P3.Model
{
    using System.Collections.Generic;

    /// <summary>Новостной список.</summary>
    public class NewsList
    {
        /// <summary>Новости.</summary>
        public List<NewEvent> News { get; set; }

        /// <summary>Будущие новости.</summary>
        public List<NewEvent> FutureNews { get; set; }            
    }
}