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
				string WebVersion = new WebClient().DownloadString("https://raw.githubusercontent.com/Ranchino-git/VRC_SkinnedMeshRenderer_BoneChanger/main/Editor/SkinnedMeshRendererBoneChanger.cs");
				File.WriteAllText(".\\Assets\\Script\\Editor\\SkinnedMeshRendererBoneChanger.cs", WebVersion);
			}
		}
	}
}