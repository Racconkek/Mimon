namespace Mimon.Api.Dto.Photos;

public class Reaction
{
    public Guid? Id { get; set; }
    public Guid PhotoId { get; set; }
    public Guid UserId { get; set; }
    public ReactionType ReactionType { get; set; }
}