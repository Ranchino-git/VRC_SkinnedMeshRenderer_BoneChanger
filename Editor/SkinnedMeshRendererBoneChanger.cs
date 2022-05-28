using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Net;
using System.IO;

namespace StudioRan
{
	namespace VRChat
	{
		public class BackgroundColorScope : GUI.Scope
		{
			private readonly Color Bcolor, Ccolor;
			public BackgroundColorScope(Color IBcolor, Color ICcolor)
			{
				Bcolor = GUI.backgroundColor;
				Ccolor = GUI.contentColor;
				GUI.backgroundColor = IBcolor;
				GUI.contentColor = ICcolor;
			}

			protected override void CloseScope()
			{
				GUI.backgroundColor = Bcolor;
				GUI.contentColor = Ccolor;
			}
		}

		public class SkinnedMeshRendererBoneChanger : EditorWindow
		{
			internal string WebVersion = new WebClient().DownloadString("https://raw.githubusercontent.com/Ranchino-git/VRC_SkinnedMeshRenderer_BoneChanger/main/Editor/SkinnedMeshRendererBoneChanger/VersionData");
			internal string LocalVersion = File.ReadAllText(".\\Assets\\Script\\Editor\\SkinnedMeshRendererBoneChanger\\VersionData");
			private Object Cloth;
			private Object RootArmature;
			private Object CachedObject;
			public List<Object> Old_Bones = new List<Object>();
			private bool isBoneChecked = false;
			private bool SaveBone = false;

			private bool BoneListWindow = false;
			private bool ToolOptionWindow = false;

			Vector2 scrollPosition = Vector2.zero;

			[MenuItem ("Tools/SkinnedMeshRenderer Bone Changer")]
			public static void ShowWindow()
			{
				EditorWindow.GetWindow(typeof(SkinnedMeshRendererBoneChanger));
			}

			void OnGUI()
			{
				if(WebVersion != LocalVersion)
				{
					using (new BackgroundColorScope (Color.white, Color.red * 2))
						GUILayout.Box("※Update!※", GUILayout.Width(position.width));

					GUILayout.Label("Current Version: " + LocalVersion);
					GUILayout.Label("New Version: " + WebVersion);
					if(GUILayout.Button("Update Tool", GUILayout.Height(30)))
					{
						new SkinnedMeshRendererBoneChangerUpdate().UpdateTool();
					}
					if(GUILayout.Button("Update Tool", GUILayout.Height(30)))
					{
						Repaint();
					}
				}
				GUILayout.Label("응애 옷 바꺼");

				GUILayout.Space(10);
				Cloth = EditorGUILayout.ObjectField("Cloth", Cloth, typeof(GameObject), true);
				GUILayout.Space(10);

				if(GUILayout.Button("Bone Find", GUILayout.Height(30)) && Cloth != null)
				{
					BoneFind();
					isBoneChecked = true;
					CachedObject = Cloth;
					BoneListWindow = true;
					ToolOptionWindow = false;
				}

				if(BoneListWindow = ColorChangeToggle(BoneListWindow, "▲Bone List▲", "▼Bone List▼", Color.white*1.3f, Color.grey, Color.white, GUILayout.Height(30)))
				{
					ToolOptionWindow = false;
					scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height - 228));
					if(isBoneChecked && CachedObject == Cloth && Cloth != null)
					{
						
						Repaint();
						SkinnedMeshRenderer rend = ((GameObject)Cloth).GetComponent<SkinnedMeshRenderer>();
						for(int i = 0; i < Old_Bones.Count; i++)
						{
							if(rend.bones[i] == null)
								continue;
							
							if (Old_Bones[i] == null)
								using (new BackgroundColorScope (Color.red, new Color(2, 0.5f, 0.5f)))
									Old_Bones[i] = EditorGUILayout.ObjectField("#" + i.ToString() + " [" + rend.bones[i].name + "]", Old_Bones[i], typeof(GameObject), true);
							else if(Old_Bones[i] != rend.bones[i].gameObject)
								using (new BackgroundColorScope (new Color(1.4f, 1.4f, 2.4f), new Color(2, 2, 0.6f)))
									Old_Bones[i] = EditorGUILayout.ObjectField("#" + i.ToString() + " [" + rend.bones[i].name + "]", Old_Bones[i], typeof(GameObject), true);
							else
								using (new BackgroundColorScope (new Color(1f, 1f, 1f), Color.white))
									Old_Bones[i] = EditorGUILayout.ObjectField("#" + i.ToString() + " [" + rend.bones[i].name + "]", Old_Bones[i], typeof(GameObject), true);
						}
					}
					else if(isBoneChecked && SaveBone && Cloth != null)
					{
						Repaint();
						GUILayout.Label("▼Bone List▼");
						SkinnedMeshRenderer rend = ((GameObject)Cloth).GetComponent<SkinnedMeshRenderer>();
						for(int i = 0; i < Old_Bones.Count; i++)
						{
							if(rend.bones[i] == null)
								continue;
							Old_Bones[i] = EditorGUILayout.ObjectField("#" + i.ToString() + " [" + rend.bones[i].name + "]", Old_Bones[i], typeof(GameObject), true);
						}
					}
					GUILayout.Label("\n");
					GUILayout.EndScrollView();
				}

