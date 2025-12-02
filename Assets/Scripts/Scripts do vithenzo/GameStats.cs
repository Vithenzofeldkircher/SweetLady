using UnityEngine;

public static class GameStats
{

    public static bool radioDialogoInicialTocado = false;
    public static bool radioRelatorioAtivo = false; // para controlar quando toca o relatório
    public static bool jaFaleiComNPC = false;  // Controla se jogador falou com NPC

    // Radio / diálogo
    public static bool mostrarRadio = false;
    public static string relatorioUltimaNoite = "";

    // Estatísticas de mortes
    public static int totalInocentesMortos = 0;
    public static int totalImpostoresMortos = 0;

    // Dados do NPC atual (persistidos entre cenas)
    public static Sprite currentNPCSprite = null;
    public static bool currentNPCIsImpostor = false;

    // Configurações
    public static bool shouldGoToRoomAfterDialog = true; // se true, ao terminar diálogo da Game -> RoomScene
    public static bool mostrarRadioNaProximaEntrada = false; // se quiser forçar rádio
}



