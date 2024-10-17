using System.ComponentModel.DataAnnotations;
using Points.Contracts.Point;

namespace PixiePointsApp.GraphQL.Dto;

public class GetAddressPointsLogDto
{
    public IncomeSourceType Role { get; set; }
    public string Address { get; set; }
}