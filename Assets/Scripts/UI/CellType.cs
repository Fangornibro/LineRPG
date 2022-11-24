using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellType : MonoBehaviour
{
    public enum Type { Everything, HeadArmor, ChestArmor, LegArmor, Usable, Quest }
    public Type cellType;
}
