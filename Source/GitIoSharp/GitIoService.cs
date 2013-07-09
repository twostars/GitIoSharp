using System;
using System.Net;
using System.Text;

namespace GitIoSharp
{
	public class GitIoService
	{
        private const int RequestTimeout = 30 * 1000; // 30 seconds

		public Uri Execute(string url)
		{
			var request = HttpWebRequest.Create("http://git.io") as HttpWebRequest;
            request.UserAgent = "GitIoSharp";
			request.ContentType = "application/x-www-form-urlencoded";
			request.Method = "POST";
            request.Timeout = RequestTimeout;
			var bytes = Encoding.ASCII.GetBytes(url);
			request.ContentLength = bytes.Length;
			using (var os = request.GetRequestStream())
			{
				os.Write(bytes, 0, bytes.Length);
			}
			try
			{
				var response = (HttpWebResponse)request.GetResponse();

				if (response.StatusCode != HttpStatusCode.Created)
					throw new GitIoException(response.StatusDescription);

				return new Uri(response.Headers[HttpResponseHeader.Location]);	
			}
			catch (WebException e)
			{
				throw new GitIoException(e.Message);
			}
		}
	}
}