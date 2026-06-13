using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RB_Skin_Forge.Core.Abstractions;
using RB_Skin_Forge.Core.Localization;
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
builder.Services.AddSingleton<FbxParser>();                                // Phase 3: ASCII FBX
builder.Services.AddSingleton<MeshDecimator>();                            // Phase 3: decimation
builder.Services.AddSingleton<AutoRigger>();                               // Phase 3: auto-rigging
builder.Services.AddSingleton<IAutoRigger>(sp => sp.GetRequiredService<AutoRigger>());
builder.Services.AddSingleton<MeshRenderer>();
builder.Services.AddSingleton<IGeometryEngine, GeometryEngine>();          // Phase 2/3: OBJ + FBX meshes
builder.Services.AddSingleton<IAssetPipeline, AssetPipeline>();            // routes by type

// Localization (shared 10-language UI string table).
builder.Services.AddSingleton<Localizer>();

await builder.Build().RunAsync();
