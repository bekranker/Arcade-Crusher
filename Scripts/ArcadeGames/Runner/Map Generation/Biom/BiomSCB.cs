using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Biom", menuName = "Procuderal Runner / Create Biom", order = 1)]
public class BiomSCB : ScriptableObject
{
    public ProcuderalPacket PrePucket;
    public List<Material> Materials;
    public GameObject BiomGroundPrefab;
}