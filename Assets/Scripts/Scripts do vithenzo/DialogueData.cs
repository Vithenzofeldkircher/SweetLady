using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NovoDialogo", menuName = "Dialogo/Novo Dialogo")]
public class DialogueData : ScriptableObject
{
    [TextArea(2, 5)]
    public List<string> falas = new List<string>();
}