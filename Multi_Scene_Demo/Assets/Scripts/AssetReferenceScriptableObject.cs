using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class AssetReferenceScriptableObject : AssetReferenceT<ScriptableObject>
{
	public AssetReferenceScriptableObject(string guid) : base(guid) { }
}