using System;
using System.Collections.Generic;

namespace GitIoSharp
{
	public class GitIo
	{
		private readonly GitIoService _gitIoService;
        private readonly Dictionary<string, Uri> _urlCache;

		public GitIo()
		{
            _gitIoService = new GitIoService();
            _urlCache = new Dictionary<string, Uri>();
		}

        private Uri AttemptLookup(string url)
        {
            var key = url;
            if (_urlCache.ContainsKey(key))
                return _urlCache[key];

            try
            {
                var shortUri = _gitIoService.Execute(url);
                _urlCache.Add(key, shortUri);
                return shortUri;
            }
            catch (Exception)
            {
                throw;
            }
        }

		public Uri Shorten(Uri url)
		{
            return AttemptLookup(String.Format("url={0}", url));
		}

		public Uri Shorten(Uri url, string code)
		{
            return AttemptLookup(String.Format("url={0}&code={1}", url, code));
		}
	}
}