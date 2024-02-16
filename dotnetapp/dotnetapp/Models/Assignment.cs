using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace dotnetapp.Models{
   public class Assignment
{
    public long AssignmentId { get; set; }
    public long ContainerId { get; set; }
    public long UserId { get; set; }
    public string Status { get; set; }
    public DateTime UpdateTime { get; set; }
    public string Route { get; set; }
    public string Shipment { get; set; }
    public string Destination { get; set; }
    
 
    public Container? Container { get; set; }

   
    public User? User { get; set; }

}

}