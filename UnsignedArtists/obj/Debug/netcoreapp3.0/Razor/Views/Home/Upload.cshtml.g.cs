#pragma checksum "C:\Users\Trevor\Dropbox\dcc\capstone\Capstone\UnsignedArtists\Views\Home\Upload.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "70f1801e10d72ed1c9bbfadc8462011cc764b6d7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Upload), @"mvc.1.0.view", @"/Views/Home/Upload.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Trevor\Dropbox\dcc\capstone\Capstone\UnsignedArtists\Views\_ViewImports.cshtml"
using UnsignedArtists;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Trevor\Dropbox\dcc\capstone\Capstone\UnsignedArtists\Views\_ViewImports.cshtml"
using UnsignedArtists.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"70f1801e10d72ed1c9bbfadc8462011cc764b6d7", @"/Views/Home/Upload.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"84770e3de11b2c2aaa506925038af38ee6e5b982", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Upload : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\Trevor\Dropbox\dcc\capstone\Capstone\UnsignedArtists\Views\Home\Upload.cshtml"
  
    ViewData["Title"] = "Upload Page";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
    <div class=""text-center"">
        <h1 class=""display-4"">Upload</h1>
        <center>
            <div id=""container"">
                <h1>dCC Capstone</h1>
                <input type=""file"" id=""fileUploadInput"" name=""files"" multiple />
                <br />
                <progress></progress>
            </div>
        </center>
    </div>
");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
