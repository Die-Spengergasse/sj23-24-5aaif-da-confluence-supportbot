using System;

namespace Supportbot.Application.Model
{
    public interface IEntity
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
    }
}
