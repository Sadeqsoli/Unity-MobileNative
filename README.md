# Unity-Mobile Native Plugin

## Overview

The Unity Native Settings Plugin is a comprehensive library designed to bridge the gap between Unity and native Android/iOS settings. This plugin allows Unity developers to easily access and manipulate various system settings that are not natively exposed through Unity's C# API. By leveraging native code (Java for Android and Swift/Objective-C for iOS), the plugin provides a seamless interface to handle system settings directly from Unity scripts.

## Features

- **Cross-Platform Support**: Handle both Android and iOS settings from a single codebase.
- **Extensive Settings Coverage**: Access a wide range of system settings including Wi-Fi, Bluetooth, accessibility settings, device information, and more.
- **Easy Integration**: Simple and straightforward integration with Unity projects.
- **Robust Error Handling**: Built-in error handling and logging for reliable operations.

## Supported Settings

- **Android**:
  - Application Details
  - Accessibility
  - Device Info
  - General Settings
  - Text-to-Speech (TTS) Settings
  - Custom Intent Actions

- **iOS**:
  - General Settings
  - Application Details
  - Custom URL Schemes for specific settings

## Installation

1. **Clone or Download the Repository**:
   ```sh
   git clone https://github.com/Sadeqsoli/Unity-MobileNative.git
   
2. **Import into Unity:

Open your Unity project.
Drag the Plugins folder from the cloned repository into your Assets folder.

3. **Configure Android and iOS Projects:

Follow the setup instructions for both Android and iOS to ensure your project is correctly configured to use the native plugins.
# Usage
## Android
To open Android settings, use the `AndroidSettingsBridge` class. For example, to open Wi-Fi settings:
using UnityEngine;

```csharp
public class SettingsExample : MonoBehaviour {
    void Start() {
        if (Application.platform == RuntimePlatform.Android) {
            AndroidSettingsBridge.OpenSettings("android.settings.WIFI_SETTINGS");
        }
    }
}

```
## iOS
To open iOS settings, use the IOSSettingsBridge class. For example, to open Wi-Fi settings:

```csharp
using UnityEngine;

public class SettingsExample : MonoBehaviour {
    void Start() {
        if (Application.platform == RuntimePlatform.IPhonePlayer) {
            IOSSettingsBridge.OpenSettings("App-Prefs:root=WIFI");
        }
    }
}
```
## Documentation
Detailed documentation is available in the docs folder, including setup guides, API references, and example use cases.

## Contributions
Contributions are welcome! Please read the CONTRIBUTING.md file for guidelines on how to contribute to the project.

## License
This project is licensed under the MIT License. See the LICENSE file for more details.
