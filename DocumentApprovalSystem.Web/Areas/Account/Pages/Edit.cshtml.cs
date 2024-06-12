using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DocumentApprovalSystem.Web.Areas.Account.Pages;

public class EditModel : SysPageModel
{
    [BindProperty]
    public Employee Employee { get; private set; }
    [ViewData]
    public string Email { get; set; }
    [ViewData]
    public string Mobile { get; set; }
    public SelectList Titles { get; private set; }
    [BindProperty]
    public IFormFile ProfileFile { get; set; }
    public async Task OnGetAsync()
    {
        PageTitle = Title = "Edit my profile..";
        Employee = await Db.Employees
         .Include(c => c.IdNavigation)
         .Include(c => c.Title)
         .FirstOrDefaultAsync(c => c.Id == CurrentUserId);
        Email = Employee.User.Email;
        Mobile = Employee.User.Mobile;
        Titles = new SelectList(await Db.Titles.OrderBy(c => c.Name).ToListAsync(), nameof(Data.Title.Id), nameof(Data.Title.Name));
    }
    public async Task<IActionResult> OnPostAsync()
    {
        Employee = Db.Employees
            .Include(c => c.IdNavigation)
            .FirstOrDefault(c => c.Id == CurrentUserId);
        Employee.EcNumber = Employee.EcNumber;
        Employee.EmployeeId = CurrentUserId;

        if (await TryUpdateModelAsync(Employee, "", p => p.Title, p => p.FirstName, p => p.Surname, p => p.DoB, p => p.IdNumber, p => p.EcNumber, p => p.Address, p => p.Position))
        {
            //if (ProfileFile != null && ProfileFile.Length > 0)
            //{
            //    var finfo = new FileInfo(ProfileFile.FileName);
            //    var attachment = new Attachment
            //    {
            //        Id = Guid.NewGuid(),
            //        CreationDate = DateTime.UtcNow,
            //        UniqueId = Employee.Id,
            //        Container = nameof(Employee),
            //        Size = (int)ProfileFile.Length,
            //        CreatorId = CurrentUserId,
            //        Name = ProfileFile?.Name,
            //        Extension = finfo.Extension
            //    };
            //    await Db.Attachments.AddAsync(attachment);
            //    //await FileStore.UploadAsync(attachment.Container, attachment.UniqueId, $"{attachment.Id}{attachment.Extension}", ProfileFile.OpenReadStream());
            //    Employee.ProfilePictureId = attachment.Id;
            //}
            await Db.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
        return Page();
    }
}
