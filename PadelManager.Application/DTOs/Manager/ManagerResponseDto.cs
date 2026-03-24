using PadelManager.Application.DTOs.Tournament;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Manager
{
    public class ManagerResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Dni { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Email { get; set; } = null!;


        public List<TournamentResponseDto> Tournaments { get; set; } = new();

    }
}
