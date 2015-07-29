#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GreedyGame.Runtime.Common;

public class DynamicUnitManager : ScriptableObject {
	public List<UnitItem> Units;
	public string GameProfile = null;
	public string PostLevel = null;

	public string[] UnitsID {
		get {
			List<string> s = new List<string>();
			foreach(UnitItem u in Units) {
				s.Add(u.unitId);
			}

			return s.ToArray();
		}
	}

	//Singleton Mechanism
    private static DynamicUnitManager s_Instance = null;

    public static DynamicUnitManager Instance {
		get {
			return GetInstance();
		}
	}

    private static DynamicUnitManager GetInstance() {

		if( s_Instance == null ) {
#if UNITY_EDITOR
			// If there's no instance, load or create one
			DynamicUnitManager asset = null;
			string assetPathAndName = GeneratePath();

			// Check the asset database for an existing instance of the asset
            asset = AssetDatabase.LoadAssetAtPath(assetPathAndName, typeof(ScriptableObject)) as DynamicUnitManager;
			
			// If the asset doesn't exist, create it
			if( asset == null ) {
                asset = ScriptableObject.CreateInstance<DynamicUnitManager>();
                asset.Units = new List<UnitItem>();
				AssetDatabase.CreateAsset( asset, assetPathAndName );
				AssetDatabase.SaveAssets();	
			}
			s_Instance = asset;
#else
			s_Instance = ScriptableObject.FindObjectOfType(typeof(DynamicUnitManager)) as DynamicUnitManager;
#endif
		}

		return s_Instance;
	}
	
	public void SaveInstanceData() {
#if UNITY_EDITOR
		EditorUtility.SetDirty( s_Instance );
		AssetDatabase.SaveAssets();
#endif
	}
	
	private static string GeneratePath() {
        return "Assets/" + typeof(DynamicUnitManager).ToString() + ".asset";
	}

}

