using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveManipulator
{
	class Program
	{
		static readonly string token = @"h:\программки\GoogleDriveManipulator\client_secret.json";
		static readonly string filename = "dictionaryCopy.db";
		static void Main(string[] args)
		{
			void Menu()
			{
				Console.WriteLine("1 - Upload file");
				Console.WriteLine("2 - Download file");
				Console.WriteLine("0 - Exit");
			}
			void ShowMenu()
			{
				ConsoleKeyInfo s;
				do
				{
					Menu();
					s = Console.ReadKey();
					Console.Clear();

				} while (s.KeyChar  == '0');
				switch (s.KeyChar)
				{
					case '1':
						{
							Console.WriteLine("Input file path");
							string path = Path.Combine(Console.ReadLine(), filename);
							string output;
							GoogleUploader gu = new GoogleUploader(token, filename, path, out output);
							break;
						}
					case '2':
						{
							int num;
							var gd = GoogleDownloader.Create(token, filename).Result;
							foreach (string li in gd.list)
								Console.WriteLine(li);
							Console.Write("Select number");
							num = Convert.ToInt32(Console.ReadLine());
							gd.DownloadFile(gd.response.Files[num].Id);
							break;
						}
				}
				
			}
			ShowMenu();
		}
	}
}
