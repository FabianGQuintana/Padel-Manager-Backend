using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Match
{
    public class LoadMatchResultDto  // dto específico para cargar el tanteador
    {
        public Guid Id { get; set; } // El partido al que le cargamos el resultado
        public Guid WinnerCoupleId { get; set; }
        public Guid LoserCoupleId { get; set; }

        // Marcadores
        public int Set1_coupleA { get; set; }
        public int Set1_coupleB { get; set; }
        public int Set2_coupleA { get; set; }
        public int Set2_coupleB { get; set; }
        public int? Set3_coupleA { get; set; }
        public int? Set3_coupleB { get; set; }

    }
}
