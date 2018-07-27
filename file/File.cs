namespace navdi2.file {

	using System.Collections;
	using UnityEngine;
	using System.IO;

	public static class File {

		
		public static string PathToStreamingAssetsPath(string path) {
			return Path.Combine(Application.streamingAssetsPath, path);
		}
		public static string CombinedPath(params string[] names) {
			if (names.Length==0) return "";
			string combinedName = names[0];
			for (int i = 1; i < names.Length; i++) {
				combinedName = Path.Combine(combinedName,names[i]);
			}
			return combinedName;
		}
		public static void CreateFolderIfDoesNotExist(string folderPath) {
			if (!DoesFolderExist(folderPath)) {
				System.IO.Directory.CreateDirectory(folderPath);
			}
		}
		public static bool DoesFolderExist(string folderPath) {
			return System.IO.Directory.Exists(folderPath);
		}
		public static bool DoesFileExist(string filePath) {
			return System.IO.File.Exists(filePath);
		}
		public static IEnumerator AsyncReadFile(string filePath, System.Action<string> RcvResult) 
		{
			if (filePath.Contains ("://") || filePath.Contains (":///")) {
				WWW www = new WWW (filePath);
				yield return www;
				RcvResult(www.text);
			} else {
				RcvResult(System.IO.File.ReadAllText (filePath));
			}
		}
		public static string ReadFile(string filePath) 
		{
			if (filePath.Contains ("://") || filePath.Contains (":///")) {
				WWW www = new WWW (filePath);
				return www.text;
			} else {
				return System.IO.File.ReadAllText (filePath);
			}
		}
		public static void WriteFile(string filePath, string contents) {
			System.IO.File.WriteAllText (filePath, contents);
		}

	}
}