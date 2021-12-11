
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICommandRepository
    {
        Task<PagedList<Command>> GetCommandsAsync(CommandParameters commandParameters);

        Task<Command> GetCommandByIdAsync(Guid id);
        Task<bool> CommandExistAsync(Command command);

        Task CreateCommandAsync(Command command);
        Task UpdateCommandAsync(Command command);
        Task UpdateCommandAsync(IEnumerable<Command> commands);
        Task DeleteCommandAsync(Command command);
    }
}
