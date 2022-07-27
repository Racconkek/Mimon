using System.ComponentModel.DataAnnotations;
using Mimon.Api.Dto;

namespace Mimon.BusinessLogic.Repositories.Reactions;

public class ReactionStorageElement
{
    [Key] public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid PhotoId { get; set; }
    public Reaction Reaction { get; set; }
}