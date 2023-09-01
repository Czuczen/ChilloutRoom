﻿using System.Threading.Tasks;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;

/// <summary>
/// Interfejs dla tworzenia rekordów graczy generowanych na podstawie definicji.
/// </summary>
/// <typeparam name="TCreateDto">Typ DTO używany do tworzenia encji.</typeparam>
public interface ICreateDefinition<in TCreateDto>
    where TCreateDto : class
{
    /// <summary>
    /// Tworzy rekordy graczy na podstawie utworzonej definicji.
    /// </summary>
    /// <param name="entity">DTO utworzonej definicji.</param>
    Task Create(TCreateDto entity);
}