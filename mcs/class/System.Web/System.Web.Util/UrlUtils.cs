/**
 * Namespace: System.Web.UI.Util
 * Class:     UrlUtils
 * 
 * Author:  Gaurav Vaish
 * Maintainer: gvaish@iitk.ac.in
 * Status:  ??%
 * 
 * (C) Gaurav Vaish (2001)
 */

using System;
using System.Collections;
using System.Text;

namespace System.Web.Util
{
	internal class UrlUtils
	{
		/*
		 * I could not find these functions in the class System.Uri
		 * Besides, an instance of Uri will not be formed until and unless the address is of
		 * the form protocol://[user:pass]host[:port]/[fullpath]
		 * ie, a protocol, and that too without any blanks before,
		 * is a must which may not be the case here.
		 * Important: Escaped URL is assumed here. nothing like .aspx?path=/something
		 * It should be .aspx?path=%2Fsomething
		 */
		public static string GetProtocol(string url)
		{
			//Taking code from Java Class java.net.URL
			if(url!=null)
			{
				if(url.Length>0)
				{
					
					int i, start = 0, limit;
					limit = url.Length;
					char c;
					bool aRef = false;
					while( (limit > 0) && (url[limit-1] <= ' '))
					{
						limit --;
					}
					while( (start < limit) && (url[start] <= ' '))
					{
						start++;
					}
					if(RegionMatches(true, url, start, "url:", 0, 4))
					{
						start += 4;
					}
					if(start < url.Length && url[start]=='#')
					{
						aRef = true;
					}
					for(i = start; !aRef && (i < limit) && ((c=url[i]) != '/'); i++)
					{
						if(c==':')
						{
							return url.Substring(start, i - start);
						}
					}
				}
			}
			return String.Empty;
		}
		
		public static bool IsRelativeUrl(string url)
		{
			if (url.IndexOf(':') == -1)
				return !IsRooted(url);

			return false;
		}

		public static bool IsRootUrl(string url)
		{
			if(url!=null)
			{
				if(url.Length>0)
				{
					return IsValidProtocol(GetProtocol(url).ToLower());
				}
			}
			return true;
		}
		
		public static bool IsRooted(string path)
		{
			if(path!=null && path.Length > 0)
			{
				return (path[0]=='/' || path[0]=='\\');
			}
			return true;
		}
		
		public static void FailIfPhysicalPath(string path)
		{
			if(path!= null && path.Length > 1)
			{
				if(path[1]==':' || path.StartsWith(@"\\"))
					throw new HttpException(HttpRuntime.FormatResourceString("Physical_path_not_allowed", path));
			}
		}
		
		public static string Combine (string basePath, string relPath)
		{
			FailIfPhysicalPath (relPath);
			if (IsRooted (relPath))
				return Reduce (relPath);

			if (relPath.Length < 3 || relPath [0] != '~' || relPath [0] == '/' || relPath [0] == '\\') {
				if (basePath == null || (basePath.Length == 1 && basePath [0] == '/'))
					basePath = String.Empty;

				string slash = relPath [0] == '/' ? "" : "/";
				return Reduce (basePath + slash + relPath);
			}

			string vPath = HttpRuntime.AppDomainAppVirtualPath;
			if (vPath.Length <= 1)
				vPath = String.Empty;

			return Reduce (vPath + "/" + relPath.Substring (2));
		}
		
		public static bool IsValidProtocol(string protocol)
		{
			if(protocol.Length < 1)
				return false;
			char c = protocol[0];
			if(!Char.IsLetter(c))
			{
				return false;
			}
			for(int i=1; i < protocol.Length; i++)
			{
				c = protocol[i];
				if(!Char.IsLetterOrDigit(c) && c!='.' && c!='+' && c!='-')
				{
					return false;
				}
			}
			return true;
		}
		
		/*
		 * MakeRelative("http://www.foo.com/bar1/bar2/file","http://www.foo.com/bar1")
		 * will return "bar2/file"
		 * while MakeRelative("http://www.foo.com/bar1/...","http://www.anotherfoo.com")
		 * return 'null' and so does the call
		 * MakeRelative("http://www.foo.com/bar1/bar2","http://www.foo.com/bar")
		 */
		public static string MakeRelative(string fullUrl, string relativeTo)
		{
			if(fullUrl==relativeTo)
			{
				return String.Empty;
			}
			if(fullUrl.IndexOf(relativeTo)!=0)
			{
				return null;
			}
			string leftOver = fullUrl.Substring(relativeTo.Length);
			if(!fullUrl.EndsWith("/") && !leftOver.StartsWith("/"))
			{
				return null;
			}
			if(leftOver.StartsWith("/"))
			{
				leftOver = leftOver.Substring(1);
			}
			return leftOver;
		}
		
		/*
		 * Check JavaDocs for java.lang.String#RegionMatches(bool, int, String, int, int)
		 * Could not find anything similar in the System.String class
		 */
		public static bool RegionMatches(bool ignoreCase, string source, int start, string match, int offset, int len)
		{
			if(source!=null || match!=null)
			{
				if(source.Length>0 && match.Length>0)
				{
					char[] ta = source.ToCharArray();
					char[] pa = match.ToCharArray();
					if((offset < 0) || (start < 0) || (start > (source.Length - len)) || (offset > (match.Length - len)))
					{
						return false;
					}
					while(len-- > 0)
					{
						char c1 = ta[start++];
						char c2 = pa[offset++];
						if(c1==c2)
							continue;
						if(ignoreCase)
						{
							if(Char.ToUpper(c1)==Char.ToUpper(c2))
								continue;
							// Check for Gregorian Calendar where the above may not hold good
							if(Char.ToLower(c1)==Char.ToLower(c2))
								continue;
						}
						return false;
					}
					return true;
				}
			}
			return false;
		}

		public static string Reduce (string path)
		{
			path = path.Replace ('\\','/');

			string [] parts = path.Split ('/');
			ArrayList result = new ArrayList ();
			
			int end = parts.Length;
			for (int i = 0; i < end; i++) {
				string current = parts [i];
				if (current == "" || current == ".")
					continue;

				if (current == "..") {
					if (result.Count == 0)
						throw new HttpException ("Invalid path.");

					result.RemoveAt (result.Count - 1);
					continue;
				}

				result.Add (current);
			}

			result.Insert (0, "");
			return String.Join ("/", (string []) result.ToArray (typeof (string)));
		}
		
		public static string GetDirectory(string url)
		{
			if(url==null)
			{
				return null;
			}
			if(url.Length==0)
			{
				return String.Empty;
			}
			url.Replace('\\','/');

			string baseDir = "";
			int last = url.LastIndexOf ('/');
			if (last > 0)
				baseDir = url.Substring(0, url.LastIndexOf('/'));

			if(baseDir.Length==0)
			{
				baseDir = "/";
			}
			return baseDir;
		}
	}
}
