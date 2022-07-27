using System.ComponentModel.DataAnnotations;
using Mimon.Api.Dto;

namespace Mimon.BusinessLogic.Repositories.Users;

public class UserStorageElement
{
    [Key]
    public Guid Id { get; set; }
    public string UserName { get; set; }
    // TODO: public ??? GoogleId { get; set; } хз пока как это должно выглядеть (и надо ли вообще?...)
}