using Azure.Storage.Blobs.Models;
using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Lib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DocumentApprovalSystem.Web.Pages
{
    public class IndexModel : SysPageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public DocStatus DocStatus { get; set; }
        public List<Employee> Employees { get; private set; }
        public List<Employee> Customers { get; private set; }
        public List<DocRequest> DocRequest { get; private set; }

        public List<User> Users { get; private set; }
        public async Task<IActionResult> OnGet(Guid id)
        {
            //Title = PageTitle = "Dashboard";
            if (User.IsEmployee()) return RedirectToPage("/Index", new { area = "Account" });
            Employees = Db.Employees
                .Include(c => c.Title)
                .Include(c => c.Department)
                .ToList();
            Users = Db.Users.ToList();
            DocRequest = Db.DocRequests
                .Include(c => c.Employee.Title)
                .Include(c => c.DocType)
                .Include(c => c.Department)
                .ToList();
            return Page();
        }
    }
}
