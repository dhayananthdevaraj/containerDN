using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace dotnetapp.Models
{
  public class User
{
    public long UserId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }
    public string MobileNumber { get; set; }
    public string UserRole { get; set; }

    [JsonIgnore]
    public List<Assignment>? Assignments { get; set; }

    [JsonIgnore]
    public List<Issue>? Issues { get; set; }
}



}
