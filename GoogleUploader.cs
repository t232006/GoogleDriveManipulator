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
		public GoogleUploader(string _token, string _fileName, string _filePath, out string _output) : base(_token, _fileName)
		{
			_output = "";
			try
			{
				Start();
				var fileMetadata = new Google.Apis.Drive.v3.Data.File()
				{
					Name = "Report",
					MimeType = "application / octet - stream"
				};
				FilesResource.CreateMediaUpload request;
				using (var stream = new FileStream(_filePath, FileMode.Open))
				{
					request = driveService.Files.Create(fileMetadata, stream, "text/csv");
					request.Fields = "id";
					request.Upload();
				}
				var file = request.ResponseBody;
				_output = file.Id;
			}
			catch (Exception e)
			{
				if (e is AggregateException)
				{
					_output = "Credential not found";
				}
				if (e is FileNotFoundException)
				{
					_output = "File not found";
				}
			}
		}
	}

}
