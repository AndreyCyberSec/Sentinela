using Application.InterfacesService.InterfaceScanner;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.ServiceDesign
{
    public class ScannerWebService : IScannerWebService
    {
        public async Task ScannerWebApplication()
        {
            var title = new FigletText("Sentinela")
      .Color(Color.Green);

            var subtitle = new Markup("[italic grey]Welcome the tools of Sentinela[/]");
            var subtitle1 = new Markup("[italic grey]Version: 1.0.0[/]");
            var subtitle2 = new Markup("[italic grey]Development by Andrey[/]");

            var titleAligned = Align.Center(title);
            var subtitleAligned = Align.Left(subtitle);
            var subtitle1Aligned = Align.Left(subtitle1);
            var subtitle2Aligned = Align.Left(subtitle2);

            AnsiConsole.Write(titleAligned);
            AnsiConsole.Write(subtitleAligned);
            AnsiConsole.Write(subtitle1Aligned);
            AnsiConsole.Write(subtitle2Aligned);
            AnsiConsole.WriteLine();
            await AnsiConsole.Status()
                      .Spinner(Spinner.Known.Binary)
                     .StartAsync("Loading...", async ctx =>
                     {
                         await Task.Delay(1000);
                     });
        }

    }
}
