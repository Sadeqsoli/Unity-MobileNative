using UnityEngine;
using System;

namespace SleeplessDev
{
	public static class AndroidSetting
	{
		static readonly string AndroidSettingPre = "android.settings.";
		static readonly string APPLICATION_DETAILS_SETTINGS = AndroidSettingPre + "APPLICATION_DETAILS_SETTINGS";
		static readonly string ACTION_ACCESSIBILITY_SETTINGS = AndroidSettingPre + "ACCESSIBILITY_SETTINGS";
		static readonly string ACTION_DEVICE_INFO_SETTINGS = AndroidSettingPre + "DEVICE_INFO_SETTINGS";
		static readonly string ACTION_SETTINGS = AndroidSettingPre + "SETTINGS";

		#region Call Number

		/// <summary>
		/// Initiates a phone call to the specified number.
		/// </summary>
		/// <param name="phoneNumber">The phone number to call.</param>
		public static void CallNumber(string phoneNumber)
		{
#if UNITY_ANDROID 
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            string actionDial = intentClass.GetStatic<string>("ACTION_DIAL");
            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", actionDial);
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uri = uriClass.CallStatic<AndroidJavaObject>("parse", "tel:" + phoneNumber);
            intent.Call<AndroidJavaObject>("setData", uri);
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call("startActivity", intent);
#else
			Application.OpenURL("tel:" + phoneNumber);
#endif
		}

		#endregion

#if UNITY_ANDROID

		#region Text-to-Speech (TTS) Settings

