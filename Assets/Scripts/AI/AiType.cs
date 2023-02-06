using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AiType", menuName = "ScriptableObjects/AiType")]
public class AiType : ScriptableObject
{
    public AiEnum aiEnum = AiEnum.Null;
    public enum AiEnum
    {
        Null,
        Boar,
        Deer,
        Wolf,
        Bison
    }
}
