using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public static class save_util {

	// Path settings
	public static string generalPath() {
		return Application.persistentDataPath + "/";
	}

	// Save a data object to the persistent data path at file_name
	public static void save_to_binary<T>(string subpath, string file_name, T data) {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file;

		string path = get_full_path_safe(subpath, file_name);
		if (!File.Exists(path)) {
			file = File.Create(path);
		} else {
			file = File.Open(path, FileMode.Open);
		}

		bf.Serialize(file, data);
		file.Close();
	}

	// Load a data object from the persistent data path at file_name
	public static bool try_load_from_binary<T>(string subpath, string file_name, out T data) {
		object generic_data = null;

		string path = get_full_path_safe(subpath, file_name);
		if (File.Exists(path)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(path, FileMode.Open);

			generic_data = bf.Deserialize(file);
			file.Close();
		} else {
			Debug.LogError("No file: " + path + " found.");
			data = default(T);
			return false;
		}

		if (generic_data is T) {
			data = (T)generic_data;
			return true;
		} else {
			Debug.LogError("Object saved at path: " + path + " is not of the expected type.");
			data = default(T);
			return false;
		}
	}

	// Save an object as a .json to the persistent data path at file_name
	public static void save_to_JSON(string subpath, string file_name, object data) {
		string path = get_full_path_safe(subpath, file_name, ".json");
		string data_json;
		if (data is string) {
			data_json = (string)data;
		} else {
			data_json = JsonUtility.ToJson(data);
		}

		if (!File.Exists(path)) {
			File.Create(path).Dispose();
		}

		File.WriteAllText(path, data_json);
	}

	// Try to load an object from its .json file in the persistent data path
	public static bool try_load_from_JSON<T>(string subpath, string file_name, out T data) {
		string path = get_full_path_safe(subpath, file_name, ".json");
		if (File.Exists(path)) {
			string data_json = File.ReadAllText(path);
			data = JsonUtility.FromJson<T>(data_json);
			return true;
		} else {
			Debug.Log("No json found at: " + path);
			data = default(T);
			return false;
		}
	}

	// Get a list of all the files that exist in the root of a particular directory
	public static List<string> get_files_in_dir(string subpath, bool with_full_paths = false, bool with_extensions = false) {
		string directory_path = get_full_path_safe(subpath, "");
		string[] file_names = Directory.GetFiles(directory_path);
		List<string> files_list = new List<string>(file_names);

		if (!with_full_paths) {
			for (int i=0; i < files_list.Count; i++) {
				string new_name = Path.GetFileName(files_list[i]);
				if (!with_extensions) {
					new_name = remove_extension(new_name);
				}
				files_list[i] = new_name;
			}
		}

		return files_list;
	}

	// Remove the extension (everything after, and including, the last '.') from a string
	public static string remove_extension(string path) {
		int extension_index = path.LastIndexOf(".");
		if (extension_index >= 0) {
			return path.Substring(0, extension_index);
		}
		return path;
	}

	// Check if a certain file exists in the persistent data path
	public static bool file_exists(string subpath, string file_name) {
		string directory_path = Path.Combine(generalPath(), subpath);
		string path = get_full_path_safe(subpath, file_name, "", false);
		return Directory.Exists(directory_path) && File.Exists(path);
	}

	// Generates the full path from persistent data path, subpath, and file_name (which gets sanitized).
	// This will also create the directory path UP TO the file if it doesn't exist yet.
	private static string get_full_path_safe(string subpath, string file_name, string extension = "", bool ensure_subpath = true) {
		if (extension != "" && !file_name.ToLower().EndsWith(extension)) {
			file_name += extension;
		}

		string directory_path = Path.Combine(generalPath(), subpath);
		if (ensure_subpath && !Directory.Exists(directory_path)) {
			Directory.CreateDirectory(directory_path);
		}

		return Path.Combine(directory_path, sanitize_file_name(file_name));
	}

	// Util function to make sure that a file name is valid.
	private static string sanitize_file_name(string name) {
		var invalids = System.IO.Path.GetInvalidFileNameChars();
		return String.Join("_", name.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
	}

}