				if(ToolOptionWindow = ColorChangeToggle(ToolOptionWindow, "▲Tool Option▲", "▼ToolOption▼", Color.white*1.3f, Color.grey, Color.white, GUILayout.Height(30)))
				{
					BoneListWindow = false;

					using (new BackgroundColorScope (Color.black, Color.white))
						GUILayout.Box("◆Auto Match◆", GUILayout.Width(position.width), GUILayout.Height(25));
					RootArmature = EditorGUILayout.ObjectField("Root Armature", RootArmature, typeof(GameObject), true);
					if(GUILayout.Button("Auto Match", GUILayout.Height(20)) && RootArmature != null)
						AutoBoneMatch();

					//contour
					GUILayout.Space(20);

					using (new BackgroundColorScope (Color.black, Color.white))
						GUILayout.Box("◆Bone Save◆", GUILayout.Width(position.width), GUILayout.Height(25));
					SaveBone = ColorChangeToggle(SaveBone, "Bone Save On", "Bone Save Off", Color.green, Color.red, Color.white, GUILayout.Height(40));
					
					//contour
					GUILayout.Space(20);

					using (new BackgroundColorScope (Color.red*2, Color.white))
						GUILayout.Box("◆Reset◆", GUILayout.Width(position.width), GUILayout.Height(25));
					//Reset
					using (new BackgroundColorScope (Color.red, new Color(2, 0.5f, 0.5f)))
					{
						if(GUILayout.Button("Reset Tool"))
						{
							Cloth = null;
							CachedObject = null;
							Old_Bones.Clear();
							isBoneChecked = false;
							SaveBone = false;
							BoneListWindow = false;
							ToolOptionWindow = false;
							RootArmature = null;
						}
					}
					//contour
					GUILayout.Space(20);
				}
				if(GUILayout.Button("Bone Apply", GUILayout.Height(40)) && Cloth != null)
				{
					BoneApply();
				}
			}

			private bool ColorChangeToggle(bool Toggled, string TextOn, string TextOff, Color OnColor, Color OffColor, Color TextColor,params GUILayoutOption[] option)
			{
				if(Toggled)
					using (new BackgroundColorScope (OnColor, TextColor))
						return GUILayout.Toggle(Toggled, Toggled ? TextOn : TextOff, "button", option);
				else
					using (new BackgroundColorScope (OffColor, TextColor))
						return GUILayout.Toggle(Toggled, Toggled ? TextOn : TextOff, "button", option);
			}

			private void AutoBoneMatch()
			{
				BoneListWindow = true;
				ToolOptionWindow = false;
				Transform[] Rootrend = ((GameObject)RootArmature).transform.GetComponentsInChildren<Transform>(true);
				for(int i = 0; i < Old_Bones.Count; i++)
				{
					for(int j = 0; j < Rootrend.Length; j++)
					{
						if(Rootrend[j].gameObject.name == ((GameObject)Old_Bones[i]).name)
							Old_Bones[i] = Rootrend[j].gameObject;
					}
				}
			}

			private void BoneFind()
			{
				if(Old_Bones.Count > 0 && !SaveBone)
				{
					SkinnedMeshRenderer rend = ((GameObject)Cloth).GetComponent<SkinnedMeshRenderer>();
					Old_Bones.Clear();
					for(int i = 0; i < rend.bones.Length && !SaveBone; i++)
					{
						try
						{
							Old_Bones.Add(null);
							if(rend.bones[i] != null)
								Old_Bones[i] = rend.bones[i].gameObject;
						}
						catch
						{
							Old_Bones.Clear();
							EditorUtility.DisplayDialog("Bone has lost.", "Bone has lost.","ok","");
						}
					}
				}
			}

			private void BoneApply()
			{
				SkinnedMeshRenderer rend = ((GameObject)Cloth).GetComponent<SkinnedMeshRenderer>();
				Transform[] bones = rend.bones;
				GameObject GameObjectBase;
				for (int i = 0; i < Old_Bones.Count; i++)
				{
					GameObjectBase = (GameObject)Old_Bones[i];
					if(GameObjectBase != null)
					{
						bones[i] = GameObjectBase.transform;
					}
				}
				rend.bones = bones;
			}
		}
	}
}