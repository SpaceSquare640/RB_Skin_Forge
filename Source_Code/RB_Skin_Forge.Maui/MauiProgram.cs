using Microsoft.Extensions.Logging;
using RB_Skin_Forge.Core.Abstractions;
using RB_Skin_Forge.Core.Localization;
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
			builder.Services.AddSingleton<FbxParser>();                                // Phase 3: ASCII FBX
			builder.Services.AddSingleton<MeshDecimator>();                            // Phase 3: decimation
			builder.Services.AddSingleton<AutoRigger>();                               // Phase 3: auto-rigging
			builder.Services.AddSingleton<IAutoRigger>(sp => sp.GetRequiredService<AutoRigger>());
		builder.Services.AddSingleton<MeshRenderer>();
		builder.Services.AddSingleton<IGeometryEngine, GeometryEngine>();          // Phase 2: OBJ meshes
		builder.Services.AddSingleton<IAssetPipeline, AssetPipeline>();            // routes by type
		builder.Services.AddSingleton<Localizer>();                                // 10-language UI strings
			builder.Services.AddTransient<MainPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
