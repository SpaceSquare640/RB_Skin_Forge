using Microsoft.Extensions.Logging;
using RB_Skin_Forge.Core.Abstractions;
using RB_Skin_Forge.Core.Services;

namespace RB_Skin_Forge.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// RB_Skin_Forge Core pipeline.
		builder.Services.AddSingleton<IAssetIngestionEngine, AssetIngestionEngine>();
		builder.Services.AddSingleton<ITemplateGenerator, TemplateGenerator>();
		builder.Services.AddSingleton<ISpecValidator, RobloxSpecValidator>();
		builder.Services.AddSingleton<IImageProcessor, ImageProcessor>();          // Phase 1: images
		builder.Services.AddSingleton<ObjParser>();
		builder.Services.AddSingleton<MeshRenderer>();
		builder.Services.AddSingleton<IGeometryEngine, GeometryEngine>();          // Phase 2: OBJ meshes
		builder.Services.AddSingleton<IAssetPipeline, AssetPipeline>();            // routes by type
		builder.Services.AddTransient<MainPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
