using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialToolBox.Core.Web
{
    /// <summary>
    /// Argument serializer.
    /// </summary>
    public class WebUrl : IEquatable<WebUrl>
    {
        private readonly List<string> _path = new List<string>();
        private readonly Dictionary<string, string> _get = new Dictionary<string, string>();

        /// <summary>
        /// The domain of this URL. 
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// The port of this URL.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Is this HTTPS or plain HTTP ?
        /// </summary>
        public bool IsSecure { get; set; }

        public WebUrl() { }

        public WebUrl(string domain, IEnumerable<string> path, bool secure = false, int? port = null)
        {
            Domain = domain;
            _path.InsertRange(0, path.Where(s => !string.IsNullOrWhiteSpace(s)));
            IsSecure = secure;
            Port = port == null ? (IsSecure ? 443 : 80) : (int) port;
        }

        /// <summary>
        /// The individual elements of the URL.
        /// </summary>
        public IEnumerable<string> Path { get { return _path; } }

        /// <summary>
        /// The additional GET parameters.
        /// </summary>
        public IEnumerable<KeyValuePair<string,string>> Get { get { return _get; } }

        /// <summary>
        /// Append an element to the path suffix. Empty elements are not allowed.
        /// </summary>
        public void AddPathSegment(string added)
        {
            if (string.IsNullOrWhiteSpace(added))
                throw new ArgumentException("Empty or whitespace path segment.", "added");
            
            _path.Add(added);
        }

        /// <summary>
        /// Add a key-value pair to the GET parameters. Duplicate arguments are not 
        /// allowed.
        /// </summary>
        public void AddParameter(string key, string value)
        {
            if (_get.ContainsKey(key))
                throw new ArgumentException(
                    string.Format("Duplicate key '{0}'.", key), "key");
        
            _get.Add(key, value);
        }

        public bool Equals(WebUrl other)
        {
            if (Domain != other.Domain) return false;
            if (Port != other.Port) return false;
            if (IsSecure != other.IsSecure) return false;

            if (_path.Count != other._path.Count) return false;
            if (_path.Where((t, i) => t != other._path[i]).Any()) return false;

            return _get.Count == other._get.Count 
                && other._get.All(kv => _get.ContainsKey(kv.Key) && kv.Value == _get[kv.Key]);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            
            sb.Append(IsSecure ? "https://" : "http://");
            sb.Append(Domain);
            if (Port != (IsSecure ? 443 : 80)) sb.AppendFormat(":{0}", Port);
            
            foreach (var seg in _path)
            {
                sb.Append('/');
                sb.Append(Uri.EscapeDataString(seg));
            }

            var first = true;
            foreach (var kv in _get)
            {
                sb.Append(first ? '?' : '&');
                sb.Append(Uri.EscapeDataString(kv.Key));
                sb.Append('=');
                sb.Append(Uri.EscapeDataString(kv.Value));
                first = false;
            }

            return sb.ToString();
        }
    }
}
