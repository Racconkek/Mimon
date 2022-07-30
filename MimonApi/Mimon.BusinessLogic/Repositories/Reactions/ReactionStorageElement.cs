using System.ComponentModel.DataAnnotations;
using Mimon.Api.Dto;
using Mimon.Api.Dto.Photos;

namespace Mimon.BusinessLogic.Repositories.Reactions;

public class ReactionStorageElement
{
    [Key] public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid PhotoId { get; set; }
    public ReactionType ReactionType { get; set; }
}