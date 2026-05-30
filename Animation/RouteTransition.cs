using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OneDriveDriver.Desktop.Animation;

public class RouteTransition : IPageTransition {
    private const double OffsetY = 12d;

    public RouteTransition() { }

    public RouteTransition(TimeSpan duration) {
        Duration = duration;
    }

    public TimeSpan Duration { get; set; }

    public async Task Start(
        Visual? from,
        Visual? to,
        bool forward,
        CancellationToken cancellationToken
    ) {
        if (cancellationToken.IsCancellationRequested) {
            return;
        }

        var tasks = new List<Task>();
        var easing = new CircularEaseOut();
        var fromTransform = from?.RenderTransform;
        var toTransform = to?.RenderTransform;

        if (from != null) {
            from.RenderTransform = new TranslateTransform();
            from.Opacity = 1;

            tasks.Add(RunPageAnimation(from, 0d, OffsetY, 1d, 0d, easing, cancellationToken));
        }

        if (to != null) {
            to.RenderTransform = new TranslateTransform { Y = OffsetY };
            to.Opacity = 0;
            to.IsVisible = true;

            tasks.Add(RunPageAnimation(to, OffsetY, 0d, 0d, 1d, easing, cancellationToken));
        }

        try {
            await Task.WhenAll(tasks);
        } catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) {
            return;
        } finally {
            if (from != null) {
                from.RenderTransform = fromTransform;
            }

            if (to != null) {
                to.RenderTransform = toTransform;
            }
        }

        if (from != null && !cancellationToken.IsCancellationRequested) {
            from.IsVisible = false;
            from.Opacity = 1;
        }

        if (to != null && !cancellationToken.IsCancellationRequested) {
            to.Opacity = 1;
        }
    }

    private Task RunPageAnimation(
        Visual target,
        double fromY,
        double toY,
        double fromOpacity,
        double toOpacity,
        Easing easing,
        CancellationToken cancellationToken
    ) {
        return new Avalonia.Animation.Animation {
            FillMode = FillMode.Forward,
            Duration = Duration,
            Easing = easing,
            Children = {
                new KeyFrame {
                    Cue = new Cue(0d),
                    Setters = {
                        new Setter { Property = TranslateTransform.YProperty, Value = fromY },
                        new Setter { Property = Visual.OpacityProperty, Value = fromOpacity }
                    }
                },
                new KeyFrame {
                    Cue = new Cue(1d),
                    Setters = {
                        new Setter { Property = TranslateTransform.YProperty, Value = toY },
                        new Setter { Property = Visual.OpacityProperty, Value = toOpacity }
                    }
                }
            }
        }.RunAsync(target, cancellationToken);
    }
}