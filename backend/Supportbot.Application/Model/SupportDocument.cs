using System;

namespace Supportbot.Application.Model
{
    public record SupportDocument(string Title, DateTime Created, string Content);

}
