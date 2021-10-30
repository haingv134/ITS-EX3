using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebLayer.ExtensionMethod
{
    public static class MyExtension
    {
        public static void UseCustomeErrorCode(this IApplicationBuilder app)
        {
            app.UseStatusCodePages(appBuider =>
            {
                appBuider.Run(async context =>
                {
                    var code = context.Response.StatusCode;
                    var content = @$"<html>
                        <head>
                               <meta charset='UTF=8' />
                               <title> Error: {code} </title>
                        </head>
                        <body>
                             <p style=""background-color: red""> ERROR Name: {(HttpStatusCode)code} </p>
                         </body>
                                    </html>";
                    await context.Response.WriteAsync(content);
                });
            });
        }
    }
}
