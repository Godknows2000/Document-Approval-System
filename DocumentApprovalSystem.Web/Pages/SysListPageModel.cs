using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using DocumentApprovalSystem.Web.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace DocumentApprovalSystem.Web.Pages
{
    public class SysListPageModel<T> : SysPageModel where T : class
    {
        [ViewData]
        public string? SearchPlaceholder { get; protected set; }
        [ViewData]
        public string? QueryString { get; protected set; }
        public IPagedList<T> List { get; protected set; }
        protected int DefaultPageSize { get; set; } = 50;
        public void SetPageTitles(string description)
        {
            Title = description.Pluralize();
            PageTitle = description.ToQuantity(List.TotalItemCount) + (QueryString == null ? "" : " found..");
            if (List?.PageCount > 1) PageSubTitle = new HtmlString($"Page <strong>{List.PageNumber}</strong> of <strong>{List.PageCount}</strong>");
            SearchPlaceholder = $"Search {Title}..".Humanize(LetterCasing.Sentence);
        }
    }
}
