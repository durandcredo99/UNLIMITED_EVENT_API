
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IMailRepository
    {
        Task SendEmailAsync(EmailModel emailData);
    }
}
