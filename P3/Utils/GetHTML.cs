namespace P3.Utils
{
    using System.Net;
    using System.Text;

    /// <summary>Получение HTML.</summary>
    public class GetHTML
    {
        /// <summary>HTML.</summary>
        /// <param name="address">Адрес.</param>
        /// <returns>Строка.</returns>
        public string Html(string address)
        {
            var sb = new StringBuilder();
            var buf = new byte[8192];
            var request = (HttpWebRequest)WebRequest.Create(address);
            request.Credentials = CredentialCache.DefaultCredentials;
            var response = (HttpWebResponse)request.GetResponse();

            var resStream = response.GetResponseStream();
            var count = 0;

            do
            {
                if (resStream != null)
                {
                    count = resStream.Read(buf, 0, buf.Length);
                }

                if (count != 0)
                {
                    sb.Append(Encoding.UTF8.GetString(buf, 0, count));
                }
            }
            while (count > 0);

            return sb.ToString();
        }
    }
}