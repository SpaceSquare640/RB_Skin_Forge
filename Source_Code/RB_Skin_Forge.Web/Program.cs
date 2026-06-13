using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RB_Skin_Forge.Core.Abstractions;
using RB_Skin_Forge.Core.Services;
using RB_Skin_Forge.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// RB_Skin_Forge Core pipeline.
builder.Services.AddSingleton<IAssetIngestionEngine, AssetIngestionEngine>();
builder.Services.AddSingleton<ITemplateGenerator, TemplateGenerator>();
builder.Services.AddSingleton<ISpecValidator, RobloxSpecValidator>();
builder.Services.AddSingleton<IImageProcessor, ImageProcessor>();          // Phase 1: images
builder.Services.AddSingleton<ObjParser>();
builder.Services.AddSingleton<MeshRenderer>();
builder.Services.AddSingleton<IGeometryEngine, GeometryEngine>();          // Phase 2: OBJ meshes
builder.Services.AddSingleton<IAssetPipeline, AssetPipeline>();            // routes by type

await builder.Build().RunAsync();
