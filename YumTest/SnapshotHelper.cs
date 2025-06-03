using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YumTest
{
    public class SnapshotHelper
    {
        public static string WrapWithStyles(string markup)
        {
            var cssPath = ResolveCssPath();

            return @$"
                <!DOCTYPE html>
                <html lang=""en"">

                    <head>
                        <meta charset=""utf-8"" />
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
                        <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.5/dist/css/bootstrap.min.css"" rel=""stylesheet"" integrity=""sha384-SgOJa3DmI69IUzQ2PVdRZhwQ+dy64/BUtbMJw1MZ8t5HZApcHrRKUc4W0kG879m7"" crossorigin=""anonymous"">
                        <link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css"">
                        <link rel=""stylesheet"" href=""{cssPath}/app.css"" />
                    </head>
                    <body>
                        <div class=""review-container"">
                            {markup}
                        </div>
                    </body>

                </html>
            ";
        }

        private static string ResolveCssPath()
        {
            // Strategy 1: Check if wwwroot was copied to output
            if (Directory.Exists("wwwroot"))
                return "wwwroot";

            // Strategy 2: Relative path to main project
            // C:\work\Yum\YumTest\bin\Debug\net9.0
            if (Directory.Exists("../../../../wwwroot"))
                return "../../../wwwroot";

            return string.Empty;
        }
    }
}
