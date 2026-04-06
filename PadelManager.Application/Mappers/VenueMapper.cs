using PadelManager.Application.DTOs.Venue;
using PadelManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Mappers
{
    public static class VenueMapper
    {
        public static VenueResponseDto ToResponseDto(this Venue venue)
        {
            return new VenueResponseDto
            {
                Id = venue.Id,
                Name = venue.Name,
                Address = venue.Address,
                City = venue.City,
                PhoneNumber = venue.PhoneNumber,
                IsActive = venue.DeletedAt == null ? "Activo" : "Inactivo"
            };
        }

        public static IEnumerable<VenueResponseDto> ToResponseDto(this IEnumerable<Venue> venues)
        {
            return venues.Select(v => v.ToResponseDto());
        }

        public static void MapToEntity(this Venue existingEntity, UpdateVenueDto dto)
        {
            if (!string.IsNullOrEmpty(dto.Name)) existingEntity.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Address)) existingEntity.Address = dto.Address;
            if (!string.IsNullOrEmpty(dto.City)) existingEntity.City = dto.City;
            if (!string.IsNullOrEmpty(dto.PhoneNumber)) existingEntity.PhoneNumber = dto.PhoneNumber;
        }

        public static Venue ToEntity(this CreateVenueDto dto)
        {
            return new Venue
            {
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City,
                PhoneNumber = dto.PhoneNumber
            };
        }
    }
}
