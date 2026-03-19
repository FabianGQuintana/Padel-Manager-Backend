using System;
using System.Collections.Generic;
using System.Linq;
using PadelManager.Application.DTOs.Statistic;
using PadelManager.Domain.Entities;

namespace PadelManager.Application.Mappers
{
    public static class StatisticMapper
    {
        public static StatisticResponseDto ToResponseDto(this Statistic statistic)
        {
            return new StatisticResponseDto
            {
                Id = statistic.Id,
                Points = statistic.Points,
                WoCount = statistic.WoCount,
                MatchesPlayed = statistic.MatchesPlayed,
                MatchesWon = statistic.MatchesWon,
                SetsWon = statistic.SetsWon,
                SetsLost = statistic.SetsLost,
                GamesWon = statistic.GamesWon,
                GamesLost = statistic.GamesLost,
                CoupleId = statistic.CoupleId,
                ZoneId = statistic.ZoneId
            };
        }

        public static IEnumerable<StatisticResponseDto> ToResponseDto(this IEnumerable<Statistic> statistics)
        {
            return statistics.Select(s => s.ToResponseDto());
        }

        public static void MapToEntity(this Statistic existingEntity, UpdateStatisticDto dto)
        {
            if (dto.CoupleId.HasValue) existingEntity.CoupleId = dto.CoupleId.Value;
            if (dto.ZoneId.HasValue) existingEntity.ZoneId = dto.ZoneId.Value;

            if (dto.Points.HasValue) existingEntity.Points = dto.Points.Value;
            if (dto.WoCount.HasValue) existingEntity.WoCount = dto.WoCount.Value;
            if (dto.MatchesPlayed.HasValue) existingEntity.MatchesPlayed = dto.MatchesPlayed.Value;
            if (dto.MatchesWon.HasValue) existingEntity.MatchesWon = dto.MatchesWon.Value;
            if (dto.SetsWon.HasValue) existingEntity.SetsWon = dto.SetsWon.Value;
            if (dto.SetsLost.HasValue) existingEntity.SetsLost = dto.SetsLost.Value;
            if (dto.GamesWon.HasValue) existingEntity.GamesWon = dto.GamesWon.Value;
            if (dto.GamesLost.HasValue) existingEntity.GamesLost = dto.GamesLost.Value;
        }

        public static Statistic ToEntity(this CreateStatisticDto dto)
        {
            return new Statistic
            {
                CoupleId = dto.CoupleId,
                ZoneId = dto.ZoneId,
                Points = dto.Points,
                WoCount = dto.WoCount,
                MatchesPlayed = dto.MatchesPlayed,
                MatchesWon = dto.MatchesWon,
                SetsWon = dto.SetsWon,
                SetsLost = dto.SetsLost,
                GamesWon = dto.GamesWon,
                GamesLost = dto.GamesLost
            };
        }
    }
}
