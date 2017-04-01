using System.ComponentModel;

namespace Gris.Domain.Core.Models
{
    public abstract class SoftDeleteEntity : ISoftDelete
    {
        [DefaultValue(true)]
        public bool Active { get; set; }
    }
}