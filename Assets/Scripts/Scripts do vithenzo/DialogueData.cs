using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NovoDialogo", menuName = "Dialogo/Novo Dialogo")]
public class DialogueData : ScriptableObject
{
    [Serializable]
    public class Fala
    {
        public string nomePersonagem;   // nome acima da fala
        [TextArea(2, 5)]
        public string texto;            // fala do personagem
    }

    public List<Fala> falas = new List<Fala>();
}
