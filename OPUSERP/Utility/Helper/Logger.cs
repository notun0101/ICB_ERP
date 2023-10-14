using System.IO;
using System;
using System.Text;

namespace OPUSERP.Utility.Helper
{
	public static class Logger
	{
		public static void Write(string message, string interfaceName, string path)
		{
			string filePath = path; //@"C:\Error.txt";

			using (StreamWriter writer = new StreamWriter(filePath, true))
			{
				writer.WriteLine("-----------------------------------------------------------------------------");
				writer.WriteLine("Date : " + DateTime.Now.ToString());
				writer.WriteLine();
				writer.WriteLine(message);


				//while (ex != null)
				//{
				//	writer.WriteLine(ex.GetType().FullName);
				//	writer.WriteLine("Message : " + ex.Message);
				//	writer.WriteLine("StackTrace : " + ex.StackTrace);
				//	ex = ex.InnerException;
				//}
			}
		}

		public static void Write(string UIName, string info, Exception ex, string path)
		{
			try
			{
				using (StreamWriter writer = new StreamWriter(path, true)) //System.IO.File.AppendText(path))
				{
					writer.WriteLineAsync();
					writer.WriteLineAsync();
					writer.WriteLineAsync("Date : " + DateTime.Now.ToString() + " --------------------" + UIName + " --------------------");
					writer.WriteLineAsync(info);

					if (ex != null)
					{
						writer.WriteLineAsync("FullName: " + ex.GetType().FullName);
						writer.WriteLineAsync("Message: " + ex.Message);
						//writer.WriteLine("StackTrace : " + ex.StackTrace);
					}
					writer.FlushAsync();
				}
			}
			catch 
			{
			}
		}
	}
}
