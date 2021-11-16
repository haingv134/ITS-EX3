using System;
using Microsoft.AspNetCore.Authorization;

namespace WebLayer.Security.Requirement 
{
    public class GenzRequirements : IAuthorizationRequirement
    {
        public GenzRequirements(int fromYear = 2000, int toYear = 2020)
        {
            this.fromYear = fromYear;
            this.toYear = toYear;
        }

        public int fromYear {get; set;}
        public int toYear {get; set;}
    }
}