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
				File.Delete(".\\Assets\\Script\\Editor\\SkinnedMeshRendererBoneChanger.cs");
				File.Delete(".\\Assets\\Script\\Editor\\SkinnedMeshRendererBoneChanger\\VersionData");
				string Tool = new WebClient().DownloadString("https://raw.githubusercontent.com/Ranchino-git/VRC_SkinnedMeshRenderer_BoneChanger/main/Editor/SkinnedMeshRendererBoneChanger.cs");
				string WebVersionData = new WebClient().DownloadString("https://raw.githubusercontent.com/Ranchino-git/VRC_SkinnedMeshRenderer_BoneChanger/main/Editor/SkinnedMeshRendererBoneChanger/VersionData");
				File.WriteAllText(".\\Assets\\Script\\Editor\\SkinnedMeshRendererBoneChanger.cs", Tool);
				File.WriteAllText(".\\Assets\\Script\\Editor\\SkinnedMeshRendererBoneChanger\\VersionData", WebVersionData);
			}
		}
	}
}