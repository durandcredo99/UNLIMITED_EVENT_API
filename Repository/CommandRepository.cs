using Contracts;
using Entities;
using Entities.Extensions;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CommandRepository : RepositoryBase<Command>, ICommandRepository
    {
        private ISortHelper<Command> _sortHelper;

        public CommandRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Command> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Command>> GetCommandsAsync(CommandParameters commandParameters)
        {
            var commands = Enumerable.Empty<Command>().AsQueryable();

            ApplyFilters(ref commands, commandParameters);

            PerformSearch(ref commands, commandParameters.SearchTerm);

            var sortedCommands = _sortHelper.ApplySort(commands, commandParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Command>.ToPagedList
                (
                    sortedCommands,
                    commandParameters.PageNumber,
                    commandParameters.PageSize)
                );
        }

        public async Task<Command> GetCommandByIdAsync(Guid id)
        {
            return await FindByCondition(command => command.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CommandExistAsync(Command command)
        {
            return await FindByCondition(x => x.Date == command.Date)
                .AnyAsync();
        }

        public async Task CreateCommandAsync(Command command)
        {
            await CreateAsync(command);
        }

        public async Task UpdateCommandAsync(Command command)
        {
            await UpdateAsync(command);
        }

        public async Task UpdateCommandAsync(IEnumerable<Command> commands)
        {
            await UpdateAsync(commands);
        }

        public async Task DeleteCommandAsync(Command command)
        {
            await DeleteAsync(command);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Command> commands, CommandParameters commandParameters)
        {
            commands = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(commandParameters.AppUserId))
            {
                commands = commands.Where(x => x.AppUserId == commandParameters.AppUserId);
            }

            if (commandParameters.MinBirthday != null)
            {
                commands = commands.Where(x => x.Birthday >= commandParameters.MinBirthday);
            }

            if (commandParameters.MaxBirthday != null)
            {
                commands = commands.Where(x => x.Birthday < commandParameters.MaxBirthday);
            }

            if (commandParameters.MinCreateAt != null)
            {
                commands = commands.Where(x => x.CreateAt >= commandParameters.MinCreateAt);
            }

            if (commandParameters.MaxCreateAt != null)
            {
                commands = commands.Where(x => x.CreateAt < commandParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Command> commands, string searchTerm)
        {
            if (!commands.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            //commands = commands.Where(x => x.Title.ToLower().Contains(searchTerm.Trim().ToLower()));
            commands = commands.Where(x => x.Date.ToString().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}
