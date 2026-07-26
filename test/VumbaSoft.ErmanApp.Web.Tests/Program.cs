using Microsoft.AspNetCore.Builder;
using VumbaSoft.ErmanApp;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("VumbaSoft.ErmanApp.Web.csproj"); 
await builder.RunAbpModuleAsync<ErmanAppWebTestModule>(applicationName: "VumbaSoft.ErmanApp.Web");

public partial class Program
{
}
