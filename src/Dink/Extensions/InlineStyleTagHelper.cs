using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Dink.Extensions {
    public class InlineStyleTagHelper : TagHelper {

        readonly ILogger<InlineStyleTagHelper> _logger;

        public InlineStyleTagHelper(IWebHostEnvironment hostingEnvironment, ILogger<InlineStyleTagHelper> logger) {
            HostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        private IWebHostEnvironment HostingEnvironment { get; }

        [HtmlAttributeName("href")]
        public string Href { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            var path = Href;
            IFileProvider fileProvider = HostingEnvironment.WebRootFileProvider;
            IFileInfo file = fileProvider.GetFileInfo(path);
            if (file == null)
                return;

            string fileContent = await ReadFileContent(file);
            if (fileContent == null) {
                output.SuppressOutput();
                return;
            }

            _logger.LogInformation(fileContent);

            // Generate the output
            output.TagName = "style"; // Change the name of the tag from inline-style to style
            output.Attributes.RemoveAll("href"); // href attribute is not needed anymore
            output.Content.AppendHtml(fileContent);
        }

        private static async Task<string> ReadFileContent(IFileInfo file) {
            using (var stream = file.CreateReadStream())
            using (var textReader = new StreamReader(stream)) {
                return await textReader.ReadToEndAsync();
            }
        }
    }
}