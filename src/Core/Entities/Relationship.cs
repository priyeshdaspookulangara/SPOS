using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Relationship
{
    public int Id { get; set; }

    public int ParentCustomerId { get; set; }
    [ForeignKey("ParentCustomerId")]
    public Customer? ParentCustomer { get; set; }

    public int ChildCustomerId { get; set; }
    [ForeignKey("ChildCustomerId")]
    public Customer? ChildCustomer { get; set; }

    public string RelationshipType { get; set; } = "Is Parent Of";
}