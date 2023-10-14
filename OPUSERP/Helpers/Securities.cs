using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OPUSERP.Helpers
{
	public class Securities
	{
		private const string SecurityKey = "ThisIsAComplexKeyForEncryptionByMH";


		public static string GetMacAddress()
		{
			NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
			String sMacAddress = string.Empty;
			foreach (NetworkInterface adapter in nics)
			{
				if (sMacAddress == String.Empty)// only return MAC Address from first card  
				{
					IPInterfaceProperties properties = adapter.GetIPProperties();
					sMacAddress = adapter.GetPhysicalAddress().ToString();
				}
			}

			return sMacAddress;
		}

		public static string GetPCName()
		{
			return Dns.GetHostName();
		}


		public static string EncryptString(string str)
		{
			byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(str);
			MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();
			byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
			objMD5CryptoService.Clear();
			var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
			objTripleDESCryptoService.Key = securityKeyArray;
			objTripleDESCryptoService.Mode = CipherMode.ECB;
			objTripleDESCryptoService.Padding = PaddingMode.PKCS7;
			var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();
			byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);
			objTripleDESCryptoService.Clear();
			return Convert.ToBase64String(resultArray, 0, resultArray.Length);
		}

		public static string DecryptString(string str)
		{
			byte[] toEncryptArray = Convert.FromBase64String(str);
			MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();
			byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
			objMD5CryptoService.Clear();
			var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
			objTripleDESCryptoService.Key = securityKeyArray;
			objTripleDESCryptoService.Mode = CipherMode.ECB;
			objTripleDESCryptoService.Padding = PaddingMode.PKCS7;
			var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();
			byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
			objTripleDESCryptoService.Clear();
			return UTF8Encoding.UTF8.GetString(resultArray);
		}

		public static List<string> GetIPAddress()
		{
			List<string> ips = new List<string>();
			IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

			if (localIPs.Length >= 2)
			{
				ips.Add(localIPs[localIPs.Length - 1].ToString());
				ips.Add(localIPs[localIPs.Length - 2].ToString());
			}

			return ips;
		}

		
	}
}
