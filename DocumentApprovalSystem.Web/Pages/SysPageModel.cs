using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DocumentApprovalSystem.Data;
using Microsoft.EntityFrameworkCore;
using wCyber.Helpers.Web;
using DinkToPdf;
using wCyber.Lib.FileStorage;

namespace DocumentApprovalSystem.Web.Pages
{
    [Authorize]
    public class SysPageModel : PageModel
    {
        public string BaseFilePath => @"c:/ministryportal/uploads";
        public Guid CurrentUserId => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        static bool IsDbCreated;
        DocumentApprovalDbContext? _db;
        public DocumentApprovalDbContext Db
        {
            get
            {
                if (_db == null)
                {
                    _db = Request.HttpContext.RequestServices.GetService<DocumentApprovalDbContext>();
                    if (!IsDbCreated)
                    {
                        try
                        {
                            _db?.Database.Migrate();
                        }
                        catch { }
                        IsDbCreated = true;
                    }
                }
                return _db!;
            }
        }


        User _currentUser;
        public User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                    _currentUser = Db.Users
                        .FirstOrDefault(c => c.Id == CurrentUserId);
                return _currentUser;
            }
        }

        IFileStore _filestore;
        protected IFileStore FileStore
        {
            get
            {
                if (_filestore == null) _filestore = Request.HttpContext.RequestServices.GetService<IFileStore>();
                return _filestore;
            }
        }

        [ViewData]
        public string? Title { get; protected set; }

        [ViewData]
        public string? PageTitle { get; protected set; }
        [ViewData]
        public dynamic? PageSubTitle { get; protected set; }

        //[ViewData]
        //public string SideNavPath { get; protected set; }

        //[ViewData]
        //public bool OverrideNav { get; protected set; }

        [ViewData]
        public PageActionBar ActionBar { get; protected set; } = new PageActionBar();

        [ViewData]
        public BreadCrumb? BreadCrumb { get; protected set; }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            Title = GetType().Namespace?[(GetType().Namespace.LastIndexOf(".") + 1)..];
            if (Title == "Pages" && GetType().Namespace!.Contains("Areas"))
                Title = GetType().Namespace!.Replace(".Pages", "")[(GetType().Namespace!.Replace(".Pages", "").LastIndexOf(".") + 1)..];
            BreadCrumb = new(this);
            base.OnPageHandlerExecuting(context);
        }

        public byte[] GeneratePdf(string link)
        {
            var PdfConverter = Request.HttpContext.RequestServices.GetService<DinkToPdf.Contracts.IConverter>();
            var cookies = new Dictionary<string, string>(Request.Cookies);
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings() { Top = 10, Bottom = 10 },

                },
                Objects = {
                    new ObjectSettings()
                    {
                        Page = link,
                        LoadSettings = new LoadSettings{ Cookies= cookies},
                        FooterSettings = new FooterSettings
                        {
                            Left = "Page [page] of [toPage]",
                            FontSize = 9,
                            Spacing = 10,
                        },
                    },
                }
            };
            return PdfConverter.Convert(doc);
        }
        public string RequestBaseUrl => $"{Request.Scheme}://{Request.Host}";
        public string RequestPath => $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
    }
}
