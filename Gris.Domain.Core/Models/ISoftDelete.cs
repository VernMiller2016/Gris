namespace Gris.Domain.Core.Models
{
    public interface ISoftDelete
    {
        bool Active { get; set; }
    }
}