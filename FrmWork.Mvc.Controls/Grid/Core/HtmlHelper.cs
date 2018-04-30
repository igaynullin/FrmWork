using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using FrmWork.Mvc.Controls.Grid.Common;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrmWork.Mvc.Controls.Grid.Core
{
    ///<summary>
    ///	Extension methods for generating paging controls that can operate on instances of IPagedList.
    ///</summary>
    public static class HtmlHelper
    {
        private static void SetInnerText(TagBuilder tagBuilder, string innerText)
        {
            tagBuilder.InnerHtml.SetContent(innerText);
        }

        private static string TagBuilderToString(TagBuilder tagBuilder, TagRenderMode renderMode = TagRenderMode.Normal)
        {
            var encoder = HtmlEncoder.Create(new TextEncoderSettings());
            var writer = new System.IO.StringWriter() as TextWriter;
            tagBuilder.WriteTo(writer, encoder);
            return writer.ToString();
        }

        private static TagBuilder WrapInListItem(string text)
        {
            var li = new TagBuilder("li");
            SetInnerText(li, text);
            return li;
        }

        private static TagBuilder WrapInListItem(TagBuilder inner, PagedListRenderOptionsBase options, params string[] classes)
        {
            var li = new TagBuilder("li");
            foreach (var @class in classes)
                li.AddCssClass(@class);

            if (options is PagedListRenderOptions)
            {
                if (((PagedListRenderOptions)options).FunctionToTransformEachPageLink != null)
                {
                    inner = ((PagedListRenderOptions)options).FunctionToTransformEachPageLink(li, inner);
                }
            }
            li.InnerHtml.AppendHtml(TagBuilderToString(inner));

            return li;
        }

        private static void AddCssClasses(TagBuilder a, PagedListRenderOptionsBase options)
        {
            foreach (var c in options.PageClasses ?? Enumerable.Empty<string>())
                a.AddCssClass(c);
        }

        private static TagBuilder First(IPagedList list, string urlFormat, PagedListRenderOptionsBase options)
        {
            const int targetPageNumber = 1;
            var first = new TagBuilder("a");

            first.InnerHtml.AppendHtml(string.Format(options.LinkToFirstPageFormat, targetPageNumber));
            AddCssClasses(first, options);

            if (list.IsFirstPage)
                return WrapInListItem(first, options, "PagedList-skipToFirst", "disabled");

            first.Attributes["href"] = string.Format(urlFormat, targetPageNumber);
            return WrapInListItem(first, options, "PagedList-skipToFirst");
        }

        private static TagBuilder Previous(IPagedList list, string urlFormat, PagedListRenderOptionsBase options)
        {
            var targetPageNumber = list.PageNumber - 1;
            var previous = new TagBuilder("a");
            previous.InnerHtml.AppendHtml(string.Format(options.LinkToPreviousPageFormat, targetPageNumber));
            previous.Attributes["rel"] = "prev";

            AddCssClasses(previous, options);

            if (!list.HasPreviousPage)
                return WrapInListItem(previous, options, options.PreviousElementClass, "disabled");

            previous.Attributes["href"] = string.Format(urlFormat, targetPageNumber);
            return WrapInListItem(previous, options, options.PreviousElementClass);
        }

        private static TagBuilder Page(int i, IPagedList list, string urlFormat, PagedListRenderOptionsBase options)
        {
            var format = options.FunctionToDisplayEachPageNumber
                         ?? (pageNumber => string.Format(options.LinkToIndividualPageFormat, pageNumber));
            var targetPageNumber = i;
            var page = i == list.PageNumber ? new TagBuilder("span") : new TagBuilder("a");
            SetInnerText(page, format(targetPageNumber));

            AddCssClasses(page, options);

            if (i == list.PageNumber)
                return WrapInListItem(page, options, options.ActiveLiElementClass);

            page.Attributes["href"] = string.Format(urlFormat, targetPageNumber);

            return WrapInListItem(page, options);
        }

        private static TagBuilder Next(IPagedList list, string urlFormat, PagedListRenderOptionsBase options)
        {
            var targetPageNumber = list.PageNumber + 1;
            var next = new TagBuilder("a");
            next.InnerHtml.AppendHtml(string.Format(options.LinkToNextPageFormat, targetPageNumber));
            next.Attributes["rel"] = "next";

            AddCssClasses(next, options);

            if (!list.HasNextPage)
                return WrapInListItem(next, options, options.NextElementClass, "disabled");

            next.Attributes["href"] = string.Format(urlFormat, targetPageNumber);
            return WrapInListItem(next, options, options.NextElementClass);
        }

        private static TagBuilder Last(IPagedList list, string urlFormat, PagedListRenderOptionsBase options)
        {
            var targetPageNumber = list.PageCount;
            var last = new TagBuilder("a");
            last.InnerHtml.AppendHtml(string.Format(options.LinkToLastPageFormat, targetPageNumber));
            AddCssClasses(last, options);

            if (list.IsLastPage)
                return WrapInListItem(last, options, "PagedList-skipToLast", "disabled");

            last.Attributes["href"] = string.Format(urlFormat, targetPageNumber);
            return WrapInListItem(last, options, "PagedList-skipToLast");
        }

        private static TagBuilder PageCountAndLocationText(IPagedList list, PagedListRenderOptionsBase options)
        {
            var text = new TagBuilder("a");
            SetInnerText(text, string.Format(options.PageCountAndCurrentLocationFormat, list.PageNumber, list.PageCount));

            return WrapInListItem(text, options, "PagedList-pageCountAndLocation", "disabled");
        }

        private static TagBuilder ItemSliceAndTotalText(IPagedList list, PagedListRenderOptionsBase options)
        {
            var text = new TagBuilder("a");
            SetInnerText(text, string.Format(options.ItemSliceAndTotalFormat, list.FirstItemOnPage, list.LastItemOnPage, list.TotalItemCount));

            return WrapInListItem(text, options, "PagedList-pageCountAndLocation", "disabled");
        }

        private static TagBuilder Ellipses(PagedListRenderOptionsBase options)
        {
            var a = new TagBuilder("a");
            a.InnerHtml.AppendHtml(options.EllipsesFormat);
            return WrapInListItem(a, options, options.EllipsesElementClass, "disabled");
        }

        private static string BuildUrlFormat(string area, string controller, string action, object routeValues)
        {
            var str = new StringBuilder();
            var counter = 0;
            if (!string.IsNullOrEmpty(area))
            {
                str
                    .Append(area).Append("/");
            }
            if (string.IsNullOrEmpty(controller))
            {
                throw new ArgumentNullException(nameof(controller));
            }
            if (string.IsNullOrEmpty(action))
            {
                throw new ArgumentNullException(nameof(action));
            }
            str
                .Append(controller).Append("/")
                .Append(action)
                .Append("?")
                .Append(nameof(IPagedList.PageNumber))
                .Append("={0}");

            if (routeValues != null)
            {
                var properties = routeValues.GetType().GetProperties();
                foreach (var property in properties)
                {
                    if (!property.GetCustomAttributes<NotMappedAttribute>().Any())
                    {
                        if (property.PropertyType.GetInterface(nameof(IList)) != null)
                        {
                            var arrayValues = property.GetValue(routeValues, null);
                            if (arrayValues == null) continue;
                            foreach (var item in (IEnumerable)arrayValues)
                            {
                                //  str.AppendFormat("&{0}[{1}]={2}", property.Name, counter, item.ToString());
                                str.Append("&").Append(property.Name)
                                    .Append("[").Append(counter.ToString()).Append("]")
                                    .Append("=").Append(item);
                                counter++;
                            }
                            counter = 0;
                        }
                        else
                        {
                            var propValue = property.GetValue(routeValues, null);
                            if (!string.IsNullOrEmpty(propValue?.ToString()))
                            {
                                //str.AppendFormat("&{0}={1}", property.Name, (string)propValue);
                                str.Append("&").Append(property.Name)
                                    .Append("=").Append(propValue);
                            }
                        }
                    }
                }
            }
            return str.ToString();
        }

        public static HtmlString PagedListPager(this IHtmlHelper html
            , IPagedList list
            , string area
            , string controller
            , string action
            , int pageNumber
            , object routeValues
            , PagedListRenderOptionsBase options)
        {
            if (options == null)
                options = new PagedListRenderOptions();

            if (options.Display == PagedListDisplayMode.Never || (options.Display == PagedListDisplayMode.IfNeeded && list.PageCount <= 1))
                return null;

            var listItemLinks = new List<TagBuilder>();
            //
            var href = BuildUrlFormat(area, controller, action, routeValues);
            //calculate start and end of range of page numbers
            var firstPageToDisplay = 1;
            var lastPageToDisplay = list.PageCount;
            var pageNumbersToDisplay = lastPageToDisplay;
            if (options.MaximumPageNumbersToDisplay.HasValue && list.PageCount > options.MaximumPageNumbersToDisplay)
            {
                // cannot fit all pages into pager
                var maxPageNumbersToDisplay = options.MaximumPageNumbersToDisplay.Value;
                firstPageToDisplay = list.PageNumber - maxPageNumbersToDisplay / 2;
                if (firstPageToDisplay < 1)
                    firstPageToDisplay = 1;
                pageNumbersToDisplay = maxPageNumbersToDisplay;
                lastPageToDisplay = firstPageToDisplay + pageNumbersToDisplay - 1;
                if (lastPageToDisplay > list.PageCount)
                    firstPageToDisplay = list.PageCount - maxPageNumbersToDisplay + 1;
            }

            //first
            if (options.DisplayLinkToFirstPage == PagedListDisplayMode.Always || (options.DisplayLinkToFirstPage == PagedListDisplayMode.IfNeeded && firstPageToDisplay > 1))
                listItemLinks.Add(First(list, href, options));

            //previous
            if (options.DisplayLinkToPreviousPage == PagedListDisplayMode.Always || (options.DisplayLinkToPreviousPage == PagedListDisplayMode.IfNeeded && !list.IsFirstPage))
                listItemLinks.Add(Previous(list, href, options));

            //text
            if (options.DisplayPageCountAndCurrentLocation)
                listItemLinks.Add(PageCountAndLocationText(list, options));

            //text
            if (options.DisplayItemSliceAndTotal)
                listItemLinks.Add(ItemSliceAndTotalText(list, options));

            //page
            if (options.DisplayLinkToIndividualPages)
            {
                //if there are previous page numbers not displayed, show an ellipsis
                if (options.DisplayEllipsesWhenNotShowingAllPageNumbers && firstPageToDisplay > 1)
                    listItemLinks.Add(Ellipses(options));

                foreach (var i in Enumerable.Range(firstPageToDisplay, pageNumbersToDisplay))
                {
                    //show delimiter between page numbers
                    if (i > firstPageToDisplay && !string.IsNullOrWhiteSpace(options.DelimiterBetweenPageNumbers))
                        listItemLinks.Add(WrapInListItem(options.DelimiterBetweenPageNumbers));

                    //show page number link
                    listItemLinks.Add(Page(i, list, href, options));
                }

                //if there are subsequent page numbers not displayed, show an ellipsis
                if (options.DisplayEllipsesWhenNotShowingAllPageNumbers && (firstPageToDisplay + pageNumbersToDisplay - 1) < list.PageCount)
                    listItemLinks.Add(Ellipses(options));
            }

            //next
            if (options.DisplayLinkToNextPage == PagedListDisplayMode.Always || (options.DisplayLinkToNextPage == PagedListDisplayMode.IfNeeded && !list.IsLastPage))
                listItemLinks.Add(Next(list, href, options));

            //last
            if (options.DisplayLinkToLastPage == PagedListDisplayMode.Always || (options.DisplayLinkToLastPage == PagedListDisplayMode.IfNeeded && lastPageToDisplay < list.PageCount))
                listItemLinks.Add(Last(list, href, options));

            if (listItemLinks.Any())
            {
                //append class to first item in list?
                if (!string.IsNullOrWhiteSpace(options.ClassToApplyToFirstListItemInPager))
                    listItemLinks.First().AddCssClass(options.ClassToApplyToFirstListItemInPager);

                //append class to last item in list?
                if (!string.IsNullOrWhiteSpace(options.ClassToApplyToLastListItemInPager))
                    listItemLinks.Last().AddCssClass(options.ClassToApplyToLastListItemInPager);

                //append classes to all list item links
                foreach (var li in listItemLinks)
                    foreach (var c in options.LiElementClasses ?? Enumerable.Empty<string>())
                        li.AddCssClass(c);
            }

            //collapse all of the list items into one big string
            var listItemLinksString = listItemLinks.Aggregate(
                new StringBuilder(),
                (sb, listItem) => sb.Append(TagBuilderToString(listItem)),
                sb => sb.ToString()
                );

            var ul = new TagBuilder("ul");
            ul.InnerHtml.AppendHtml(listItemLinksString);
            foreach (var c in options.UlElementClasses ?? Enumerable.Empty<string>())
                ul.AddCssClass(c);

            if (options.UlElementattributes != null)
            {
                foreach (var c in options.UlElementattributes)
                    ul.MergeAttribute(c.Key, c.Value);
            }

            var outerDiv = new TagBuilder("div");
            foreach (var c in options.ContainerDivClasses ?? Enumerable.Empty<string>())
                outerDiv.AddCssClass(c);
            outerDiv.InnerHtml.AppendHtml(TagBuilderToString(ul));
            return new HtmlString(TagBuilderToString(outerDiv));
        }

        public static HtmlString PagedListPager(this IHtmlHelper html
          , IPagedList list

          , PagedListRenderOptionsBase options)
        {
            if (options == null)
                options = new PagedListRenderOptions();

            if (options.Display == PagedListDisplayMode.Never || (options.Display == PagedListDisplayMode.IfNeeded && list.PageCount <= 1))
                return null;

            var listItemLinks = new List<TagBuilder>();
            //
            var href = BuildUrlFormat(list.Area, list.Controller, list.Action, list.RouteValues);
            //calculate start and end of range of page numbers
            var firstPageToDisplay = 1;
            var lastPageToDisplay = list.PageCount;
            var pageNumbersToDisplay = lastPageToDisplay;
            if (options.MaximumPageNumbersToDisplay.HasValue && list.PageCount > options.MaximumPageNumbersToDisplay)
            {
                // cannot fit all pages into pager
                var maxPageNumbersToDisplay = options.MaximumPageNumbersToDisplay.Value;
                firstPageToDisplay = list.PageNumber - maxPageNumbersToDisplay / 2;
                if (firstPageToDisplay < 1)
                    firstPageToDisplay = 1;
                pageNumbersToDisplay = maxPageNumbersToDisplay;
                lastPageToDisplay = firstPageToDisplay + pageNumbersToDisplay - 1;
                if (lastPageToDisplay > list.PageCount)
                    firstPageToDisplay = list.PageCount - maxPageNumbersToDisplay + 1;
            }

            //first
            if (options.DisplayLinkToFirstPage == PagedListDisplayMode.Always || (options.DisplayLinkToFirstPage == PagedListDisplayMode.IfNeeded && firstPageToDisplay > 1))
                listItemLinks.Add(First(list, href, options));

            //previous
            if (options.DisplayLinkToPreviousPage == PagedListDisplayMode.Always || (options.DisplayLinkToPreviousPage == PagedListDisplayMode.IfNeeded && !list.IsFirstPage))
                listItemLinks.Add(Previous(list, href, options));

            //text
            if (options.DisplayPageCountAndCurrentLocation)
                listItemLinks.Add(PageCountAndLocationText(list, options));

            //text
            if (options.DisplayItemSliceAndTotal)
                listItemLinks.Add(ItemSliceAndTotalText(list, options));

            //page
            if (options.DisplayLinkToIndividualPages)
            {
                //if there are previous page numbers not displayed, show an ellipsis
                if (options.DisplayEllipsesWhenNotShowingAllPageNumbers && firstPageToDisplay > 1)
                    listItemLinks.Add(Ellipses(options));

                foreach (var i in Enumerable.Range(firstPageToDisplay, pageNumbersToDisplay))
                {
                    //show delimiter between page numbers
                    if (i > firstPageToDisplay && !string.IsNullOrWhiteSpace(options.DelimiterBetweenPageNumbers))
                        listItemLinks.Add(WrapInListItem(options.DelimiterBetweenPageNumbers));

                    //show page number link
                    listItemLinks.Add(Page(i, list, href, options));
                }

                //if there are subsequent page numbers not displayed, show an ellipsis
                if (options.DisplayEllipsesWhenNotShowingAllPageNumbers && (firstPageToDisplay + pageNumbersToDisplay - 1) < list.PageCount)
                    listItemLinks.Add(Ellipses(options));
            }

            //next
            if (options.DisplayLinkToNextPage == PagedListDisplayMode.Always || (options.DisplayLinkToNextPage == PagedListDisplayMode.IfNeeded && !list.IsLastPage))
                listItemLinks.Add(Next(list, href, options));

            //last
            if (options.DisplayLinkToLastPage == PagedListDisplayMode.Always || (options.DisplayLinkToLastPage == PagedListDisplayMode.IfNeeded && lastPageToDisplay < list.PageCount))
                listItemLinks.Add(Last(list, href, options));

            if (listItemLinks.Any())
            {
                //append class to first item in list?
                if (!string.IsNullOrWhiteSpace(options.ClassToApplyToFirstListItemInPager))
                    listItemLinks.First().AddCssClass(options.ClassToApplyToFirstListItemInPager);

                //append class to last item in list?
                if (!string.IsNullOrWhiteSpace(options.ClassToApplyToLastListItemInPager))
                    listItemLinks.Last().AddCssClass(options.ClassToApplyToLastListItemInPager);

                //append classes to all list item links
                foreach (var li in listItemLinks)
                    foreach (var c in options.LiElementClasses ?? Enumerable.Empty<string>())
                        li.AddCssClass(c);
            }

            //collapse all of the list items into one big string
            var listItemLinksString = listItemLinks.Aggregate(
                new StringBuilder(),
                (sb, listItem) => sb.Append(TagBuilderToString(listItem)),
                sb => sb.ToString()
                );

            var ul = new TagBuilder("ul");
            ul.InnerHtml.AppendHtml(listItemLinksString);
            foreach (var c in options.UlElementClasses ?? Enumerable.Empty<string>())
                ul.AddCssClass(c);

            if (options.UlElementattributes != null)
            {
                foreach (var c in options.UlElementattributes)
                    ul.MergeAttribute(c.Key, c.Value);
            }

            var outerDiv = new TagBuilder("div");
            foreach (var c in options.ContainerDivClasses ?? Enumerable.Empty<string>())
                outerDiv.AddCssClass(c);
            outerDiv.InnerHtml.AppendHtml(TagBuilderToString(ul));
            return new HtmlString(TagBuilderToString(outerDiv));
        }
    }
}