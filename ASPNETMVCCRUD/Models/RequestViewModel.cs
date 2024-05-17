using HelpComing.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HelpComing.Models
{
    public class RequestViewModel
    {
        public Models.Domain.Request Request { get; set; }
        public string Username { get; set; }
    }
}
