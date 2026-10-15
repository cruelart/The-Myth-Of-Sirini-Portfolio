using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropTable", menuName = "Scriptable Objects/DropTable")]
public class PublicDropTable : ScriptableObject
{
    public string tableName;

    public List<DropTableEntry> dropTableData = new List<DropTableEntry>();
}
