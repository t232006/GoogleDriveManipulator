using Google.Apis.Drive.v3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveManipulator
{
	class GoogleUploader : GoogleHelper
	{
		public string output;
		GoogleUploader(string _token, string _filePath) : base(_token)
		{
		}
		public static async Task<GoogleUploader> Create(string token, string filePath)
		{
			GoogleUploader instance = new GoogleUploader(token, filePath);
			instance.output = "";
			try
			{
				await instance.Start();
				var fileMetadata = new Google.Apis.Drive.v3.Data.File()
				{
					Name = Path.GetFileName(filePath),
					MimeType = "application / octet - stream"
				};
				FilesResource.CreateMediaUpload request;
				using (var stream = new FileStream(filePath, FileMode.Open))
				{
					request = instance.driveService.Files.Create(fileMetadata, stream, "text/csv");
					request.Fields = "id";
					request.Upload();
				}
				var file = request.ResponseBody;
				instance.output = file.Id;
			}
			catch (Exception e)
			{
				if (e is AggregateException)
				{
					instance.output = "Credential not found";
				}
				if (e is FileNotFoundException)
				{
					instance.output = "File not found";
				}
			}
			return instance;
		}
	}

}