		/// <summary>
		/// Opens the Text-to-Speech (TTS) settings.
		/// </summary>
		public static void LaunchTTSSettings()
		{
			try
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					AndroidJavaClass buildVersion = new AndroidJavaClass("android.os.Build$VERSION");
					int sdkInt = buildVersion.GetStatic<int>("SDK_INT");

					if (sdkInt >= 14)
					{
						OpenTTSWithAction("com.android.settings.TTS_SETTINGS");
					}
					else
					{
						OpenTTSWithComponent("com.android.settings", "com.android.settings.TextToSpeechSettings");
					}
				}
			}
			catch (Exception e)
			{
				Debug.LogError("Error launching TTS settings: " + e.Message);
			}
		}

		/// <summary>
		/// Opens the TTS service settings for Google TTS.
		/// </summary>
		public static void Launch_TTS_ServiceSettings()
		{
			try
			{
				AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
				intentObject.Call<AndroidJavaObject>("setPackage", "com.google.android.tts");
				intentObject.Call<AndroidJavaObject>("setAction", "android.speech.tts.TextToSpeechService");

				AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

				currentActivity.Call("startActivity", intentObject);
			}
			catch (Exception e)
			{
				Debug.LogError("Error launching TTS service settings: " + e.Message);
			}
		}

		/// <summary>
		/// Opens the TTS engine settings for Google TTS.
		/// </summary>
		public static void Launch_TTS_EngineSettings()
		{
			try
			{
				AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
				intentObject.Call<AndroidJavaObject>("setPackage", "com.google.android.tts");
				intentObject.Call<AndroidJavaObject>("setAction", "android.speech.tts.TextToSpeech.Engine");

				AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

				currentActivity.Call("startActivity", intentObject);
			}
			catch (Exception e)
			{
				Debug.LogError("Error launching TTS engine settings: " + e.Message);
			}
		}

		/// <summary>
		/// Opens the settings to install TTS data for Google TTS.
		/// </summary>
		public static void LaunchINSTALL_TTS_DATASettings()
		{
			try
			{
				AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
				intentObject.Call<AndroidJavaObject>("setPackage", "com.google.android.tts");
				intentObject.Call<AndroidJavaObject>("setAction", "android.speech.tts.engine.INSTALL_TTS_DATA");

				AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

				currentActivity.Call("startActivity", intentObject);
			}
			catch (Exception e)
			{
				Debug.LogError("Error launching INSTALL TTS DATA settings: " + e.Message);
			}
		}

		/// <summary>
		/// Helper method to open TTS settings with a specific action.
		/// </summary>
		/// <param name="action">The action string for the intent.</param>
		static void OpenTTSWithAction(string action)
		{
			try
			{
				AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent");
				intent.Call<AndroidJavaObject>("setAction", action);
				intent.Call<AndroidJavaObject>("setFlags", 0x10000000); // FLAG_ACTIVITY_NEW_TASK
				CallActivity(intent);
			}
			catch (Exception e)
			{
				Debug.LogError("Error opening TTS settings with action: " + action + "\n" + e.Message);
			}
		}

		/// <summary>
		/// Helper method to open TTS settings with a specific component.
		/// </summary>
		/// <param name="packageName">The package name of the settings.</param>
		/// <param name="className">The class name of the settings activity.</param>
		static void OpenTTSWithComponent(string packageName, string className)
		{
			try
			{
				AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent");
				intent.Call<AndroidJavaObject>("addCategory", "android.intent.category.LAUNCHER");
				intent.Call<AndroidJavaObject>("setComponent", new AndroidJavaObject("android.content.ComponentName", packageName, className));
				intent.Call<AndroidJavaObject>("setFlags", 0x10000000); // FLAG_ACTIVITY_NEW_TASK
				CallActivity(intent);
			}
			catch (Exception e)
			{
				Debug.LogError("Error opening TTS settings with component: " + packageName + "/" + className + "\n" + e.Message);
			}
		}

		#endregion

		#region General Settings

		/// <summary>
		/// Opens the specified Android system setting.
		/// </summary>
		/// <param name="targetSetting">The setting to open.</param>
		public static void OpenSettings(ASetting targetSetting)
		{
			using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			using (AndroidJavaObject currentActivityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AndroidJavaObject intentObject = null;
				string packageName = currentActivityObject.Call<string>("getPackageName");
				AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
				AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromParts", "package", packageName, null);
				switch (targetSetting)
				{
					case ASetting.AppDetails:
						intentObject = new AndroidJavaObject("android.content.Intent", APPLICATION_DETAILS_SETTINGS, uriObject);
						break;
					case ASetting.Accessibility:
						intentObject = new AndroidJavaObject("android.content.Intent", ACTION_ACCESSIBILITY_SETTINGS);
						break;
					case ASetting.DeviceInfo:
						intentObject = new AndroidJavaObject("android.content.Intent", ACTION_DEVICE_INFO_SETTINGS);
						break;
					case ASetting.Setting:
						intentObject = new AndroidJavaObject("android.content.Intent", ACTION_SETTINGS);
						break;
					default:
						Debug.LogError("ERROR: Unsupported setting: " + targetSetting);
						return;
				}

				if (intentObject != null)
				{
					intentObject.Call<AndroidJavaObject>("addCategory", "android.intent.category.DEFAULT");
					intentObject.Call<AndroidJavaObject>("setFlags", 0x10000000); // FLAG_ACTIVITY_NEW_TASK
					currentActivityObject.Call("startActivity", intentObject);
				}
			}
		}

		/// <summary>
		/// Launches system settings with a custom intent action.
		/// </summary>
		/// <param name="targetSetting">The intent action string for the setting.</param>
		public static void LaunchSystemSettingsWithIntent(string targetSetting)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				using (var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					var intent = new AndroidJavaObject("android.content.Intent", targetSetting);
					currentActivity.Call("startActivity", intent);
				}
			}
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// Helper method to start an activity with the given intent.
		/// </summary>
		/// <param name="intent">The intent to start the activity.</param>
		static void CallActivity(AndroidJavaObject intent)
		{
			try
			{
				AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
				currentActivity.Call("startActivity", intent);
			}
			catch (Exception e)
			{
				Debug.LogError("Error calling activity: " + e.Message);
			}
		}

		#endregion

#endif
	}
}
