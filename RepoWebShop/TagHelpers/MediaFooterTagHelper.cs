﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using RepoWebShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepoWebShop.TagHelpers
{
    public class MediaFooterTagHelper : TagHelper
    {
        public IEnumerable<MediaViewModel> Media { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "section";
            output.Attributes.SetAttribute("id", "contact");
            output.Attributes.SetAttribute("class", "contact");
            output.Content.AppendHtml(
            @"<div class='container' >
                <div class='row'>
                    <div class='col-lg-6 col-md-6 hidden-sm hidden-xs text-center'>
                        <h2 class='section-heading'>
                            Seguinos
                            <i class='fa fa-heart fa-beat'></i>
                        </h2>
                    </div>
                    <div class='col-lg-6 col-md-6 col-sm-6 col-xs-6 text-center'>
                        <ul class='list-inline list-social'>"
                            +
                            String.Concat(Media.Select(x =>
                            $"<li class='list-inline-item {x.Class}'>" +
                                $"<a href='{x.Ref}'>" +
                                    $"<i class='fa {x.Icon}'></i>" + 
                                @"</a>
                            </li>"))
                            +
                        @"</ul>
                    </div>
                    <div class='hidden-lg hidden-md col-sm-6 col-xs-6 text-center'>
                        <h2 class='section-heading'>
                            &zwnj;
                        </h2>
                    </div>
                </div>
            </div>");
        }
    }
}
