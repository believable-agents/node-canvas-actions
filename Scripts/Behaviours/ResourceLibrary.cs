using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceLibrary : MonoBehaviour {

    [Serializable]
    public class ResourceGroup
    {
        public string name;
        public GameObject[] resources;
    }

    public ResourceGroup[] groups;

    public ResourceGroup FindGroup(string name)
    {
        return this.groups.First(g => g.name == name);
    }
}
