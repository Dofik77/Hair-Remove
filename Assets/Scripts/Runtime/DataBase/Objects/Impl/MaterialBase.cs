using System;
using UnityEngine;

namespace Runtime.DataBase.Objects.Impl
{
    public class MaterialBase : ScriptableObject
    {
        [SerializeField] private Material[] _materials;
        
        public Material Get(string prefabName)
        {
            for (var i = 0; i < _materials.Length; i++)
            {
                var prefab = _materials[i];
                if (prefab.name == prefabName)
                    return prefab;
            }

            throw new Exception("[PrefabsBase] Can't find prefab with name: " + prefabName);
        }
    }
}
