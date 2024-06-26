﻿using AssetRipper.Assets.Bundles;
using AssetRipper.Assets.IO;
using AssetRipper.Import.Structure.Platforms;

namespace AssetRipper.Import.Structure;

internal sealed partial record class GameInitializer : DefaultGameInitializer
{
	public UnityVersion TargetVersion { get; }

	public GameInitializer(PlatformGameStructure? platformStructure, PlatformGameStructure? mixedStructure, UnityVersion defaultVersion, UnityVersion targetVersion)
		: base(new StructureDependencyProvider(platformStructure, mixedStructure), new CustomResourceProvider(platformStructure, mixedStructure), defaultVersion)
	{
		TargetVersion = targetVersion;
	}

	public override void OnPathsLoaded(GameBundle gameBundle, AssetFactoryBase assetFactory)
	{
		EngineResourceInjector.InjectEngineFilesIfNecessary(gameBundle, TargetVersion);
	}

	public override void OnDependenciesInitialized(GameBundle gameBundle, AssetFactoryBase assetFactory)
	{
		if (TargetVersion != default)
		{
			VersionChanger.ChangeVersions(gameBundle.FetchAssetCollections(), TargetVersion);
		}
	}
}
