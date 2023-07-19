using Google.Apis.Download;
using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveManipulator
{
	class GoogleDownloader : GoogleHelper
	{
		public FileList response;
		public string[] list;
		public GoogleDownloader(string _token, string _fileName) : base(_token, _fileName)
		{
		}
		public static async Task<GoogleDownloader> Create(string token, string fileName)
		{
			var instance = new GoogleDownloader(token, fileName);
			await instance.Start();
			//instance.driveService
			
			
			var request = instance.driveService.Files.List();
			instance.response = request.Execute();
			//List<string> list = new List<string>();
			//instance.list = "";
			int k = 1;
			string[] templist = new string[instance.response.Files.Count + 1];
			foreach (var file in instance.response.Files)
			{
				templist[k++] = String.Format("{2}) ID:{0}, Name:{1}", file.Id, file.Name, k);
			}
			instance.list = templist;
			return instance;
		}
		public MemoryStream DownloadFile(string FileId)
		{
			var request = driveService.Files.Get(FileId);
			var stream = new MemoryStream();
			request.MediaDownloader.ProgressChanged += progress =>
			{
				switch (progress.Status)
				{
					case DownloadStatus.Downloading:
						{
							Console.WriteLine(progress.BytesDownloaded);
							break;
						}
					case DownloadStatus.Completed:
						{
							Console.WriteLine("Download complete.");
							break;
						}
					case DownloadStatus.Failed:
						{
							Console.WriteLine("Download failed.");
							break;
						}
				}	
			};
			request.Download(stream);
			return stream;
		}


	}
}
