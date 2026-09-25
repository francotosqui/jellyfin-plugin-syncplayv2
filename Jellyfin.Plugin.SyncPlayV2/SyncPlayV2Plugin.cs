using System;
using Jellyfin.Plugin.SyncPlayV2.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.SyncPlayV2;

/// <summary>
/// SyncPlay protocol v2 as a server plugin: the phase-1 engine (versioned
/// state, snapshots, beacons, adaptive tolerances, disconnect grace) serving
/// stock v1 clients and v2 clients from one group registry. Shadows the core
/// ISyncPlayManager while installed; disable the plugin to restore stock
/// SyncPlay.
/// </summary>
public class SyncPlayV2Plugin : BasePlugin<PluginConfiguration>
{
    public SyncPlayV2Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
        : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
    }

    /// <summary>
    /// Gets the plugin instance.
    /// </summary>
    public static SyncPlayV2Plugin? Instance { get; private set; }

    /// <inheritdoc />
    public override Guid Id => new Guid("181f9934-bf71-4941-974e-a5f2cdcccc4e");

    /// <inheritdoc />
    public override string Name => "SyncPlay v2";

    /// <inheritdoc />
    public override string Description
        => "SyncPlay protocol v2: versioned state, snapshots, position beacons, adaptive tolerances, bounded group-waits and robust reconnects — serving stock v1 clients and v2 clients from one group registry, replacing the built-in SyncPlay while installed.";
}
