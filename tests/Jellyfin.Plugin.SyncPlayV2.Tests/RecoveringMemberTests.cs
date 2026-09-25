using System;
using Jellyfin.Plugin.SyncPlayV2.Tests.Harness;
using MediaBrowser.Model.SyncPlay;
using Xunit;

namespace Jellyfin.Plugin.SyncPlayV2.Tests;

/// <summary>
/// A member that rebuffered and reports ready behind the group: the group
/// resumes when the member will have caught up (#15).
/// </summary>
public class RecoveringMemberTests
{
    private static readonly long Minute = TimeSpan.FromMinutes(1).Ticks;

    [Fact]
    public void ARecoveringMemberThatIsAlreadyPlayingIsToldTheGroupResumed()
    {
        // jellyfin-web answers the group's Ready state update with its
        // "schedule-play" indicator and only clears it on a scheduled
        // Unpause; left out of the Unpause, the recovering member shows the
        // indicator over its playing video for good.
        var harness = new GroupHarness();
        var a = harness.Join("a");
        var b = harness.Join("b");
        harness.StartPlaying(new[] { a, b }, Minute);

        a.Buffer(Minute, isPlaying: true);
        Assert.Equal(GroupStateType.Waiting, harness.State);

        a.Ready(Minute - TimeSpan.FromSeconds(9).Ticks, isPlaying: true);

        Assert.Equal(GroupStateType.Playing, harness.State);
        Assert.Contains(b.Commands, command => command.Command == "Unpause");
        Assert.Contains(a.Commands, command => command.Command == "Unpause");
    }
}
