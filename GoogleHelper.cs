using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleDriveManipulator
{
	class GoogleHelper
	{
		private readonly string token;
		private readonly string filename;
		private DriveService driveService;
		private UserCredential credentials;
		private string FileId;

		public GoogleHelper(string _token, string _fileName)
		{
			this.token = _token;
			this.filename = _fileName;
		}

		public string ApplicationName { get; private set; } = "GoogleNotepad";
		public string[] Scopes { get; private set; } = new string[] { DriveService.Scope.Drive };

		internal async Task<String> Start()
		{
			string credentialPath = Path.Combine(Environment.CurrentDirectory, ".credentials", ApplicationName);
			using (var stream = new FileStream(token, FileMode.Open, FileAccess.Read))
			{
				this.credentials = await GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.FromStream(stream).Secrets,
					//new[] {DriveService.ScopeConstants.DriveReadonly},
					Scopes,
					user: "user",
					taskCancellationToken: CancellationToken.None,
					new FileDataStore(credentialPath, true));
			}
			this.driveService = new DriveService(new Google.Apis.Services.BaseClientService.Initializer
			{
				HttpClientInitializer = this.credentials,
				ApplicationName = ApplicationName
			});

			var request = this.driveService.Files.List();
			var response = request.Execute();
			//List<string> list = new List<string>();
			string list = "";
			foreach (var file in response.Files)
			{
				list += String.Format("ID:{0}, Name:{1} \n", file.Id, file.Name);
				if (file.Name == filename)
				{
					FileId = file.Id;
					break;
				}

			}
			//var textFile = driveService.Files.Get(FileId).Execute();
			//return textFile;
			return list;
			/*
			if (!string.IsNullOrEmpty(this.FileId))
			{
				return true;
			}
			return false;*/

		}
	}
}
