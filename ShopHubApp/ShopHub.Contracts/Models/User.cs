using System;
using System.Collections.Generic;
using System.Text;

namespace ShopHub.Contracts.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}
