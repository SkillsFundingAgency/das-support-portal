﻿using System;
using System.Web.WebPages;
using SFA.DAS.Support.Portal.Web.ViewModels;

#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SFA.DAS.Support.Portal.Web.Views.Search
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [PageVirtualPath("~/Views/Search/Detail.cshtml")]
    public partial class _Views_Search_Detail_cshtml : System.Web.Mvc.WebViewPage<DetailViewModel>
    {
        public _Views_Search_Detail_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Views\Search\Detail.cshtml"
  
    ViewBag.modelTitle = $"{Model.User.FirstName} {Model.User.LastName}";
    ViewBag.currentSection = "user";
    ViewBag.currentPage = "overview";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n    <h1>Overview</h1>\r\n\r\n");

            
            #line 10 "..\..\Views\Search\Detail.cshtml"
    
            
            #line default
            #line hidden
            
            #line 10 "..\..\Views\Search\Detail.cshtml"
     if (Model.User != null)
    {

            
            #line default
            #line hidden
WriteLiteral("        <dl>\r\n\r\n            <dt>Name:</dt>\r\n            <dd>");

            
            #line 15 "..\..\Views\Search\Detail.cshtml"
           Write(Model.User.FirstName);

            
            #line default
            #line hidden
WriteLiteral(" ");

            
            #line 15 "..\..\Views\Search\Detail.cshtml"
                                 Write(Model.User.LastName);

            
            #line default
            #line hidden
WriteLiteral("</dd>\r\n\r\n\r\n            <dt>Email:</dt>\r\n            <dd><a");

WriteAttribute("href", Tuple.Create(" href=\"", 438), Tuple.Create("\"", 469)
, Tuple.Create(Tuple.Create("", 445), Tuple.Create("mailto:", 445), true)
            
            #line 19 "..\..\Views\Search\Detail.cshtml"
, Tuple.Create(Tuple.Create("", 452), Tuple.Create<System.Object, System.Int32>(Model.User.Email
            
            #line default
            #line hidden
, 452), false)
);

WriteLiteral(">");

            
            #line 19 "..\..\Views\Search\Detail.cshtml"
                                              Write(Model.User.Email);

            
            #line default
            #line hidden
WriteLiteral("</a></dd>\r\n\r\n            <dt>Status:</dt>\r\n            <dd>");

            
            #line 22 "..\..\Views\Search\Detail.cshtml"
           Write(Model.User.Status);

            
            #line default
            #line hidden
WriteLiteral("</dd>\r\n\r\n        </dl>\r\n");

            
            #line 25 "..\..\Views\Search\Detail.cshtml"
    }
    else
    {

            
            #line default
            #line hidden
WriteLiteral("        <p>User not found</p>\r\n");

            
            #line 29 "..\..\Views\Search\Detail.cshtml"
    }

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591