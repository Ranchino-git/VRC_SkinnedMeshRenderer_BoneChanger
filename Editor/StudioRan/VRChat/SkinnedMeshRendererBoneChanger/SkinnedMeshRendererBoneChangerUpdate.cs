using System;    
using System.Threading;  
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Net;

namespace StudioRan
{
	namespace VRChat
	{
		public class SkinnedMeshRendererBoneChangerUpdate
		{
			Thread thread;
			internal void UpdateTool()
			{
				thread = new Thread(UpdateToolIn);
				thread.Start();
			}

			private void UpdateToolIn()
			{
				File.Delete(".\\Assets\\Script\\Editor\\StudioRan\\VRChat\\SkinnedMeshRendererBoneChanger\\SkinnedMeshRendererBoneChanger.cs");
				File.Delete(".\\Assets\\Script\\Editor\\StudioRan\\VRChat\\SkinnedMeshRendererBoneChanger\\VersionData");
				string Tool = new WebClient().DownloadString("https://raw.githubusercontent.com/Ranchino-git/VRC_SkinnedMeshRenderer_BoneChanger/main/Editor/StudioRan/VRChat/SkinnedMeshRendererBoneChanger/SkinnedMeshRendererBoneChanger.cs");
				string WebVersionData = new WebClient().DownloadString("https://raw.githubusercontent.com/Ranchino-git/VRC_SkinnedMeshRenderer_BoneChanger/main/Editor/StudioRan/VRChat/SkinnedMeshRendererBoneChanger/VersionData");
				File.WriteAllText(".\\Assets\\Script\\Editor\\StudioRan\\VRChat\\SkinnedMeshRendererBoneChanger\\SkinnedMeshRendererBoneChanger.cs", Tool);
				File.WriteAllText(".\\Assets\\Script\\Editor\\StudioRan\\VRChat\\SkinnedMeshRendererBoneChanger\\VersionData", WebVersionData);
			}
		}
	}
}