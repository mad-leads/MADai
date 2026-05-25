using System.Threading;
using System.Threading.Tasks;

namespace MADai.Application.Abstractions;

public interface IEmailSender
{
	Task SendAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default(CancellationToken));
}
